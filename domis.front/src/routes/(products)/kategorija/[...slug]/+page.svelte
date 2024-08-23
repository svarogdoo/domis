<script lang="ts">
  import { onMount } from "svelte";
  import { page } from "$app/stores";
  import ProductCard from "./CategoryProductCard.svelte";
  import { getCategoryProducts } from "../../../../services/category-service";
  import { getLastSlug } from "../../../../helpers/slugParsing";

  let products: Array<CategoryProduct> = [];
  let slug;
  let isOpen = false;
  let sortType = "low-to-high";

  $: slug = $page.params.slug;
  $: if (slug) {
    setCategoryProducts(slug);
  }
  $: sortProducts(sortType);

  async function setCategoryProducts(slug: string) {
    let lastSlug = getLastSlug(slug);
    if (lastSlug) {
      products = await getCategoryProducts(Number.parseInt(lastSlug));
    }
  }

  onMount(async () => {
    setCategoryProducts(slug);
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
  <div class="flex justify-between items-center mb-5">
    <!-- TODO: use actual category name -->
    <h2 class="text-2xl">Keramika</h2>
    <div class="relative flex">
      <div>
        <button
          on:click={toggleDropdown}
          class="flex gap-x-2 font-light items-center"
          >Sortiraj prema:

          <p class="ring-1 rounded-lg ring-gray-500 px-4 py-2 font-light">
            {sortType === "low-to-high" ? "Cena rastuće" : "Cena opadajuće"}
          </p></button
        >
      </div>
      {#if isOpen}
        <ul
          class="absolute right-0 mt-12 w-40 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5 focus:outline-none"
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
  <div class="w-full grid grid-cols-2 lg:grid-cols-3 gap-x-6 gap-y-6">
    {#each products as product}
      <ProductCard {product} />
    {/each}
  </div>
</section>
