<script lang="ts">
  import ProductCard from "./ProductCard.svelte";
</script>

<section>
  <div class="category gap-x-4">
    <div class="product-sidebar gap-y-4">
      <h2 class="text-xl">Svi proizvodi</h2>
      <div class="sidebar-items rounded-md">
        <p>Nedeljna akcija</p>
        <p>Podovi</p>
        <p>Dekorativne letvice</p>
        <p>Nedeljna akcija</p>
        <p>Podovi</p>
        <p>Dekorativne letvice</p>
      </div>
    </div>
    <div class="products">
      <h2 class="text-xl">Keramika</h2>
      <div class="product-cards grid grid-cols-3 gap-x-12 gap-y-6">
        <ProductCard />
        <ProductCard />
        <ProductCard />
        <ProductCard />
        <ProductCard />
        <ProductCard />
        <ProductCard />
      </div>
    </div>
  </div>
</section>

<style>
  section {
    width: 100%;
    display: flex;
    justify-content: center;
  }

  .category {
    width: 90%;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
  }

  .product-sidebar {
    width: 15%;
    display: flex;
    flex-direction: column;
  }
  .sidebar-items {
    display: flex;
    flex-direction: column;
    border-width: 1px;
    border-color: #808080;
    padding: 12px 26px;
  }

  .products {
    width: 78%;
  }

  .product-cards {
    width: 100%;
  }
</style>
