<script lang="ts">
  import { onMount } from "svelte";
  import { getProducts } from "../../../../services/product-service";
  import Input from "./Input.svelte";

  let productsList: Array<Product>;
  let selectedProduct: Product;
  let title: string;
  let height: string;
  let width: string;
  let depth: string;
  let length: string;
  let thickness: string;
  let weight: string;
  //TODO: DESCRIPTION
  let isSurfaceType: boolean;
  const errors = {
    height: "",
    width: "",
    depth: "",
    length: "",
    thickness: "",
    weight: "",
  };

  function setSelectedProduct(value: Product) {
    selectedProduct = value;
    title = selectedProduct.title ? `${selectedProduct.title}` : "";
    height = selectedProduct.height ? `${selectedProduct.height}` : "";
    width = selectedProduct.width ? `${selectedProduct.width}` : "";
    depth = selectedProduct.depth ? `${selectedProduct.depth}` : "";
    length = selectedProduct.length ? `${selectedProduct.length}` : "";
    thickness = selectedProduct.thickness ? `${selectedProduct.thickness}` : "";
    weight = selectedProduct.weight ? `${selectedProduct.weight}` : "";
  }
  async function setProducts() {
    productsList = await getProducts();
  }
  onMount(() => {
    setProducts();
  });

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
  }

  function submit() {
    // set surfacetype
  }

  function validateAndSubmit() {
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

<div class="flex w-full mx-4">
  {#if productsList}
    <ul class="flex flex-col w-1/4 h-lvh overflow-y-scroll">
      {#each productsList as product}
        <button
          on:click={() => setSelectedProduct(product)}
          type="button"
          class="hover:bg-blue-500 hover:text-white px-2 rounded-lg text-left"
        >
          {product.name}
        </button>
      {/each}
    </ul>
  {/if}
  <div class="w-3/4 flex flex-col ml-12">
    <div>
      <h2 class="mt-4 text-xl" placeholder="Postojeći naziv proizvoda">
        {selectedProduct ? selectedProduct.name : "Odaberite proizvod iz liste"}
      </h2>
      {#if selectedProduct?.featuredImageUrl}
        <img class="w-96 h-96" src={selectedProduct?.featuredImageUrl} alt="" />
      {/if}
      {#if selectedProduct?.description}
        <textarea>{selectedProduct.description}</textarea>
      {/if}
    </div>

    <p class="mt-12 text-lg">Kako popuniti polja?</p>
    <p class="text-md font-light mt-4">
      <span class="font-semibold">Naslov:</span> Unesite naslov bez imena
      kategorije. Primer -
      <span class="italic"> Keramičke pločice Blend Graphite</span>
      → Naslov:
      <span class="italic"> Blend Graphite</span>
    </p>
    <p class="text-md font-light mt-2">
      <span class="font-semibold">Dimenzije:</span> Unesite samo vrednosti koje proizvod
      ima. Dimenzije su u centimetrima.
    </p>
    <p class="text-md font-light mt-2">
      <span class="font-semibold">Težina:</span> Vrednost je izražena u gramima.
    </p>

    <form class="w-full mt-8">
      <div class="flex flex-col gap-y-4">
        <Input
          value={title}
          title="Naslov"
          placeholder="Unesite naziv proizvoda u što manje reči"
          error=""
        />

        <Input
          bind:value={height}
          title="Visina"
          placeholder="0.00"
          error={errors?.height}
        />
        <Input
          bind:value={width}
          title="Širina"
          placeholder="0.00"
          error={errors?.width}
        />
        <Input
          bind:value={depth}
          title="Dubina"
          placeholder="0.00"
          error={errors?.depth}
        />
        <Input
          bind:value={length}
          title="Dužina"
          placeholder="0.00"
          error={errors?.length}
        />
        <Input
          bind:value={thickness}
          title="Debljina"
          placeholder="0.00"
          error={errors?.thickness}
        />
        <Input
          bind:value={weight}
          title="Težina"
          placeholder="0.00"
          error={errors?.weight}
        />

        <label class="inline-flex items-center cursor-pointer mt-2">
          <span class="mr-3 font-medium">Proizvod se prodaje po komadu</span>
          <input
            type="checkbox"
            bind:value={isSurfaceType}
            class="sr-only peer"
          />
          <div
            class="relative w-11 h-6 bg-gray-600 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-blue-600"
          ></div>
          <span class="ms-3 font-medium">Proizvod se prodaje po površini</span>
        </label>

        <button
          on:click={validateAndSubmit}
          type="submit"
          class="mt-4 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm w-96 px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
          >Sačuvaj</button
        >
      </div>
    </form>
  </div>
</div>
