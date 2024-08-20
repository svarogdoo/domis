<script lang="ts">
  import SidebarCategory from "./SidebarCategory.svelte";
  import { categories } from "../../stores/categories";

  let categoriesList: Array<Category>;
  $: categories.subscribe((value) => {
    categoriesList = value;
  });
</script>

<aside class="w-full flex flex-col gap-y-4">
  <h2 class="text-xl">Svi proizvodi</h2>
  <ul class="flex flex-col border px-4 py-2 border-gray-400 rounded-md">
    {#each categoriesList as category}
      <li
        class="w-full flex flex-col relative border-b border-gray-400 my-1 tracking-wider font-light text-wrap pr-4"
      >
        <a
          href={category.categoryId}
          class="hover:scale-105 transition ease-in-out"
          >{category.categoryName}</a
        >
        {#if category.subcategories?.length && category.subcategories.length > 0}
          <SidebarCategory {category} />
        {/if}
      </li>
    {/each}
  </ul>
</aside>
