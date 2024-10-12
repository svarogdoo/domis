<script>
  import CheckoutForm from "./CheckoutForm.svelte";
  import OrderDetails from "./OrderDetails.svelte";
</script>

<section class="flex w-full px-8 gap-x-12 lg:mt-6">
  <div class="flex w-2/3 mx-12">
    <CheckoutForm />
  </div>
  <div class="flex w-1/3">
    <OrderDetails />
  </div>
</section>
