<script lang="ts">
  import { getParentCategories } from "../../../../stores/categories";

  export let category: FlattenedCategory;
</script>

<tr class="hover:bg-slate-100 group">
  <td class="pl-1">
    {#if category.level > 0}
      {#each Array(category.level) as _, i}
        <span class="text-white group-hover:text-slate-100">www</span>
      {/each}
    {/if}
    {category.name}</td
  >
</tr>
