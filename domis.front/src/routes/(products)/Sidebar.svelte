<script lang="ts">
  import { categories } from "../../stores/categories";
  import SidebarCategory from "./SidebarCategory.svelte";

  export let hamburgerStyle = false;

  let categoriesList: Array<Category>;
  $: categories.subscribe((value) => {
    categoriesList = value;
  });
</script>

<aside class="w-full h-full flex flex-col gap-y-4">
  <!-- TODO: Loader -->
  {#if categoriesList?.length && categoriesList.length > 0}
    {#if !hamburgerStyle}
      <h2 class="text-xl">Svi proizvodi</h2>
    {/if}
    <ul
      class="flex flex-col px-4 py-2 {hamburgerStyle
        ? ''
        : 'border'} border-gray-400 rounded-md"
    >
      {#each categoriesList as category}
        <li
          class="w-full flex flex-col relative border-b border-gray-400 my-1 py-1 tracking-wider font-light text-wrap pr-4"
        >
          <a
            href="/kategorija/{category.id}"
            class="hover:scale-105 transition ease-in-out">{category.name}</a
          >
          {#if category.subcategories?.length && category.subcategories.length > 0}
            <SidebarCategory {category} />
          {/if}
        </li>
      {/each}
    </ul>
  {/if}
</aside>
