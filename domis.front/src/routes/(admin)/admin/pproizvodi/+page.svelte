<script lang="ts">
  import ProductRow from "../proizvodi/ProductRow.svelte";

  export let data;

  let products: Array<Product>;
  $: products = data.props.products;
</script>

<div class="w-full">
  <table class="w-full">
    <thead class=" bg-domis-dark text-white">
      <th class="max-w-12">SKU</th>
      <th>Naziv</th>
      <th>Popust</th>
      <th>Status</th>
      <th></th>
    </thead>
    <tbody class="divide-y divide-gray-200">
      {#each products as product}
        <ProductRow {product} />
      {/each}</tbody
    >
  </table>
</div>

<style>
  table {
    box-shadow: 0px 4px 18px 2.25px rgba(0, 0, 0, 0.18);
  }
  th {
    padding: 8px 12px;
  }
</style>
