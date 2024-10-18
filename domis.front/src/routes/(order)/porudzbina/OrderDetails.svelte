<script lang="ts">
  import RadioButton from "../../../components/RadioButton.svelte";
  import { paymentVendorOptions, type PaymentVendorType } from "../../../enums";
  import { formatPrice } from "../../../helpers/numberFormatter";
  import {
    saveOrder,
    saveShippingDetails,
  } from "../../../services/order-service";
  import { cart } from "../../../stores/cart";

  export let onClick: () => CheckoutFormData | null;

  let cartId: number;
  let cartItems: Array<CartProduct> = [];
  let totalCartPrice: number;
  let isTermsAccepted = false;
  let paymentVendor: PaymentVendorType;

  function handleExtraClicked() {
    isTermsAccepted = !isTermsAccepted;
  }

  async function handleSubmit() {
    let checkoutFormData = onClick();
    if (checkoutFormData) {
      const shippingResponse = await saveShippingDetails(
        checkoutFormData.shippingDetails
      );
      if (shippingResponse) {
        const order: Order = {
          cartId: cartId,
          paymentStatusId: 1,
          orderShippingId: shippingResponse.orderShippingId,
          paymentVendorTypeId: paymentVendor,
          comment: checkoutFormData.comment,
        };

        const orderResponse = await saveOrder(order);
        cart.initialize(); // todo: check if cart refreshes correctly

        if (orderResponse) {
          //todo: show some success popup
        }
      }
    }
  }

  const unsubscribe = cart.subscribe((value) => {
    if (value && value.cartId) cartId = value.cartId;
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

  <RadioButton
    options={paymentVendorOptions}
    legend="Odaberite način plaćanja:"
    isColumn={true}
    bind:userSelected={paymentVendor}
  />

  <p>
    Vaši lični podaci biće korišćeni u cilju poboljšanja vašeg korisničkog
    iskustva na ovoj web stranici, za upravljanje pristupom vašem računu i za
    druge svrhe opisane u našim <a
      href="/politika-privatnosti"
      class="underline cursor-pointer">politika privatnosti</a
    >.
  </p>

  <button
    on:click={handleExtraClicked}
    class="relative flex mt-4 gap-x-2 items-center"
  >
    <input
      type="checkbox"
      class="appearance-none h-5 w-5 cursor-pointer rounded-md border border-gray-700 checked:bg-gray-700"
      checked={isTermsAccepted}
    />
    <span class="absolute text-white left-0.5">
      <svg
        xmlns="http://www.w3.org/2000/svg"
        class="h-3.5 w-3.5"
        viewBox="0 0 20 20"
        fill="currentColor"
        stroke="currentColor"
        stroke-width="1"
      >
        <path
          fill-rule="evenodd"
          d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
          clip-rule="evenodd"
        ></path>
      </svg>
    </span>
    <p class="pt-1 text-sm font-extralight tracking-wider">
      Pročitao/la sam i prihvatam uslove veb mesta <span class="text-red-500"
        >*</span
      >
    </p>
  </button>

  <button
    on:click={handleSubmit}
    class="text-light bg-black text-white py-2 px-4 rounded-lg text-center tracking-widest hover:bg-gray-600 disabled:bg-gray-400"
    disabled={!isTermsAccepted}
  >
    NARUČI
  </button>
</div>
