<script lang="ts">
  import {
    formatPrice,
    formatToTwoDecimals,
  } from "../../../../helpers/numberFormatter";

  export let isExtraChecked: boolean;
  export let productPrice: ProductPricing;
  export let productSize: ProductSizing;

  let boxInput: number = 0;
  let meterSqInput: number = 0;
  let totalPrice: number = 0;
  let wordEnding: string;

  $: if (boxInput) {
    setTotalPrice();
    getCorrectWordEnding();
  }

  function handleMeterSqInputChanged() {
    if (isExtraChecked)
      boxInput = Math.ceil((1.1 * meterSqInput) / productSize.box);
    else boxInput = Math.ceil(meterSqInput / productSize.box);
  }
  function handleBoxInputChanged() {
    boxInput = Math.round(boxInput);
    if (isExtraChecked)
      meterSqInput = formatToTwoDecimals((boxInput * productSize.box) / 1.1);
    else meterSqInput = formatToTwoDecimals(boxInput * productSize.box);
  }
  function handleExtraClicked() {
    isExtraChecked = !isExtraChecked;
    handleMeterSqInputChanged();
  }

  function selectText(event: Event) {
    const input = event.target as HTMLInputElement;
    input.select();
  }
  function getCorrectWordEnding() {
    if ([2, 3, 4].includes(boxInput % 10) && Math.floor(boxInput / 10) !== 1)
      wordEnding = "e";
    else wordEnding = "a";
  }

  function setTotalPrice() {
    const boxesPerPallet = productSize.pallet / productSize.box;
    const palletNumber = Math.floor(boxInput / boxesPerPallet);
    const remainingBoxes = boxInput % boxesPerPallet;

    totalPrice =
      palletNumber > 0
        ? palletNumber * productPrice.perPallet +
          remainingBoxes * productPrice.perBox
        : boxInput * productPrice.perBox;
  }
</script>

<div class="flex flex-col mt-4">
  <!-- Quantity -->
  <div class="flex gap-x-4">
    <input
      type="number"
      placeholder="Unesite broj m2"
      class="w-full py-3 px-5 outline-none border border-gray-400 text-sm font-extralight tracking-wider"
      bind:value={meterSqInput}
      on:input={handleMeterSqInputChanged}
      on:click={selectText}
    />
    <input
      type="number"
      placeholder="ili unesite broj pakovanja"
      class="w-full py-3 px-5 outline-none border border-gray-400 text-sm font-extralight tracking-wider"
      bind:value={boxInput}
      on:input={handleBoxInputChanged}
      on:click={selectText}
    />
  </div>
  <!-- Checkbox -->
  <button
    on:click={handleExtraClicked}
    class="relative flex mt-4 gap-x-2 items-center"
  >
    <input
      type="checkbox"
      class="appearance-none h-5 w-5 cursor-pointer rounded-md border border-green-700 checked:bg-green-700"
      checked={isExtraChecked}
    />
    <span class="absolute text-white left-0.5">
      <svg
        xmlns="http://www.w3.org/2000/svg"
        class="h-3.5 w-3.5"
        viewBox="0 0 20 20"
        fill="currentColor"
        stroke="currentColor"
        stroke-width="1"
      >
        <path
          fill-rule="evenodd"
          d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
          clip-rule="evenodd"
        ></path>
      </svg>
    </span>
    <p class="text-green-700 pt-1 text-sm font-extralight tracking-wider">
      Dodajte 10% za škart (preporučeno)
    </p>
  </button>

  <!-- Payment -->
  <div class="flex mt-6 justify-between">
    <p class="tracking-wider font-semibold text-lg">Ukupan iznos</p>
    <div class="flex flex-col">
      <p class="text-2xl">
        {formatPrice(totalPrice)} RSD
      </p>
      {#if boxInput > 0}
        <p class="text-gray-500 font-extralight">
          {boxInput} kutij{wordEnding} pokrivaju {formatToTwoDecimals(
            boxInput * productSize.box
          )} m²
        </p>
      {/if}
    </div>
  </div>
</div>
