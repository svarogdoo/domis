<script lang="ts">
  import dropdownIcon from "$lib/icons/dropdown.svg";
  export let category: Category;
</script>

<div>
  <input type="checkbox" id={category.categoryId} class="peer hidden" />
  <label for={category.categoryId} class="w-full flex justify-between">
    <img
      class="w-auto h-2 absolute top-1 right-0"
      src={dropdownIcon}
      alt="icon"
    />
  </label>
  <ul
    class="hidden peer-checked:block py-2 ml-4"
    aria-labelledby="dropdownButton"
  >
    {#if category.subcategories}
      {#each category?.subcategories as subcategory}
        <li
          class="relative text-wrap pr-4 hover:scale-105 transition ease-in-out"
        >
          <a href={subcategory.categoryId}>{subcategory.categoryName}</a>
          {#if subcategory.subcategories?.length && subcategory.subcategories.length > 0}
            <svelte:self category={subcategory} />
          {/if}
        </li>
      {/each}
    {/if}
  </ul>
</div>
