<script lang="ts">
  import CheckoutForm from "./CheckoutForm.svelte";
  import OrderDetails from "./OrderDetails.svelte";

  let validate: () => CheckoutFormData | null;
</script>

<section class="flex flex-col lg:flex-row w-full gap-x-4 lg:mt-6 items-start">
  <div class="flex w-full lg:w-2/3 px-4 lg:px-8">
    <CheckoutForm bind:validate />
  </div>
  <div class="flex w-full lg:w-1/3 lg:px-8">
    <OrderDetails onClick={validate} />
  </div>
</section>
