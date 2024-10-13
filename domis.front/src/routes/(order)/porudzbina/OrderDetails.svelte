<script lang="ts">
  import { formatPrice } from "../../../helpers/numberFormatter";
  import { cart } from "../../../stores/cart";

  let cartItems: Array<CartProduct> = [];
  let totalCartPrice: number;

  const unsubscribe = cart.subscribe((value) => {
    if (value && value.items) cartItems = value.items;
    if (value && value.totalCartPrice) totalCartPrice = value.totalCartPrice;
  });
</script>

<div class="flex flex-col w-full px-8 py-6 gap-y-4 bg-gray-50 font-light">
  <h2 class="text-xl tracking-wide">Detalji porudžbine</h2>
  <div class="h-0.5 w-full bg-gray-400"></div>
  <div class="flex flex-col gap-y-4">
    <div class="flex justify-between font-normal">
      <p>Proizvod</p>
      <p>Ukupno</p>
    </div>
    {#each cartItems as item (item.cartItemId)}
      <div
        class="flex justify-between relative group"
        id="id-{item.cartItemId}"
      >
        <p class="w-52 truncate">
          {item.quantity} × {item.productDetails.name}
        </p>
        <p class="text-end">
          {formatPrice(item.cartItemPrice)}
          <span class="font-light text-md">RSD</span>
        </p>
        <span
          class="absolute left-1/2 transform -translate-x-1/2 bottom-full mb-1 bg-white border border-gray-300 rounded text-xs p-2 text-black opacity-0 transition-opacity duration-200 ease-in-out group-hover:opacity-100"
        >
          {item.quantity} × {item.productDetails.name}
        </span>
      </div>
    {/each}
  </div>
  <div class="h-0.5 w-full bg-gray-400"></div>
  <div class="flex justify-between">
    <p>Ukupno</p>
    <p>
      {formatPrice(totalCartPrice)} <span class="font-light text-md">RSD</span>
    </p>
  </div>
  <div class="h-0.5 w-full bg-gray-400"></div>
  <div class="flex justify-between text-lg">
    <p class="font-normal">Ukupno</p>
    <p>
      {formatPrice(totalCartPrice)} <span class="font-light text-md">RSD</span>
    </p>
  </div>
  <div class="h-0.5 w-full bg-gray-400"></div>
  <!-- TODO: Payment option choice -->
</div>
