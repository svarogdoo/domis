<script lang="ts">
  import { onMount } from "svelte";
  import { userService } from "../../../../services/user-service";
  import OrderRow from "./OrderRow.svelte";

  let date = new Date();
  let orders: Array<UserOrder> = [
    {
      id: 1,
      date: date,
      status: 1,
      address: "Cvijiceva 53",
      paymentType: 1,
      paymentAmount: 2000,
      items: [
        {
          id: 1,
          quantity: 2,
          itemPrice: 2000,
          productDetails: {
            sku: 123,
            name: "Proizvod",
            image: "null",
            price: 1000,
            quantityType: 1,
          },
        },
      ],
      comment: "",
    },
    {
      id: 2,
      date: date,
      status: 4,
      address: "Cvijiceva 53",
      paymentType: 1,
      paymentAmount: 2000,
      items: [
        {
          id: 1,
          quantity: 2,
          itemPrice: 2000,
          productDetails: {
            sku: 123,
            name: "Proizvod",
            image: "null",
            price: 1000,
            quantityType: 1,
          },
        },
      ],
      comment: "",
    },
    {
      id: 3,
      date: date,
      status: 5,
      address: "Cvijiceva 53",
      paymentType: 1,
      paymentAmount: 2000,
      items: [
        {
          id: 1,
          quantity: 2,
          itemPrice: 2000,
          productDetails: {
            sku: 123,
            name: "Proizvod",
            image: "null",
            price: 1000,
            quantityType: 1,
          },
        },
      ],
      comment: "",
    },
  ];

  //   async function setOrders() {
  //     orders = await userService.getUserOrders();
  //   }

  //   onMount(() => {
  //     setOrders();
  //   });
</script>

<section class="w-full mt-12">
  {#if orders && orders.length > 0}
    <table class="w-full table-hover">
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
