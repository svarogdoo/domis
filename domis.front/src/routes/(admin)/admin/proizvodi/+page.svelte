<script lang="ts">
  import { getProductSaleHistory } from "../../../../services/admin-service";
  import {
    getCategoryProductsBasicInfo,
    postProduct,
  } from "../../../../services/product-service";
  import { categories } from "../../../../stores/categories";
  import AdminCategoryList from "./AdminCategoryList.svelte";
  import Input from "./Input.svelte";
  import Snackbar from "../../../../components/Snackbar.svelte";
  import ProductsTable from "./ProductsTable.svelte";

  interface FlattenedCategory {
    id: string;
    name: string;
    level: number;
  }

  let snackbarMessage: string;
  let isSnackbarSuccess: boolean;
  let showSnackbar = false;

  let selectedCategoryId: string;

  let flattenedCategories: Array<FlattenedCategory>;
  $: categories.subscribe((value) => {
    flattenedCategories = flattenCategories(value);
  });

  let name = "";
  let sku = "";
  let errorName = "";
  let errorSku = "";

  let showNewProductModal = false;

  async function handleSaveNewProduct() {
    if (!name || name === "") errorName = "Morate uneti naziv proizvoda";
    else errorName = "";

    if (!sku || isNaN(Number(sku))) errorSku = "Vrednost mora biti broj";
    else errorSku = "";

    if (errorName || errorSku) return;

    let newProduct: NewProduct = {
      sku: Number(sku),
      name: name,
      categoryId: Number(selectedCategoryId),
    };

    let res = await postProduct(newProduct);

    showNewProductModal = false;

    if (!res || typeof res === "string") {
      snackbarMessage = "Greška pri čuvanju proizvoda! " + res;
      isSnackbarSuccess = false;
    } else {
      snackbarMessage = "Uspešno sačuvan proizvod!";
      isSnackbarSuccess = true;
      setProducts();
    }
    handleShowSnackbar();
  }

  function handleShowSnackbar() {
    showSnackbar = true;
    setTimeout(function () {
      showSnackbar = false;
    }, 3000); // Close after 3s
  }

  function flattenCategories(
    categories: Array<Category>,
    level: number = 0
  ): Array<FlattenedCategory> {
    return categories.flatMap((category) => {
      const { id, name, subcategories } = category;

      // Add the current category with the given level
      const current = { id, name, level };

      // Recursively flatten subcategories, if any
      const subcategoriesFlattened = subcategories
        ? flattenCategories(subcategories, level + 1)
        : [];

      // Combine the current category with its flattened subcategories
      return [current, ...subcategoriesFlattened];
    });
  }
</script>

<div class="flex flex-col w-full items-center">
  <div class="flex flex-col w-full lg:w-4/5">
    <!-- Product selection -->
    <div class="w-full flex justify-between items-end">
      <div class="flex flex-wrap gap-y-4 gap-x-2 lg:gap-x-8">
        <div class="w-60">
          <AdminCategoryList
            bind:selectedCategoryId
            categoriesList={flattenedCategories}
          />
        </div>
      </div>
      {#if selectedCategoryId}
        <button
          on:click={() => (showNewProductModal = true)}
          class="px-5 py-2 rounded-lg shadow-md bg-blue-400 hover:bg-blue-500 hover:shadow-lg text-white font-light"
          >Dodaj
        </button>
      {/if}
    </div>

    {#if showNewProductModal}
      <div
        class="fixed inset-0 w-full bg-black bg-opacity-50 flex items-center justify-center z-50"
      >
        <div class="bg-white w-auto py-8 px-16 flex flex-col gap-y-4">
          <p class="font-light text-center text-lg mb-4">
            {flattenedCategories.find((x) => x.id === selectedCategoryId)?.name}
          </p>
          <Input
            bind:value={name}
            title="Naslov"
            placeholder="Unesite naziv proizvoda"
            width="96"
            gap="12"
            error={errorName}
          />
          <Input
            bind:value={sku}
            title="SKU"
            placeholder="Unesite sku proizvoda"
            width="96"
            gap="12"
            error={errorSku}
          />
          <div class="w-full flex gap-x-2">
            <button
              on:click={handleSaveNewProduct}
              type="submit"
              class="mt-4 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium tracking-wider rounded-lg text-sm w-full px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
              >Sačuvaj</button
            >
            <button
              on:click={() => (showNewProductModal = false)}
              type="submit"
              class="mt-4 text-white bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium tracking-wider rounded-lg text-sm w-full px-5 py-2.5 text-center dark:bg-gray-600 dark:hover:bg-gray-700 dark:focus:ring-gray-800"
              >Poništi</button
            >
          </div>
        </div>
      </div>
    {/if}

    {#if selectedCategoryId}
      <ProductsTable {selectedCategoryId} />
    {/if}

    <Snackbar
      message={snackbarMessage}
      isSuccess={isSnackbarSuccess}
      {showSnackbar}
    />
  </div>
</div>
