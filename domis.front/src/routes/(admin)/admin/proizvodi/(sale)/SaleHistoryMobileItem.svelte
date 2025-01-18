<script lang="ts">
  import { formatDate } from "../../../../../helpers/dateFormatter";
  import { formatPrice } from "../../../../../helpers/numberFormatter";

  export let sale: SaleInfo;
</script>

<tr class="group hover:bg-domis-light cursor-pointer">
  <td class="w-32">
    <div class="w-full flex flex-col text-center gap-y-2">
      <p class={sale.isActive ? "font-semibold" : ""}>
        {formatPrice(sale.salePrice) ?? "0.00"} RSD
      </p>
      <p>
        Početak: {sale.startDate ? formatDate(new Date(sale.startDate)) : ""}
      </p>
      {#if sale.endDate}
        <p>
          Kraj: {formatDate(new Date(sale.endDate))}
        </p>
      {/if}
      <p
        class="py-2 rounded-full text-center text-md {sale.isActive
          ? 'bg-green-200'
          : 'bg-gray-200'}"
      >
        {sale.isActive ? "aktivan" : "neaktivan"}
      </p>
    </div>
  </td>
</tr>

<style>
  td {
    padding: 8px 12px;
  }
</style>
