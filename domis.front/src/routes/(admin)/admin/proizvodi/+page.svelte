<script lang="ts">
  import { error } from "@sveltejs/kit";
  import { getProductSaleHistory } from "../../../../services/admin-service";
  import {
    getCategoryProductsBasicInfo,
    getProduct,
    postProduct,
  } from "../../../../services/product-service";
  import { categories } from "../../../../stores/categories";
  import Sale from "./(sale)/Sale.svelte";
  import Specification from "./(specification)/Specification.svelte";
  import AdminCategoryList from "./AdminCategoryList.svelte";
  import Input from "./Input.svelte";
  import Snackbar from "../../../../components/Snackbar.svelte";

  enum ViewOptions {
    Spec,
    Sale,
    Prices,
    Images,
  }

  interface FlattenedCategory {
    id: string;
    name: string;
    level: number;
  }

  let snackbarMessage: string;
  let isSnackbarSuccess: boolean;
  let showSnackbar = false;

  let selectedCategoryId: string;
  let selectedProductId: number = 0;

  let flattenedCategories: Array<FlattenedCategory>;
  $: categories.subscribe((value) => {
    flattenedCategories = flattenCategories(value);
  });

  let name = "";
  let sku = "";
  let errorName = "";
  let errorSku = "";

  let productsList: Array<Product>;
  let selectedProduct: Product | null = null;
  let saleHistory: Array<SaleInfo>;

  let viewOption = ViewOptions.Spec;

  let showNewProductModal = false;

  $: if (selectedCategoryId) {
    selectedProductId = 0;
    setProducts();
  }

  async function setProducts() {
    productsList = await getCategoryProductsBasicInfo(selectedCategoryId);
  }

  async function setSelectedProduct(value: number) {
    if (value === undefined) return;
    selectedProduct = null;
    selectedProduct = { ...(await getProduct(value)) };
    setSaleHistory(value);
  }

  async function setSaleHistory(value: number) {
    saleHistory = await getProductSaleHistory(value);
  }

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
      if (res.id) {
        selectedProductId = res.id;
        setSelectedProduct(selectedProductId);
      }
    }
    handleShowSnackbar();
  }

  async function handleSaveSaleHistory() {
    if (selectedProduct) await setSaleHistory(selectedProduct.id);
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
        {#if productsList}
          <div class="flex flex-col gap-y-2 relative w-60">
            <h2 class="font-bold">Proizvodi</h2>
            <select
              bind:value={selectedProductId}
              on:change={() => setSelectedProduct(selectedProductId)}
              class="block w-full border rounded-lg px-2 py-1 bg-white text-left font-light"
            >
              <option disabled value={0}>Izaberite proizvod</option>
              {#each productsList as product}
                <option value={product.id}>
                  {product.name}
                </option>
              {/each}
            </select>
          </div>
        {/if}
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

    {#if selectedProduct}
      <div class="flex text-sm lg:text-md mt-8">
        <button
          on:click={() => (viewOption = ViewOptions.Spec)}
          class="w-28 lg:w-36 text-center tracking-wider py-3 hover:bg-gray-100 {viewOption ===
          ViewOptions.Spec
            ? 'bg-gray-100'
            : ''}">Specifikacija</button
        >
        <button
          on:click={() => (viewOption = ViewOptions.Sale)}
          class="w-28 lg:w-36 text-center tracking-wider py-3 hover:bg-gray-100 {viewOption ===
          ViewOptions.Sale
            ? 'bg-gray-100'
            : ''}">Popusti</button
        >
        <!-- <button
        on:click={() => (viewOption = ViewOptions.Prices)}
        class="w-20 lg:w-36 text-center tracking-wider py-3 hover:bg-gray-100 {viewOption ===
        ViewOptions.Prices
          ? 'bg-gray-50'
          : ''}">Cene</button
      > -->
      </div>

      <div class="py-4 bg-gray-100">
        {#if viewOption === ViewOptions.Spec}
          <Specification
            attributes={selectedProduct.attributes}
            initDescription={selectedProduct.description}
            size={selectedProduct.size}
            initIsActive={selectedProduct.isActive}
            name={selectedProduct.name}
            id={selectedProduct.id}
          />
        {:else if viewOption === ViewOptions.Sale}
          <Sale
            on:save={handleSaveSaleHistory}
            {saleHistory}
            initialPrice={selectedProduct.price?.perUnit}
            productId={selectedProduct.id}
          />
          <!-- {:else if viewOption === ViewOptions.Prices}
        <Prices
          on:save={handleSavePrices}
          {prices}
          initialPrice={selectedProduct.price.perUnit}
          productId={selectedProduct.id}
        /> -->
        {/if}
      </div>
    {/if}
    <Snackbar
      message={snackbarMessage}
      isSuccess={isSnackbarSuccess}
      {showSnackbar}
    />
  </div>
</div>
