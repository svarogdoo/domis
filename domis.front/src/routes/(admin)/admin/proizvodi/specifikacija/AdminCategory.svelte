<script lang="ts">
  import dropdownIcon from "$lib/icons/dropdown.svg";
  export let category: Category;
  export let selectedCategoryId: string;
</script>

<div>
  <input type="checkbox" id={category.id} class="peer hidden" />
  <label for={category.id} class="w-full cursor-pointer">
    <img
      class="w-auto h-2 absolute top-1 right-0 transform transition-transform duration-200 ease-in-out"
      src={dropdownIcon}
      alt="icon"
    />
  </label>
  <ul class="hidden peer-checked:block py-2 ml-4" aria-labelledby={category.id}>
    {#if category.subcategories}
      {#each category?.subcategories as subcategory}
        <li class="relative text-wrap pr-4 transition ease-in-out">
          <button
            class="text-left"
            on:click={() => (selectedCategoryId = subcategory.id)}
            >{subcategory.name}</button
          >
          {#if subcategory.subcategories?.length && subcategory.subcategories.length > 0}
            <svelte:self category={subcategory} bind:selectedCategoryId />
          {/if}
        </li>
      {/each}
    {/if}
  </ul>
</div>

<style>
  input:checked + label img {
    @apply rotate-180;
  }
</style>
