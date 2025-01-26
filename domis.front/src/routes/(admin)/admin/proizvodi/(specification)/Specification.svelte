<script lang="ts">
  import { putProduct } from "../../../../../services/product-service";
  import RadioButton from "../../../../../components/RadioButton.svelte";
  import { QuantityType, quantityTypeOptions } from "../../../../../enums";
  import Toggle from "../../../../../components/Toggle.svelte";
  import Input from "../Input.svelte";
  import { createEventDispatcher } from "svelte";
  import { snackbarStore } from "../../../../../stores/snackbar";

  export let id: number;
  export let attributes: ProductAttributes;
  export let initDescription: string | undefined;
  export let size: ProductSizing;
  export let initIsActive: boolean | undefined;
  export let name: string;

  const dispatch = createEventDispatcher<{ save: string }>();

  let title = attributes.title ?? "";
  let description = initDescription ?? "";
  let height = attributes.height ?? null;
  let width = attributes.width ?? null;
  let depth = attributes.depth ?? null;
  let length = attributes.length ?? null;
  let thickness = attributes.thickness ?? null;
  let weight = attributes.weight ?? null;
  let box = size?.box ?? 0;
  let pallet = size?.pallet ?? 0;
  let quantityType =
    attributes.quantityType === undefined
      ? QuantityType.None
      : attributes.quantityType;
  let isActive = initIsActive !== undefined ? initIsActive : true;

  const errors = {
    height: "",
    width: "",
    depth: "",
    length: "",
    thickness: "",
    weight: "",
    box: "",
    pallet: "",
    salePrice: 0,
  };

  function validate() {
    if (height && isNaN(Number(height))) {
      errors.height = "Vrednost mora biti broj";
    } else errors.height = "";

    if (width && isNaN(Number(width))) {
      errors.width = "Vrednost mora biti broj";
    } else errors.width = "";

    if (depth && isNaN(Number(depth))) {
      errors.depth = "Vrednost mora biti broj";
    } else errors.depth = "";

    if (length && isNaN(Number(length))) {
      errors.length = "Vrednost mora biti broj";
    } else errors.length = "";

    if (thickness && isNaN(Number(thickness))) {
      errors.thickness = "Vrednost mora biti broj";
    } else errors.thickness = "";

    if (weight && isNaN(Number(weight))) {
      errors.weight = "Vrednost mora biti broj";
    } else errors.weight = "";

    if (box && isNaN(Number(box))) {
      errors.box = "Vrednost mora biti broj";
    } else errors.box = "";

    if (pallet && isNaN(Number(pallet))) {
      errors.pallet = "Vrednost mora biti broj";
    } else errors.pallet = "";
  }

  async function submit() {
    let res = await putProduct({
      id: id,
      title: title,
      description: description,
      width: width,
      height: height,
      depth: depth,
      length: length,
      thickness: thickness,
      box: box > 0 ? box : null,
      pallet: pallet > 0 ? pallet : null,
      isActive: isActive,
      quantityType: quantityType,
    });

    if (res) {
      snackbarStore.showSnackbar("Uspešno sačuvan proizvod!", true);
      dispatch("save", "saved");
    } else {
      snackbarStore.showSnackbar("Greška pri čuvanju proizvoda!", false);
    }
  }

  function validateAndSubmit(event: Event) {
    event.preventDefault(); // prevent page from scrolling to top

    validate();
    if (
      errors.height ||
      errors.width ||
      errors.depth ||
      errors.length ||
      errors.thickness ||
      errors.weight
    )
      return;
    submit();
  }
</script>

<div class="flex flex-col w-full px-2 lg:px-12 gap-y-6">
  <div class="relative w-full flex flex-col">
    <div class="flex justify-between">
      <div class="flex flex-col">
        <h2
          class="mt-4 mb-2 text-md lg:text-xl text-wrap"
          placeholder="Postojeći naziv proizvoda"
        >
          {name}
        </h2>
        <Toggle
          bind:isActive
          activeText="Proizvod je aktivan"
          inActiveText="Proizvod nije aktivan"
        />
      </div>
    </div>

    <form id="product-form" class="w-full mt-8" on:submit|preventDefault>
      <div class="flex flex-col gap-y-4">
        <Input
          bind:value={title}
          title="Naslov"
          placeholder="Unesite naziv proizvoda u što manje reči"
          width="116"
          gap="12"
          error=""
        />

        <div class="flex flex-wrap gap-y-4 gap-x-3 lg:gap-x-8">
          <Input
            bind:value={width}
            title="Širina"
            suffix="cm"
            placeholder="0.00"
            width="24"
            gap="12"
            error={errors?.width}
          />
          <Input
            bind:value={height}
            title="Visina"
            suffix="cm"
            placeholder="0.00"
            width="24"
            gap="12"
            error={errors?.height}
          />
          <Input
            bind:value={depth}
            title="Dubina"
            suffix="cm"
            placeholder="0.00"
            width="24"
            gap="12"
            error={errors?.depth}
          />
        </div>
        <div class="flex flex-wrap gap-y-4 gap-x-3 lg:gap-x-8">
          <Input
            bind:value={length}
            title="Dužina"
            suffix="cm"
            placeholder="0.00"
            width="24"
            gap="12"
            error={errors?.length}
          />
          <Input
            bind:value={thickness}
            title="Debljina"
            suffix="cm"
            placeholder="0.00"
            width="24"
            gap="12"
            error={errors?.thickness}
          />
          <Input
            bind:value={weight}
            title="Težina"
            suffix="g"
            placeholder="0.00"
            width="24"
            gap="12"
            error={errors?.weight}
          />
        </div>

        <!-- Pak and pal -->
        <div class="mt-6 flex gap-x-3 lg:gap-x-8">
          <Input
            bind:value={box}
            title="Pak"
            placeholder="0.00"
            width="16"
            gap="6"
            error={errors?.box}
          />
          <Input
            bind:value={pallet}
            title="Pal"
            placeholder="0.00"
            width="16"
            gap="6"
            error={errors?.pallet}
          />
        </div>

        <RadioButton
          options={quantityTypeOptions}
          legend="Odaberite tip proizvoda:"
          bind:userSelected={quantityType}
        />

        <textarea
          name="textarea"
          id="product-form"
          rows={5}
          value={description}
          class="h-auto block w-full rounded-md border-0 py-1.5 pl-3 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-blue-600 text-md leading-6"
          placeholder="Unesite opis proizvoda"
        ></textarea>

        <button
          on:click={validateAndSubmit}
          type="submit"
          class="mt-4 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm max-w-96 px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
          >Sačuvaj</button
        >
      </div>
    </form>
  </div>
</div>
