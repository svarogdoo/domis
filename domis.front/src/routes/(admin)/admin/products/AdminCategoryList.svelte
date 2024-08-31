<script lang="ts">
  import { categories } from "../../../../stores/categories";
  import AdminCategory from "./AdminCategory.svelte";

  export let selectedCategoryId: string;

  let categoriesList: Array<Category>;
  $: categories.subscribe((value) => {
    categoriesList = value;
  });
</script>

<aside class="w-full flex flex-col gap-y-4">
  <!-- TODO: Loader -->
  {#if categoriesList?.length && categoriesList.length > 0}
    <h2 class="text-xl">Svi proizvodi</h2>
    <ul class="flex flex-col px-4 py-2 rounded-md">
      {#each categoriesList as category}
        <li
          class="w-full flex flex-col relative my-1 tracking-wider font-light text-wrap pr-4"
        >
          <button
            on:click={() => (selectedCategoryId = category.id)}
            class="transition ease-in-out text-left">{category.name}</button
          >
          {#if category.subcategories?.length && category.subcategories.length > 0}
            <AdminCategory {category} bind:selectedCategoryId />
          {/if}
        </li>
      {/each}
    </ul>
  {/if}
</aside>
