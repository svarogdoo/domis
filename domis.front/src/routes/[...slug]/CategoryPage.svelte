<script lang="ts">
  import ProductCard from "./ProductCard.svelte";
</script>

<section>
  <div class="flex justify-between items-center mb-3">
    <h2 class="text-2xl">Keramika</h2>
    <div class="px-3 py-1 border-gray-500 border rounded-full">Sortiraj</div>
  </div>
  <div class="product-cards grid grid-cols-3 gap-x-12 gap-y-6">
    <ProductCard />
    <ProductCard />
    <ProductCard />
    <ProductCard />
    <ProductCard />
    <ProductCard />
    <ProductCard />
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
