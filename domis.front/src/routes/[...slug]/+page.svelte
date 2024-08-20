<script lang="ts">
  import ProductPage from "./ProductPage.svelte";
  import CategoryPage from "./CategoryPage.svelte";

  export let data;
</script>

{#if data?.type === "product"}
  <ProductPage />
{:else}
  <CategoryPage />
{/if}
