<script lang="ts">
  import CheckoutForm from "./CheckoutForm.svelte";
  import OrderDetails from "./OrderDetails.svelte";

  let formComponent: any;
  let validate: () => CheckoutFormData | null;
</script>

<section class="flex w-full px-8 gap-x-12 lg:mt-6">
  <div class="flex w-2/3 mx-12">
    <CheckoutForm bind:validate />
  </div>
  <div class="flex w-1/3">
    <OrderDetails onClick={validate} />
  </div>
</section>
