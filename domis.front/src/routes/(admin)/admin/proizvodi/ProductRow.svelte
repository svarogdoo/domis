<script lang="ts">
  import { createEventDispatcher } from "svelte";

  export let product: Product;

  const dispatch = createEventDispatcher<{ click: string }>();
</script>

<tr
  class="w-full hover:bg-slate-100 cursor-pointer"
  on:click={() => dispatch("click", product)}
>
  <td class="max-w-24 text-center px-2 py-1">{product.sku}</td>
  <td>
    <p class="px-2 py-1">
      {product.name}
    </p>
  </td>

  <td class="text-center px-2 py-1">
    <p
      class="text-light text-sm px-3 py-2 rounded-full {product.isActive
        ? 'bg-green-200 text-black'
        : 'bg-domis-dark text-white'}"
    >
      {product.isActive ? "aktivan" : "neaktivan"}
    </p>
  </td>
</tr>
