<script lang="ts">
  import ProductCard from "./CategoryProductCard.svelte";
  import { getCategoryProducts } from "../../../../services/category-service";
  import { onMount } from "svelte";

  export let data;

  let categoryDetails: CategoryDetails;
  let products: Array<CategoryProduct> = [];
  let isOpen = false;
  let sortType = "low-to-high";

  let pageNumber = 1;
  let pageSize = 18;

  $: products = data.props.products;
  $: categoryDetails = data.props.category;
  $: sortProducts(sortType);

  function toggleDropdown() {
    isOpen = !isOpen;
  }

  function sortProducts(sortType: string) {
    let sortedProducts = products.sort((a, b) => {
      if (sortType === "high-to-low") {
        return b.price - a.price;
      } else {
        return a.price - b.price;
      }
    });
    products = sortedProducts;
  }

  function setSortType(value: string) {
    sortType = value;
    isOpen = false;
  }
  
  async function fetchProducts() {
    const categoryData = await getCategoryProducts(Number.parseInt(categoryDetails.id), pageNumber, pageSize);
    products = categoryData.products;
  }

  function changePage(delta: number) {
    pageNumber += delta;

    const newUrl = new URL(window.location.href);
    newUrl.searchParams.set("strana", pageNumber.toString());
    history.pushState({}, '', newUrl);

    fetchProducts();
  }

  onMount(() => {
    const urlParams = new URLSearchParams(window.location.search);
    const pageParam = urlParams.get("strana");
    if (pageParam) {
      pageNumber = Number(pageParam);
    }
  });
</script>

<section class="w-full flex flex-col justify-center">
  <div class="flex justify-between items-center px-4 mb-4">
    <h2 class="text-lg lg:text-2xl">
      {categoryDetails?.name ? categoryDetails.name : ""}
    </h2>
    <div class="relative flex">
      <div class="text-sm lg:text-normal">
        <button on:click={toggleDropdown} class="flex gap-x-2 font-light items-center">
          Sortiraj prema:
          <p class="ring-1 rounded-lg ring-gray-500 px-2 lg:px-4 py-1 lg:py-2 font-light">
            {sortType === "low-to-high" ? "Cena rastuće" : "Cena opadajuće"}
          </p>
        </button>
      </div>
      {#if isOpen}
        <ul class="absolute z-10 right-0 mt-12 w-40 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5 focus:outline-none">
          <button on:click={() => setSortType("low-to-high")} class="w-full text-right py-3 px-3 hover:bg-gray-700 hover:text-white rounded-lg">
            Cena rastuće
          </button>
          <button on:click={() => setSortType("high-to-low")} class="w-full text-right py-3 px-3 hover:bg-gray-700 hover:text-white rounded-lg">
            Cena opadajuće
          </button>
        </ul>
      {/if}
    </div>
  </div>
  <div class="w-full grid grid-cols-2 lg:grid-cols-3 gap-x-2 gap-y-4">
    {#each products as product}
      <ProductCard {product} />
    {/each}
  </div>

  <!-- Pagination Controls -->
<div class="flex justify-end mt-4">
  <div class="flex space-x-2">
    <button 
      on:click={() => changePage(-1)} 
      disabled={pageNumber === 1} 
      class="disabled:opacity-50"
    >
      Prethodna
    </button>
    <button 
      on:click={() => changePage(1)} 
      disabled={products.length < pageSize}
      class="disabled:opacity-50"
    >
      Sledeća
    </button>
  </div>
</div>

</section>
