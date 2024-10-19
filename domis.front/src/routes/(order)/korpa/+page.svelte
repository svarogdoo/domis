<script lang="ts">
  import { cart } from "../../../stores/cart";
  import CartItem from "./CartItem.svelte";
  import { formatPrice } from "../../../helpers/numberFormatter";
  import CartItemMobile from "./CartItemMobile.svelte";

  let cartProducts: Array<CartProduct> | undefined = [];
  let totalCartPrice: number | undefined;

  $: cartProducts = $cart?.items;
  $: totalCartPrice = $cart?.totalCartPrice;
</script>

<section class="w-full flex flex-col">
  <h1 class="ml-12 my-8 text-2xl lg:text-3xl font-light text-black">Korpa</h1>
  {#if cartProducts && cartProducts.length > 0 && totalCartPrice}
    <div
      class="w-full flex flex-col gap-y-8 lg:flex-row justify-center items-start gap-x-8 px-4"
    >
      <!-- Desktop -->
      <table class="hidden lg:table table-hover">
        <thead class="w-full bg-black text-white">
          <th class=" "></th>
          <th class="text-start">Opis</th>
          <th class="text-center">Cena</th>
          <th class="text-center">Količina</th>
          <th class="text-center">Ukupno</th>
          <th></th>
        </thead>
        <tbody class="divide-y divide-gray-200">
          {#each cartProducts as item (item.cartItemId)}
            <CartItem bind:item />
          {/each}
        </tbody>
      </table>
      <!-- Mobile -->
      <table class="table lg:hidden w-full">
        <thead class="w-full text-sm bg-black text-white"
          ><th>Proizvodi</th></thead
        >
        <tbody class="divide-y divide-gray-200">
          {#each cartProducts as item (item.cartItemId)}
            <CartItemMobile bind:item />
          {/each}
        </tbody>
      </table>
      <div
        class="order-card flex flex-col w-full lg:w-1/4 border-l-2 px-8 py-4 border rounded-xl max-h-96"
      >
        <h2 class="text-2xl font-bold tracking-widest mt-2 text-center">
          Porudžbina
        </h2>
        <div class="flex justify-between mt-16 font-light gap-x-8">
          <p>Ukupno</p>
          <p>{formatPrice(totalCartPrice)} RSD</p>
        </div>
        <div class="w-full h-0.5 bg-black my-4"></div>
        <div class="flex justify-between text-lg gap-x-8">
          <p>Ukupno</p>
          <p>{formatPrice(totalCartPrice)} RSD</p>
        </div>
        <a
          href="/porudzbina"
          class="text-light bg-black text-white mt-16 py-2 px-4 rounded-lg text-center tracking-widest"
        >
          PLATI
        </a>
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

  .order-card {
    box-shadow: 0px 4px 18px 2.25px rgba(0, 0, 0, 0.18);
  }

  button {
    letter-spacing: 0.24em;
  }
</style>
