<script lang="ts">
  import { categories } from "../../../../../stores/categories";

  export let selectedCategoryId: string = "";

  interface FlattenedCategory {
    id: string;
    name: string;
    level: number;
  }

  let categoriesList: Array<FlattenedCategory>;
  $: categories.subscribe((value) => {
    categoriesList = flattenCategories(value);
  });

  function flattenCategories(
    categories: Array<Category>,
    level: number = 0
  ): Array<FlattenedCategory> {
    return categories.flatMap((category) => {
      const { id, name, subcategories } = category;

      // Add the current category with the given level
      const current = { id, name, level };

      // Recursively flatten subcategories, if any
      const subcategoriesFlattened = subcategories
        ? flattenCategories(subcategories, level + 1)
        : [];

      // Combine the current category with its flattened subcategories
      return [current, ...subcategoriesFlattened];
    });
  }
</script>

<div class="w-full flex flex-col gap-y-2">
  {#if categoriesList?.length && categoriesList.length > 0}
    <h2 class="font-bold">Kategorije</h2>
    <select
      bind:value={selectedCategoryId}
      class="block w-full border rounded-lg px-2 py-1 bg-white font-light"
    >
      <option disabled value="">Izaberite kategoriju</option>
      {#each categoriesList as category}
        <option value={category.id}>
          <p>
            {category.name}
          </p>
        </option>
      {/each}
    </select>
  {/if}
</div>

<!-- TODO: Loader -->
<!-- {#if categoriesList?.length && categoriesList.length > 0}
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
{/if} -->
