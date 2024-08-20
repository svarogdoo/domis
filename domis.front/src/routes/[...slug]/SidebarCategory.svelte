<script lang="ts">
  import dropdownIcon from "$lib/icons/dropdown.svg";
  export let category: Category = {
    name: "",
    id: "11111",
    subcategories: [
      {
        name: "",
        id: "1222",
        subcategories: [
          {
            name: "",
            id: "13333",
          },
        ],
      },
    ],
  };
</script>

<div>
  <input type="checkbox" id={category.id} class="peer hidden" />
  <label for={category.id} class="w-full flex justify-between">
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
        <li class="relative">
          <a href={subcategory.id}>{subcategory.name}</a>
          {#if subcategory.subcategories}
            <svelte:self category={subcategory} />
          {/if}
        </li>
      {/each}
    {/if}
  </ul>
</div>
