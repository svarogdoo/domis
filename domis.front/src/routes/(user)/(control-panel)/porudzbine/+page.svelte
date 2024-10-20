<script lang="ts">
  import { onMount } from "svelte";
  import { userService } from "../../../../services/user-service";
  import OrderRow from "./OrderRow.svelte";
  import OrderRowMobile from "./OrderRowMobile.svelte";
  import { userStore } from "../../../../stores/user";

  let orders: Array<UserOrder> = [];

  $: if ($userStore.token) {
    setOrders();
  }

  async function setOrders() {
    orders = await userService.getUserOrders();
  }
</script>

<section class="w-full">
  {#if orders && orders.length > 0}
    <table class="hidden lg:table w-full table-hover">
      <thead class="w-full bg-black text-white">
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
      <thead class="w-full text-sm bg-black text-white"
        ><th>Porudžbine</th></thead
      >
      <tbody class="divide-y divide-gray-200">
        {#each orders as order (order.id)}
          <OrderRowMobile {order} />
        {/each}
      </tbody>
    </table>
  {:else}
    <p>Nemate nijednu porudžbinu.</p>
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
