<script lang="ts">
  import { onMount } from "svelte";
  import { page } from "$app/stores";
  import ProductCard from "./CategoryProductCard.svelte";
  import { getCategoryProducts } from "../../../../services/category-service";
  import { getLastSlug } from "../../../../helpers/slugParsing";

  let categoryDetails: CategoryDetails;
  let products: Array<CategoryProduct> = [];
  let slug;
  let isOpen = false;
  let sortType = "low-to-high";

  $: slug = $page.params.slug;
  $: if (slug) {
    setCategoryData(slug);
  }
  $: sortProducts(sortType);

  async function setCategoryData(slug: string) {
    let lastSlug = getLastSlug(slug);
    if (lastSlug) {
      let categoryData = await getCategoryProducts(Number.parseInt(lastSlug));
      products = categoryData.products;
      categoryDetails = categoryData.category;
    }
  }

  onMount(async () => {
    setCategoryData(slug);
  });

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
</script>

<section class="w-full flex flex-col justify-center">
  <div class="flex justify-between items-center px-4 mb-4">
    <h2 class="text-lg lg:text-2xl">
      {categoryDetails?.name ? categoryDetails.name : ""}
    </h2>
    <div class="relative flex">
      <div class="text-sm lg:text-normal">
        <button
          on:click={toggleDropdown}
          class="flex gap-x-2 font-light items-center"
          >Sortiraj prema:

          <p
            class="ring-1 rounded-lg ring-gray-500 px-2 lg:px-4 py-1 lg:py-2 font-light"
          >
            {sortType === "low-to-high" ? "Cena rastuće" : "Cena opadajuće"}
          </p></button
        >
      </div>
      {#if isOpen}
        <ul
          class="absolute z-10 right-0 mt-12 w-40 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5 focus:outline-none"
        >
          <button
            on:click={() => setSortType("low-to-high")}
            class="w-full text-right py-3 px-3 hover:bg-gray-700 hover:text-white rounded-lg"
          >
            Cena rastuće
          </button>
          <button
            on:click={() => setSortType("high-to-low")}
            class="w-full text-right py-3 px-3 hover:bg-gray-700 hover:text-white rounded-lg"
          >
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
</section>
