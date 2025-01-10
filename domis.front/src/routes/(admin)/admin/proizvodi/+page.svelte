<script lang="ts">
  import { getProductSaleHistory } from "../../../../services/admin-service";
  import {
    getCategoryProductsBasicInfo,
    getProduct,
  } from "../../../../services/product-service";
  import Sale from "./Sale.svelte";
  import AdminCategoryList from "./specifikacija/AdminCategoryList.svelte";
  import Prices from "./specifikacija/Prices.svelte";
  import Specification from "./specifikacija/Specification.svelte";

  enum ViewOptions {
    Spec,
    Sale,
    Prices,
    Images,
  }

  let selectedCategoryId: string;
  let selectedProductId: number = 0;

  let productsList: Array<Product>;
  let selectedProduct: Product | null = null;
  let saleHistory: Array<SaleInfo>;

  let viewOption = ViewOptions.Spec;

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

  async function handleSaveSaleHistory() {
    if (selectedProduct) await setSaleHistory(selectedProduct.id);
  }
</script>

<div class="flex flex-col w-full items-center">
  <div class="flex flex-col w-full lg:w-4/5">
    <!-- Product selection -->
    <div class="flex gap-x-2 lg:gap-x-8">
      <div class="w-60">
        <AdminCategoryList bind:selectedCategoryId />
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
            initialPrice={selectedProduct.price.perUnit}
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
  </div>
</div>
