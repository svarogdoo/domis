<script lang="ts">
  import { mapQuantityTypeToCartString } from "../../enums";

  export let item: CartProduct;

  let quantity = item.quantity;

  function handleQuantityChange() {
    let num = Number(quantity);

    if (isNaN(num)) {
      quantity = item.quantity;
      return;
    }

    quantity = Math.round(num);
    item.quantity = quantity;
  }
</script>

<tr class="h-full">
  <td class="w-36">
    <img
      src={item.featuredImageUrl}
      alt="proizvod"
      class="p-3 h-32 w-32 object-cover"
    />
  </td>
  <td class="flex flex-col h-32 justify-center w-80">
    <p class="text-sm text-gray-400 font-light">
      SKU: {item.sku}
    </p>
    <p class="text-wrap pr-4">{item.name}</p>
  </td>
  <td class="w-44">{item.price}</td>
  <!-- TODO: Bind amount to be changeable -->
  <td class="w-36">
    <div class="flex items-center gap-x-2">
      <input
        class="px-2 py-1 w-16 rounded-lg border"
        bind:value={quantity}
        on:input={handleQuantityChange}
      />
      <p>
        {mapQuantityTypeToCartString(quantity)}
      </p>
    </div>
  </td>
  <td class="w-44">{item.totalPrice}</td>
  <!-- TODO: Add remove from list button -->
</tr>
