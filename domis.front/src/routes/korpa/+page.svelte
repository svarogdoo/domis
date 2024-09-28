<script lang="ts">
  import { onDestroy } from "svelte";
  import { cart } from "../../stores/cart";
  import CartItem from "./CartItem.svelte";

  let cartProducts: Array<CartProduct> = [];

  const unsubscribe = cart.subscribe((value) => {
    cartProducts = value;
  });

  const finalCart = {
    subtotal: 1150,
    total: 1150,
  };

  onDestroy(() => unsubscribe());
</script>

<section class="w-full flex flex-col">
  <h1 class="ml-12 my-8 text-3xl font-light text-black">Korpa</h1>
  {#if cartProducts.length > 0}
    <div class="w-full flex justify-center items-start gap-x-8 mx-4">
      <table class="table table-hover">
        <thead class="w-full bg-black text-white">
          <th></th>
          <th class="text-start">Opis</th>
          <th class="text-start">Cena</th>
          <th class="text-start">Količina</th>
          <th class="text-start">Ukupno</th>
        </thead>
        <tbody class="divide-y divide-gray-200">
          {#each cartProducts as item}
            <CartItem bind:item />
          {/each}
        </tbody>
      </table>
      <div
        class="order-card flex flex-col border-l-2 px-8 py-4 border rounded-xl max-h-96"
      >
        <h2 class="text-2xl font-bold tracking-widest mt-2 text-center">
          Porudžbina
        </h2>
        <div class="flex justify-between mt-16 font-light gap-x-8">
          <p>Ukupno</p>
          <p>{finalCart.subtotal}RSD</p>
        </div>
        <div class="w-full h-0.5 bg-black my-4"></div>
        <div class="flex justify-between text-lg gap-x-8">
          <p>Ukupno</p>
          <p>{finalCart.total}RSD</p>
        </div>
        <button
          class="text-light bg-black text-white mt-16 py-2 px-4 rounded-lg"
          >PLATI</button
        >
      </div>
    </div>
  {:else}
    <p class="ml-12">Trenutno nemate proizvode u korpi</p>
  {/if}
</section>

<style>
  table {
    box-shadow: 0px 4px 18px 2.25px rgba(0, 0, 0, 0.18);
  }
  th {
    padding: 8px 0px;
  }
  td {
    padding: 6px 0px;
  }

  .order-card {
    box-shadow: 0px 4px 18px 2.25px rgba(0, 0, 0, 0.18);
  }

  button {
    letter-spacing: 0.24em;
  }
</style>
