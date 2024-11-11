<script lang="ts">
  import { mapQuantityTypeToCartString } from "../../../../enums";
  import { handleImageError } from "../../../../helpers/imageFallback";
  import { formatPrice } from "../../../../helpers/numberFormatter";
  import { getCurrencyString } from "../../../../helpers/stringFormatter";

  export let item: UserOrderItem;

  let quantity = item.quantity;
</script>

<tr class="h-full">
  <td class="w-36">
    <div class="w-full flex items-center p-2 gap-x-3">
      <img
        src={item.productDetails.image}
        alt="proizvod"
        on:error={handleImageError}
        class="h-24 w-24 object-cover"
      />
      <div class="flex flex-col gap-y-1">
        <p class="text-left text-sm text-gray-400 font-light">
          SKU: {item.productDetails.sku}
        </p>
        <p class="text-left text-sm text-wrap pr-4">
          {item.productDetails.name}
        </p>
        <div class="flex gap-x-4 items-center">
          <p>
            <span class="font-light text-sm">Cena: </span>
            {item.itemPrice}
            <span class="font-light text-sm">{getCurrencyString()}</span>
          </p>
          <div class="flex items-center gap-x-2">
            <p>
              {quantity}
              {mapQuantityTypeToCartString(
                item.productDetails.quantityType,
                quantity
              )}
            </p>
          </div>
        </div>
        <p class="mt-3">
          <span class="font-light text-sm">Ukupna cena: </span>
          {formatPrice(item.itemPriceTotal)}
          <span class="font-light text-sm">{getCurrencyString()}</span>
        </p>
      </div>
    </div>
  </td>
</tr>
