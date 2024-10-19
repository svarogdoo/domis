<script lang="ts">
  import {
    getCategoryProductsBasicInfo,
    getProduct,
    putProduct,
  } from "../../../../services/product-service";
  import Input from "./Input.svelte";
  import Snackbar from "../../../../components/Snackbar.svelte";
  import AdminCategoryList from "./AdminCategoryList.svelte";
  import RadioButton from "../../../../components/RadioButton.svelte";
  import { QuantityType, quantityTypeOptions } from "../../../../enums";
  import { handleImageError } from "../../../../helpers/imageFallback";

  let selectedCategoryId: string;

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
  let quantityType: QuantityType;
  let isActive: boolean = true;

  const errors = {
    height: "",
    width: "",
    depth: "",
    length: "",
    thickness: "",
    weight: "",
  };

  $: if (selectedCategoryId) {
    setProducts();
  }

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
    quantityType =
      selectedProduct.quantityType === undefined ||
      selectedProduct.quantityType === QuantityType.None
        ? QuantityType.Piece
        : selectedProduct.quantityType;
    isActive =
      selectedProduct.isActive !== undefined ? selectedProduct.isActive : true;
  }
  async function setProducts() {
    productsList = await getCategoryProductsBasicInfo(selectedCategoryId);
  }

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
      }, 3000); // Close after 3s
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
      isActive: isActive,
      quantityType: quantityType,
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

<div class="flex w-full px-12">
  <div class="flex flex-col w-1/5 broder-r border-r">
    <ul>
      <AdminCategoryList bind:selectedCategoryId />
    </ul>
  </div>
  {#if productsList}
    <ul
      class="flex flex-col w-1/5 border-r overflow-y-scroll max-h-fit font-light gap-y-2"
    >
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
  {#if !selectedProduct}
    <h2 class="mt-4 text-xl ml-12 font-light">
      Odaberite kategoriju a potom proizvod iz liste s leve strane
    </h2>
  {:else}
    <div class="relative w-3/5 flex flex-col ml-12">
      <div class="flex justify-between gap-x-4">
        <div class="flex flex-col">
          <h2 class="mt-4 text-xl" placeholder="Postojeći naziv proizvoda">
            {selectedProduct.name}
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
            <span class="font-semibold">Dimenzije:</span> Unesite samo vrednosti
            koje proizvod ima. Dimenzije su u centimetrima.
          </p>
          <p class="text-md font-light mt-2">
            <span class="font-semibold">Težina:</span> Vrednost je izražena u gramima.
          </p>
          <p class="text-md font-light mt-2">
            <span class="font-semibold">Neaktivni proizvodi:</span> Ukoliko je proizvod
            označen kao neaktivan on neće biti prikazan na sajtu.
          </p>
        </div>
        {#if selectedProduct?.featuredImageUrl}
          <img
            class="w-48 h-48 mr-12 rounded-md border"
            src={selectedProduct?.featuredImageUrl}
            alt=""
            on:error={handleImageError}
          />
        {/if}
      </div>

      <label class="inline-flex items-center cursor-pointer mt-8">
        <input type="checkbox" bind:checked={isActive} class="sr-only peer" />
        <div
          class="relative w-11 h-6 bg-gray-600 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-green-500"
        ></div>
        {#if isActive}
          <span class="ms-3 font-medium">Proizvod je aktivan</span>
        {:else}
          <span class="ms-3 font-medium">Proizvod nije aktivan</span>
        {/if}
      </label>

      <form id="product-form" class="w-full mt-8">
        <div class="flex flex-col gap-y-4">
          <Input
            value={title}
            title="Naslov"
            placeholder="Unesite naziv proizvoda u što manje reči"
            error=""
          />

          <Input
            bind:value={width}
            title="Širina"
            placeholder="0.00"
            error={errors?.width}
          />
          <Input
            bind:value={height}
            title="Visina"
            placeholder="0.00"
            error={errors?.height}
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
  {/if}
</div>
