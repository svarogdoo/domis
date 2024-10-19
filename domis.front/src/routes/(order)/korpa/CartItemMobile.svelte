<script lang="ts">
  import { mapQuantityTypeToCartString } from "../../../enums";
  import { formatPrice } from "../../../helpers/numberFormatter";
  import { cart } from "../../../stores/cart";
  import { handleImageError } from "../../../helpers/imageFallback";

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

  function selectText(event: Event) {
    const input = event.target as HTMLInputElement;
    input.select();
  }
</script>

<tr class="h-full">
  <td class="w-36 relative">
    <div class="w-full flex items-center p-2 gap-x-3">
      <img
        src={item.productDetails.image}
        alt="proizvod"
        on:error={handleImageError}
        class="h-24 w-24 object-cover"
      />
      <div class="flex flex-col gap-y-1">
        <p class="text-sm text-gray-400 font-light">
          SKU: {item.productDetails.sku}
        </p>
        <p class="text-sm text-wrap pr-4">{item.productDetails.name}</p>
        <div class="flex gap-x-4 items-center">
          <p>
            <span class="font-light text-sm">Cena: </span>
            {item.productDetails.price}
            <span class="font-light text-sm">RSD</span>
          </p>
          <div class="flex items-center gap-x-2">
            <input
              class="text-sm px-2 w-10 rounded-lg border"
              bind:value={quantity}
              on:input={handleQuantityChange}
              on:click={selectText}
            />
            <p>
              {mapQuantityTypeToCartString(
                item.productDetails.quantityType,
                quantity
              )}
            </p>
          </div>
        </div>
        <p class="mt-3">
          <span class="font-light text-sm">Ukupna cena: </span>
          {formatPrice(item.cartItemPrice)}
          <span class="font-light text-sm">RSD</span>
        </p>
      </div>
    </div>
    <button
      on:click={removeItemFromCart}
      class="absolute top-2 right-2 text-red-600"
    >
      <svg
        xmlns="http://www.w3.org/2000/svg"
        fill="none"
        viewBox="0 0 24 24"
        stroke-width="1.5"
        stroke="currentColor"
        class="size-4"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          d="M6 18 18 6M6 6l12 12"
        />
      </svg>
    </button>
  </td>
</tr>
