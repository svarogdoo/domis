<script>
  import { onMount } from "svelte";
  import Sidebar from "./Sidebar.svelte";
  import { setCategories } from "../../services/category-service";

  onMount(() => {
    setCategories();
  });
</script>

<div class="product-search">
  <div class="category gap-x-12 pt-4">
    <div class="product-sidebar hidden lg:flex">
      <Sidebar />
    </div>
    <div class="products lg:pt-2">
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
    width: 20%;
  }

  .products {
    width: 78%;
  }

  @media (max-width: 1024px) {
    .products {
      width: 100%;
    }
  }
</style>
