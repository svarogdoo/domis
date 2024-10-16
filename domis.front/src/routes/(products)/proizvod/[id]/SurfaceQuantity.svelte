<script lang="ts">
  import { mapQuantityTypeToString, QuantityType } from "../../../../enums";
  import {
    formatPrice,
    formatToTwoDecimals,
  } from "../../../../helpers/numberFormatter";
  import { getPaketString } from "../../../../helpers/stringFormatter";
  import { cart } from "../../../../stores/cart";

  export let product: Product;
  export let isExtraChecked: boolean;
  export let quantityType: QuantityType;

  let boxInput: number = 0;
  let amountInput: number = 0;
  let totalPrice: number = 0;
  let quantityTypeString: string;

  $: if (quantityType) {
    quantityTypeString = mapQuantityTypeToString(quantityType);
  }

  $: if (boxInput) {
    setTotalPrice();
  }

  function handleAmountInputChanged() {
    if (quantityType === QuantityType.Piece) {
      amountInput = Math.round(amountInput);
      setTotalPrice();
    } else if (isExtraChecked)
      boxInput = Math.ceil((1.1 * amountInput) / product.size.box);
    else boxInput = Math.ceil(amountInput / product.size.box);
  }
  function handleBoxInputChanged() {
    boxInput = Math.round(boxInput);
    if (isExtraChecked)
      amountInput = formatToTwoDecimals((boxInput * product.size.box) / 1.1);
    else amountInput = formatToTwoDecimals(boxInput * product.size.box);
  }
  function handleExtraClicked() {
    isExtraChecked = !isExtraChecked;
    handleAmountInputChanged();
  }

  function selectText(event: Event) {
    const input = event.target as HTMLInputElement;
    input.select();
  }

  function setTotalPrice() {
    if (product.price.perBox)
      if (product.size.pallet && product.price.perPallet) {
        const boxesPerPallet = product.size.pallet / product.size.box;
        const palletNumber = Math.floor(boxInput / boxesPerPallet);
        const remainingBoxes = boxInput % boxesPerPallet;

        totalPrice =
          palletNumber > 0
            ? palletNumber * product.price.perPallet +
              remainingBoxes * product.price.perBox
            : boxInput * product.price.perBox;
      } else {
        totalPrice = boxInput * product.price.perBox;
      }
    if (product.price.perUnit) totalPrice = amountInput * product.price.perUnit;
  }

  function addItemToCart() {
    if (amountInput === 0) return;

    let cartProduct: CartProductDto = {
      productId: product.id,
      quantity: quantityType === QuantityType.Piece ? amountInput : boxInput,
    };

    cart.add(cartProduct);
  }
</script>

<div class="flex flex-col mt-4">
  <!-- Quantity -->
  <div class="flex gap-x-4">
    <div class="relative w-1/2">
      <input
        type="number"
        placeholder="Unesite broj {quantityTypeString}"
        class="w-full py-3 px-5 outline-none border border-gray-400 text-sm font-extralight tracking-wider"
        bind:value={amountInput}
        on:input={handleAmountInputChanged}
        on:click={selectText}
      />
      <p class="absolute inset-y-0 right-0 font-light flex items-center mr-2">
        {quantityTypeString}
      </p>
    </div>

    {#if quantityType !== QuantityType.Piece}
      <div class="relative w-1/2">
        <input
          type="number"
          placeholder="ili unesite broj paketa"
          class="w-full py-3 px-5 outline-none border border-gray-400 text-sm font-extralight tracking-wider"
          bind:value={boxInput}
          on:input={handleBoxInputChanged}
          on:click={selectText}
        />
        <p class="absolute inset-y-0 right-0 font-light flex items-center mr-2">
          {getPaketString(boxInput)}
        </p>
      </div>
    {/if}
  </div>
  <!-- Checkbox -->
  {#if quantityType !== QuantityType.Piece}
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
  {/if}

  <!-- Payment -->
  <div class="flex mt-6 justify-between">
    <p class="tracking-wider font-semibold text-md lg:text-lg">Ukupan iznos</p>
    <div class="flex flex-col">
      <p class="text-right text-lg lg:text-2xl">
        {formatPrice(totalPrice)} RSD
      </p>
      {#if boxInput > 0 && quantityType !== QuantityType.Piece}
        <p class="text-gray-500 text-sm lg:text-normal font-extralight">
          {boxInput}
          {getPaketString(boxInput)} pokriva
          {formatToTwoDecimals(boxInput * product.size.box)}
          {quantityTypeString}
        </p>
      {/if}
    </div>
  </div>
  <button
    on:click={addItemToCart}
    class="bg-black mt-2 text-white py-3 uppercase tracking-widest hover:bg-gray-700"
    >Dodaj u korpu</button
  >
</div>
