<script lang="ts">
  import OrderRow from "./OrderRow.svelte";
  import OrderRowMobile from "./OrderRowMobile.svelte";

  export let data;
  let orders = data.props.orders;
</script>

<section class="w-full flex flex-col gap-y-8">
  <h1 class="text-2xl">Porudžbine</h1>
  {#if orders && orders.length > 0}
    <table class="hidden lg:table w-full table-hover">
      <thead class="w-full bg-domis-dark text-white">
        <th>Datum</th>
        <th>Status</th>
        <th>Adresa isporuke</th>
        <th>Tip plaćanja</th>
        <th>Ukupna cena</th>
        <th>Artikli</th>
      </thead>
      <tbody class="divide-y divide-gray-200">
        {#each orders as order (order.id)}
          <OrderRow {order} />
        {/each}
      </tbody>
    </table>
    <table class="table lg:hidden w-full">
      <thead class="w-full text-sm bg-domis-dark text-white"
        ><th>Porudžbine</th></thead
      >
      <tbody class="divide-y divide-gray-200">
        {#each orders as order (order.id)}
          <OrderRowMobile {order} />
        {/each}
      </tbody>
    </table>
  {:else}
    <div>Trenutno nemate porudžbina</div>
  {/if}
</section>

<style>
  table {
    box-shadow: 0px 4px 18px 2.25px rgba(0, 0, 0, 0.18);
  }
  th {
    padding: 8px 0px;
  }
</style>
