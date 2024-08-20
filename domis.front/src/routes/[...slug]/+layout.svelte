<script>
  import Sidebar from "./Sidebar.svelte";
</script>

<div class="product-search">
  <div class="category gap-x-12 pt-4">
    <div class="product-sidebar">
      <Sidebar />
    </div>
    <div class="products mt-6">
      <slot />
    </div>
  </div>
</div>

<style>
  .product-search {
    width: 100%;
    display: flex;
    justify-content: center;
  }

  .category {
    width: 95%;
    display: flex;
  }

  .product-sidebar {
    width: 17%;
  }

  .products {
    width: 78%;
  }
</style>
