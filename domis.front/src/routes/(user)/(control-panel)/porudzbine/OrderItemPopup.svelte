<script lang="ts">
  import OrderItemRow from "./OrderItemRow.svelte";
  import OrderItemRowMobile from "./OrderItemRowMobile.svelte";

  export let show = false;
  export let orderItems: Array<UserOrderItem>;

  const closePopup = () => {
    show = false;
  };
</script>

<div
  class="fixed inset-0 flex items-center justify-center bg-domis-dark bg-opacity-50 z-50"
>
  <div
    id="order-item-popup"
    class="w-full lg:w-auto flex flex-col gap-y-6 items-center bg-white p-6 rounded-lg shadow-lg text-center"
  >
    <table class="hidden lg:table">
      <thead class="w-full bg-domis-dark text-white">
        <th class=" "></th>
        <th class="text-start">Opis</th>
        <th class="text-center">Cena</th>
        <th class="text-center">Količina</th>
        <th class="text-center">Ukupno</th>
      </thead>
      <tbody class="divide-y divide-gray-200">
        {#each orderItems as item (item.id)}
          <OrderItemRow {item} />
        {/each}
      </tbody>
    </table>
    <!-- Mobile -->
    <table class="table lg:hidden w-full">
      <thead class="w-full text-sm bg-domis-dark text-white"
        ><th>Proizvodi</th></thead
      >
      <tbody class="divide-y divide-gray-200">
        {#each orderItems as item (item.id)}
          <OrderItemRowMobile {item} />
        {/each}
      </tbody>
    </table>

    <button
      class="text-light bg-domis-dark text-white py-2 px-12 rounded-lg text-center tracking-widest hover:bg-gray-600 disabled:bg-gray-400"
      on:click={closePopup}
      >Zatvori
    </button>
  </div>
</div>

<style>
  table {
    box-shadow: 0px 4px 18px 2.25px rgba(0, 0, 0, 0.18);
  }
  th {
    padding: 8px 0px;
  }
</style>
