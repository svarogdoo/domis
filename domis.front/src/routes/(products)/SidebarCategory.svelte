<script lang="ts">
  import dropdownIcon from "$lib/icons/dropdown.svg";
  export let category: Category;
  export let isHamburger: boolean = false;

  let random = Math.random();

  function getUniqueId() {
    return `${category.id}${isHamburger ? "h" : "n"}${random}`;
  }
</script>

<div>
  <input type="checkbox" id={getUniqueId()} class="peer hidden" />
  <label for={getUniqueId()} class="w-full cursor-pointer">
    <img
      class="w-auto h-2 absolute top-2 right-0 transform transition-transform duration-200 ease-in-out"
      src={dropdownIcon}
      alt="icon"
    />
  </label>
  <ul
    class="hidden peer-checked:block py-2 ml-4"
    aria-labelledby={getUniqueId()}
  >
    {#if category.subcategories}
      {#each category?.subcategories as subcategory}
        <li
          class="relative text-wrap pr-4 py-1 hover:scale-105 transition ease-in-out"
        >
          <a href="/kategorija/{subcategory.id}">{subcategory.name}</a>
          {#if subcategory.subcategories?.length && subcategory.subcategories.length > 0}
            <svelte:self category={subcategory} />
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
