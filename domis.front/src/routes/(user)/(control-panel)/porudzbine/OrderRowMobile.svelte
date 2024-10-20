<script lang="ts">
  import {
    getOrderStatusColor,
    mapOrderStatusToString,
    mapPaymentVendorTypeToString,
  } from "../../../../enums";
  import { formatPrice } from "../../../../helpers/numberFormatter";
  import OrderItemPopup from "./OrderItemPopup.svelte";
  import eyeIcon from "$lib/icons/eye.svg";
  import { stringDateToString as formatDateStringToString } from "../../../../helpers/stringFormatter";

  export let order: UserOrder;
  let showOrderItems = false;
</script>

<tr>
  <td class="py-5">
    <div class="flex flex-col items-center gap-y-3 px-2">
      <div class="flex items-center justify-center gap-x-2">
        <p>
          {formatDateStringToString(order.date)}
        </p>
        <p
          class="px-3 py-1 rounded-full text-md {getOrderStatusColor(
            order.statusId
          )}"
        >
          {mapOrderStatusToString(order.statusId)}
        </p>
      </div>

      <p>{mapPaymentVendorTypeToString(order.paymentTypeId)}</p>
      <p>{order.address}</p>
      <p>
        {formatPrice(order.paymentAmount)}
        <span class="font-light text-sm">RSD</span>
      </p>
      <div class="flex items-center justify-center gap-x-2">
        <p>Artikli:</p>
        <button on:click={() => (showOrderItems = true)}>
          <img class="w-6 h-6" src={eyeIcon} alt="eye" />
        </button>
      </div>
    </div></td
  >
  {#if showOrderItems}
    <OrderItemPopup bind:show={showOrderItems} orderItems={order.items} />
  {/if}
</tr>
