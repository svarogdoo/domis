<script lang="ts">
  import { mapQuantityTypeToCartString } from "../../../../enums";
  import { handleImageError } from "../../../../helpers/imageFallback";
  import { formatPrice } from "../../../../helpers/numberFormatter";

  export let item: UserOrderItem;
</script>

<tr class="h-full">
  <td class="w-36">
    <img
      src={item.productDetails.image}
      alt="proizvod"
      on:error={handleImageError}
      class="p-3 h-32 w-32 object-cover"
    />
  </td>
  <td class="flex flex-col h-32 text-start justify-center w-80">
    <p class="text-sm text-gray-400 font-light">
      SKU: {item.productDetails.sku}
    </p>
    <p class="text-wrap pr-4">{item.productDetails.name}</p>
  </td>
  <td class="w-44 text-center"
    >{item.productDetails.price} <span class="font-light text-sm">RSD</span></td
  >
  <td class="w-36">
    <div class="flex justify-center items-center gap-x-2">
      <p>
        {item.quantity}
        {mapQuantityTypeToCartString(
          item.productDetails.quantityType,
          item.quantity
        )}
      </p>
    </div>
  </td>
  <td class="w-44 text-center"
    >{formatPrice(item.itemPrice)}
    <span class="font-light text-sm">RSD</span></td
  >
</tr>
