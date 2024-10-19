<script lang="ts">
  import {
    getOrderStatusColor,
    mapOrderStatusToString,
    mapPaymentVendorTypeToString,
  } from "../../../../enums";
  import { formatPrice } from "../../../../helpers/numberFormatter";
  import { dateToString } from "../../../../helpers/stringFormatter";
  import OrderItemPopup from "./OrderItemPopup.svelte";

  export let order: UserOrder;
  let showOrderItems = false;
</script>

<tr class="text-center">
  <td class="py-5">{dateToString(order.date)}</td>
  <td class="w-32"
    ><p class="py-2 rounded-full text-md {getOrderStatusColor(order.status)}">
      {mapOrderStatusToString(order.status)}
    </p>
  </td>
  <td class="py-5">{order.address}</td>
  <td class="py-5">{mapPaymentVendorTypeToString(order.paymentType)}</td>
  <td class="py-5"
    >{formatPrice(order.paymentAmount)}
    <span class="font-light text-sm">RSD</span></td
  >
  <td class="flex justify-center py-5">
    <button on:click={() => (showOrderItems = true)}>
      <svg
        xmlns="http://www.w3.org/2000/svg"
        fill="none"
        viewBox="0 0 24 24"
        stroke-width="2"
        stroke="currentColor"
        class="size-6"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          d="M2.036 12.322a1.012 1.012 0 0 1 0-.639C3.423 7.51 7.36 4.5 12 4.5c4.638 0 8.573 3.007 9.963 7.178.07.207.07.431 0 .639C20.577 16.49 16.64 19.5 12 19.5c-4.638 0-8.573-3.007-9.963-7.178Z"
        />
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          d="M15 12a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z"
        />
      </svg>
    </button>
  </td>
  {#if showOrderItems}
    <OrderItemPopup bind:show={showOrderItems} orderItems={order.items} />
  {/if}
</tr>
