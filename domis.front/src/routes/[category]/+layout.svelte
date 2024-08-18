<div class="product-search">
  <div class="category gap-x-12 pt-4">
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
    width: 90%;
    display: flex;
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
</style>
