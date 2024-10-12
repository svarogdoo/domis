<script lang="ts">
  import { mapQuantityTypeToCartString } from "../../enums";
  import bin from "$lib/icons/bin.svg";
  import { formatPrice } from "../../helpers/numberFormatter";
  import { cart } from "../../stores/cart";
  import { handleImageError } from "../../helpers/imageFallback";

  export let item: CartProduct;

  let quantity = item.quantity;

  function handleQuantityChange() {
    let num = Number(quantity);

    if (isNaN(num) || num === 0) {
      quantity = item.quantity;
      return;
    }

    quantity = Math.round(num);
    item.quantity = quantity;

    cart.update(item.cartItemId, item.quantity);
  }

  function removeItemFromCart() {
    cart.remove(item.cartItemId);
  }
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
  <td class="flex flex-col h-32 justify-center w-80">
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
      <input
        class="px-2 py-1 w-16 rounded-lg border"
        bind:value={quantity}
        on:input={handleQuantityChange}
      />
      <p>
        {mapQuantityTypeToCartString(
          item.productDetails.quantityType,
          quantity
        )}
      </p>
    </div>
  </td>
  <td class="w-44 text-center"
    >{formatPrice(item.cartItemPrice)}
    <span class="font-light text-sm">RSD</span></td
  >
  <td class="w-12">
    <button on:click={removeItemFromCart}>
      <img class="w-8 hover:scale-125 transition" src={bin} alt="bin" />
    </button>
  </td>
</tr>
