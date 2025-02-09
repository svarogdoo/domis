<script lang="ts">
  import { formatDate } from "../../../../../helpers/dateFormatter";
  import { formatPrice } from "../../../../../helpers/numberFormatter";

  export let sale: SaleInfo;
</script>

<tr class="group bg-white hover:bg-domis-light cursor-pointer">
  <td class="w-32 {sale.isActive ? 'font-semibold' : ''}">
    {formatPrice(sale.salePrice) ?? "0.00"} RSD</td
  >
  <td class="w-32 text-center"
    >{sale.startDate ? formatDate(new Date(sale.startDate)) : ""}</td
  >
  <td class="w-32 text-center"
    >{sale.endDate ? formatDate(new Date(sale.endDate)) : ""}</td
  >
  <td class="w-32">
    <p
      class="py-2 rounded-full text-center text-md {sale.isActive
        ? 'bg-green-200'
        : 'bg-gray-200'}"
    >
      {sale.isActive ? "aktivan" : "neaktivan"}
    </p>
  </td>
</tr>

<style>
  td {
    padding: 8px 12px;
  }
</style>
