<script lang="ts">
  import { onMount } from "svelte";
  import {
    getProduct,
    getProducts,
    putProduct,
  } from "../../../../services/product-service";
  import Input from "./Input.svelte";
  import Snackbar from "../../../../components/Snackbar.svelte";

  let snackbarMessage: string;
  let isSnackbarSuccess: boolean;

  let productsList: Array<Product>;
  let selectedProduct: Product;
  let title: string;
  let description: string = "";
  let height: string;
  let width: string;
  let depth: string;
  let length: string;
  let thickness: string;
  let weight: string;
  let isSurfaceType: boolean = false;
  const errors = {
    height: "",
    width: "",
    depth: "",
    length: "",
    thickness: "",
    weight: "",
  };

  async function setSelectedProduct(value: Product) {
    selectedProduct = await getProduct(value.id);
    title = selectedProduct.title ? selectedProduct.title : "";
    description = selectedProduct.description
      ? selectedProduct.description
      : "";
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

  function showSnackbar() {
    var snackbar = document.getElementById("snackbar");
    if (snackbar) {
      snackbar.style.display = "flex";
      setTimeout(function () {
        if (snackbar) snackbar.style.display = "none";
      }, 3000); // Close the Snackbar after 3 seconds (adjust as needed)
    }
  }

  async function submit() {
    let res = await putProduct({
      id: selectedProduct.id,
      title: title,
      description: description,
      width: Number.parseInt(width),
      height: Number.parseInt(height),
      depth: Number.parseInt(depth),
      length: Number.parseInt(length),
      thickness: Number.parseInt(thickness),
      isSurfaceType: isSurfaceType,
      isItemType: !isSurfaceType,
    });

    if (res) {
      snackbarMessage = "Uspešno sačuvan proizvod!";
      isSnackbarSuccess = true;
    } else {
      snackbarMessage = "Greška pri čuvanju proizvoda!";
      isSnackbarSuccess = false;
    }
    showSnackbar();
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

<div class="flex w-full mx-4">
  {#if productsList}
    <ul class="flex flex-col w-1/4 h-lvh overflow-y-scroll border-r">
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
  <div class="relative w-3/4 flex flex-col ml-12">
    <div class="flex justify-between gap-x-4">
      <div class="flex flex-col">
        <h2 class="mt-4 text-xl" placeholder="Postojeći naziv proizvoda">
          {selectedProduct
            ? selectedProduct.name
            : "Odaberite proizvod iz liste"}
        </h2>
        <p class="mt-12 text-lg">Kako popuniti polja?</p>
        <p class="text-md font-light mt-4">
          <span class="font-semibold">Naslov:</span> Unesite naslov bez imena
          kategorije. Primer -
          <span class="italic"> Keramičke pločice Blend Graphite</span>
          → Naslov:
          <span class="italic"> Blend Graphite</span>
        </p>
        <p class="text-md font-light mt-2">
          <span class="font-semibold">Dimenzije:</span> Unesite samo vrednosti koje
          proizvod ima. Dimenzije su u centimetrima.
        </p>
        <p class="text-md font-light mt-2">
          <span class="font-semibold">Težina:</span> Vrednost je izražena u gramima.
        </p>
      </div>
      {#if selectedProduct?.featuredImageUrl}
        <img
          class="w-48 h-48 mr-12 rounded-md border"
          src={selectedProduct?.featuredImageUrl}
          alt=""
        />
      {/if}
    </div>

    <form id="product-form" class="w-full mt-8">
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
            bind:checked={isSurfaceType}
            class="sr-only peer"
          />
          <div
            class="relative w-11 h-6 bg-gray-600 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-blue-600"
          ></div>
          <span class="ms-3 font-medium">Proizvod se prodaje po površini</span>
        </label>
        <textarea
          name="textarea"
          id="product-form"
          rows={5}
          value={description}
          class="h-auto block w-2/3 rounded-md border-0 py-1.5 pl-3 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-blue-600 text-md leading-6"
          placeholder="Unesite opis proizvoda"
        ></textarea>

        <button
          on:click={validateAndSubmit}
          type="submit"
          class="mt-4 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm w-96 px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
          >Sačuvaj</button
        >
      </div>
    </form>
    <Snackbar message={snackbarMessage} isSuccess={isSnackbarSuccess} />
  </div>
</div>
