<script lang="ts">
  import ProductCard from "./CategoryProductCard.svelte";
  import { getCategoryProducts } from "../../../../services/category-service";
  import { onMount } from "svelte";
  import { page } from "$app/stores";
  import { mapSortTypeToString, SortType } from "../../../../enums";

  export let data;

  let categoryDetails: CategoryDetails;
  let products: Array<CategoryProduct> = [];
  let isOpen = false;
  let loadMoreTrigger: HTMLElement;
  let loading = false;

  let pageNumber = 2;
  let pageSize = 18;

  $: products = data.props.products;
  $: categoryDetails = data.props.category;
  $: sortType = data.props.sort;

  function toggleDropdown() {
    isOpen = !isOpen;
  }

  function setSortType(value: SortType) {
    pageNumber = 1;
    products = [];
    sortType = value;
    isOpen = false;
    loadMore();
  }

  async function loadMore() {
    loading = true;

    const minPrice = $page.url.searchParams.get("minPrice") || undefined;
    const maxPrice = $page.url.searchParams.get("maxPrice") || undefined;
    const minWidth = $page.url.searchParams.get("minWidth") || undefined;
    const maxWidth = $page.url.searchParams.get("maxWidth") || undefined;
    const minHeight = $page.url.searchParams.get("minHeight") || undefined;
    const maxHeight = $page.url.searchParams.get("maxHeight") || undefined;

    const newItems = await getCategoryProducts(
      Number.parseInt(categoryDetails?.id ? categoryDetails.id : "sale"),
      pageNumber,
      pageSize,
      sortType,
      minPrice,
      maxPrice,
      minWidth,
      maxWidth,
      minHeight,
      maxHeight
    );

    products = filterDuplicates(newItems);

    loading = false;
    pageNumber++;
  }

  function filterDuplicates(items: CategoryData) {
    const productMap = new Map(products.map((item) => [item.id, item]));
    for (const item of items.products) {
      if (!productMap.has(item.id)) {
        productMap.set(item.id, item);
      }
    }

    return Array.from(productMap.values());
  }

  onMount(() => {
    const urlParams = new URLSearchParams(window.location.search);
    const pageParam = urlParams.get("strana");
    sortType = SortType.NameAsc;
    if (pageParam) {
      pageNumber = Number(pageParam);
    }

    const observer = new IntersectionObserver(
      (entries) => {
        if (entries[0].isIntersecting && !loading) {
          loadMore();
        }
      },
      {
        rootMargin: "100px", // Start loading earlier, as it approaches
      }
    );
    observer.observe(loadMoreTrigger);
  });
</script>

<section class="w-full flex flex-col justify-center">
  <div class="flex justify-between items-center px-4 mb-4">
    <h2 class="text-lg lg:text-2xl">
      {categoryDetails?.name ? categoryDetails.name : "Akcija"}
    </h2>
    <div class="relative flex">
      <div class="text-sm lg:text-normal">
        <button
          on:click={toggleDropdown}
          class="flex gap-x-2 font-light items-center"
        >
          <span class="hidden lg:flex">Sortiraj prema:</span>
          <p
            class="ring-1 rounded-lg ring-gray-500 px-2 lg:px-4 py-1 lg:py-2 font-light"
          >
            {mapSortTypeToString(sortType)}
          </p>
        </button>
      </div>
      {#if isOpen}
        <ul
          class="absolute z-10 right-0 mt-12 w-40 rounded-md shadow-lg bg-white ring-1 ring-domis-dark ring-opacity-5 focus:outline-none"
        >
          <button
            on:click={() => setSortType(SortType.PriceAsc)}
            class="w-full text-right py-3 px-3 hover:bg-gray-700 hover:text-white rounded-lg"
          >
            Cena rastuće
          </button>
          <button
            on:click={() => setSortType(SortType.PriceDesc)}
            class="w-full text-right py-3 px-3 hover:bg-gray-700 hover:text-white rounded-lg"
          >
            Cena opadajuće
          </button>
          <button
            on:click={() => setSortType(SortType.NameAsc)}
            class="w-full text-right py-3 px-3 hover:bg-gray-700 hover:text-white rounded-lg"
          >
            Naziv rastuće
          </button>
          <button
            on:click={() => setSortType(SortType.NameDesc)}
            class="w-full text-right py-3 px-3 hover:bg-gray-700 hover:text-white rounded-lg"
          >
            Naziv opadajuće
          </button>
        </ul>
      {/if}
    </div>
  </div>
  <div class="w-full grid grid-cols-2 lg:grid-cols-3 gap-x-2 gap-y-4">
    {#each products as product (product.id)}
      <ProductCard {product} />
    {/each}
  </div>

  <!-- Trigger for loading more data -->
  <div bind:this={loadMoreTrigger} style="height: 1px;"></div>

  <!-- Loading indicator -->
  {#if loading}
    <div class="flex self-center items-center mt-4 space-x-1">
      <div
        class="w-1.5 h-1.5 bg-domis-accent rounded-full animate-bounce"
      ></div>
      <div
        class="w-1.5 h-1.5 bg-domis-accent rounded-full animate-bounce animation-delay-200"
      ></div>
      <div
        class="w-1.5 h-1.5 bg-domis-accent rounded-full animate-bounce animation-delay-400"
      ></div>
    </div>
  {/if}
</section>
