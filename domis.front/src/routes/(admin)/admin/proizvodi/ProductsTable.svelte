<script lang="ts">
  import { getProductSaleHistory } from "../../../../services/admin-service";
  import {
    getCategoryProductsBasicInfo,
    getProduct,
  } from "../../../../services/product-service";
  import Sale from "./(sale)/Sale.svelte";
  import Specification from "./(specification)/Specification.svelte";
  import ProductRow from "./ProductRow.svelte";
  import closeIcon from "$lib/icons/x-mark.svg";
  import { onMount } from "svelte";

  enum ViewOptions {
    Spec,
    Sale,
    Prices,
    Images,
  }

  export let selectedCategoryId: string;
  export let reloadKey: number;

  let products: Array<Product>;

  $: reloadKey, setProducts();
  $: selectedCategoryId, setProducts();

  async function setProducts() {
    products = await getCategoryProductsBasicInfo(selectedCategoryId);
  }

  let detailedProduct: Product | null = null;
  let saleHistory: Array<SaleInfo>;

  let showProductEdit = false;
  let viewOption = ViewOptions.Spec;

  async function setSelectedProduct(event: Event) {
    let productId = event.detail.id;
    if (productId === undefined) return;
    detailedProduct = null;
    detailedProduct = { ...(await getProduct(productId)) };
    setSaleHistory(productId);

    showProductEdit = true;
  }

  async function setSaleHistory(value: number) {
    saleHistory = await getProductSaleHistory(value);
  }

  async function handleSaveSaleHistory() {
    if (detailedProduct) await setSaleHistory(detailedProduct.id);
  }

  function handleSpecificationSave() {
    console.info("saveddd");
    setProducts();
  }

  const handleClickOutside = (event: MouseEvent) => {
    const searchField = document.getElementById("product-modal");
    if (searchField && !searchField.contains(event.target as Node)) {
      showProductEdit = false;
    }
  };

  onMount(() => {
    document.addEventListener("click", handleClickOutside);
    return () => {
      document.removeEventListener("click", handleClickOutside);
    };
  });
</script>

<div class="w-full py-6">
  <table class="w-full">
    <thead class=" bg-domis-dark text-white">
      <th class="max-w-12">SKU</th>
      <th>Naziv</th>
      <th>Status</th>
    </thead>
    <tbody class="divide-y divide-gray-200">
      {#if products && products.length > 0}
        {#each products as product}
          <ProductRow
            {product}
            on:click={(event) => setSelectedProduct(event)}
          />{/each}
      {/if}
    </tbody>
  </table>
</div>

{#if showProductEdit}
  <div
    class="fixed inset-0 w-full bg-black bg-opacity-50 lg:py-2 flex items-center justify-center z-50"
  >
    <div
      id="product-modal"
      class="relative bg-white w-auto h-full py-1 px-2 lg:py-8 lg:px-16 flex flex-col overflow-y-scroll"
    >
      <button
        class="absolute top-2 right-2 px-2 py-1 rounded-lg"
        on:click={() => (showProductEdit = false)}
      >
        <img
          src={closeIcon}
          alt=""
          class="w-6 h-auto hover:scale-125 transition ease-in-out"
        />
      </button>
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
      </div>
      <div class="py-4 bg-gray-100">
        {#if detailedProduct && viewOption === ViewOptions.Spec}
          <Specification
            attributes={detailedProduct.attributes}
            initDescription={detailedProduct.description}
            size={detailedProduct.size}
            initIsActive={detailedProduct.isActive}
            name={detailedProduct.name}
            id={detailedProduct.id}
            on:save={handleSpecificationSave}
          />
        {:else if detailedProduct && viewOption === ViewOptions.Sale}
          <Sale
            on:save={handleSaveSaleHistory}
            {saleHistory}
            initialPrice={detailedProduct.price?.perUnit}
            productId={detailedProduct.id}
          />
        {/if}
      </div>
    </div>
  </div>
{/if}

<style>
  table {
    box-shadow: 0px 4px 18px 2.25px rgba(0, 0, 0, 0.18);
  }
  th {
    padding: 8px 12px;
  }
</style>
