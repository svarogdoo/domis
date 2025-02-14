<script lang="ts">
  import {
    getOrderStatusColor,
    mapOrderStatusToString,
    mapPaymentVendorTypeToString,
  } from "../../../../enums";
  import { formatPrice } from "../../../../helpers/numberFormatter";
  import {
    getCurrencyString,
    stringDateToString,
  } from "../../../../helpers/stringFormatter";

  export let order: AdminOrder;
</script>

<tr class="text-center">
  <td class="py-5">{stringDateToString(order.orderCreatedAt.toString())}</td>
  <td class="w-32"
    ><p
      class="py-2 rounded-full text-md {getOrderStatusColor(
        order.orderStatusId
      )}"
    >
      {mapOrderStatusToString(order.orderStatusId)}
    </p>
  </td>
  <td class="py-5">{order.deliveryOrderShipping?.shippingAddress}</td>
  <td class="py-5">{mapPaymentVendorTypeToString(order.paymentType)}</td>
  <td class="py-5"
    >{formatPrice(order.amount)}
    <span class="font-light text-sm">{getCurrencyString()}</span></td
  >
</tr>
