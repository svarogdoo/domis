<script lang="ts">
  import { onMount } from "svelte";
  import { page } from "$app/stores";
  import ProductCard from "./ProductCard.svelte";
  import { getCategoryProducts } from "../../services/category-service";

  let products;
  // console.info($page.params.slug);

  // onMount(() => {
  //   const slugParts = $page.params?.slug?.split("/");
  //   if (!slugParts) {
  //     return;
  //   }
  //   const lastSlug = slugParts[slugParts.length - 1];
  //   products = await getCategoryProducts(Number.parseInt(lastSlug));
  // });
</script>

<section>
  <div class="flex justify-between items-center mb-3">
    <h2 class="text-2xl">Keramika</h2>
    <div class="px-3 py-1 border-gray-500 border rounded-full">Sortiraj</div>
  </div>
  <div
    class="product-cards grid grid-cols-1 lg:grid-cols-3 gap-x-6 xl:gap-x-12 gap-y-6"
  >
    <!-- {#each products as product}
      <ProductCard {product} />
    {/each} -->
  </div>
</section>

<style>
  section {
    width: 100%;
    display: flex;
    flex-direction: column;
    justify-content: center;
  }

  .product-cards {
    width: 100%;
  }
</style>
