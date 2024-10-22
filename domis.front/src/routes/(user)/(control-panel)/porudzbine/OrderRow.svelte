<script lang="ts">
  import {
    getOrderStatusColor,
    mapOrderStatusToString,
    mapPaymentVendorTypeToString,
  } from "../../../../enums";
  import { formatPrice } from "../../../../helpers/numberFormatter";
  import OrderItemPopup from "./OrderItemPopup.svelte";
  import eyeIcon from "$lib/icons/eye.svg";
  import { stringDateToString } from "../../../../helpers/stringFormatter";

  export let order: UserOrder;
  let showOrderItems = false;
</script>

<tr class="text-center">
  <td class="py-5">{stringDateToString(order.date)}</td>
  <td class="w-32"
    ><p class="py-2 rounded-full text-md {getOrderStatusColor(order.statusId)}">
      {mapOrderStatusToString(order.statusId)}
    </p>
  </td>
  <td class="py-5">{order.address}</td>
  <td class="py-5">{mapPaymentVendorTypeToString(order.paymentTypeId)}</td>
  <td class="py-5"
    >{formatPrice(order.paymentAmount)}
    <span class="font-light text-sm">RSD</span></td
  >
  <td class="flex justify-center py-5">
    <button on:click={() => { showOrderItems = true; }}>
      <img class="w-6 h-6" src={eyeIcon} alt="eye" />
    </button>
  </td>
  {#if showOrderItems}
    <OrderItemPopup bind:show={showOrderItems} orderItems={order.orderItems} />
  {/if}
</tr>