<script lang="ts">
  import { categories } from "../../stores/categories";

  export let open = false;
  let showTopLevel = true;

  interface UnwrappedCategory {
    id: string;
    name: string;
    isTopLevel: boolean;
  }

  let categoriesList: Array<Category>;
  let selectedCategory: Category;
  let list: Array<UnwrappedCategory>;

  $: categories.subscribe((value) => {
    categoriesList = value.filter(
      (x) => x.name !== "Nedeljna akcija" && x.name !== "Trajno niska cena"
    );
  });

  function setSubcategoriesList() {
    list = selectedCategory.subcategories
      ? flattenCategories(selectedCategory.subcategories)
      : [];
    showTopLevel = false;
  }

  function flattenCategories(categories: Category[]): UnwrappedCategory[] {
    const result: UnwrappedCategory[] = [];

    function unwrap(category: Category) {
      // Add the current category to the result with isTopLevel set appropriately
      var isTopLevel = category.subcategories ? true : false;
      result.push({ ...category, isTopLevel });

      // Recursively unwrap each subcategory if it exists, marking them as non-top-level
      category.subcategories?.forEach((subcategory) => unwrap(subcategory));
    }

    // For each top-level category, call unwrap with isTopLevel = true
    categories.forEach((category) => unwrap(category));
    return result;
  }
</script>

<aside
  class="fixed overflow-y-scroll w-full h-svh bg-white border-r-2 pt-16 shadow-lg z-10 lg:hidden"
  class:open
>
  <!-- Top level categories -->
  {#if showTopLevel}
    <div class="flex flex-col w-full px-4 gap-y-2">
      {#each categoriesList as category}
        <button
          on:click={() => {
            selectedCategory = category;
            setSubcategoriesList();
          }}
          class="flex border bg-domis-light border-domis-accent px-3 justify-between items-center py-3 rounded-lg h-full"
        >
          <p>{category.name}</p>
          <svg
            class="-rotate-90"
            width="14"
            height="8"
            viewBox="0 0 14 8"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M1.5 1L7 6.5C8.83333 4.66667 12.5 1 12.5 1"
              stroke="black"
              stroke-width="1.3"
              stroke-linecap="round"
            />
          </svg>
        </button>
      {/each}
    </div>
  {:else}
    <div class="flex flex-col pl-4 gap-y-3">
      <span class="absolute top-1 left-12 py-3">
        <button
          on:click={() => {
            showTopLevel = true;
          }}
          ><svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            stroke-width="1.5"
            stroke="currentColor"
            class="size-6"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="M10.5 19.5 3 12m0 0 7.5-7.5M3 12h18"
            />
          </svg>
        </button>
      </span>

      <a href="/kategorija/{selectedCategory.id}" class="font-semibold text-md">
        {selectedCategory.name}</a
      >
      <div class="flex flex-col">
        {#each list as listItem}
          <a
            href="/kategorija/{listItem.id}"
            class="text-sm py-1 {listItem.isTopLevel
              ? 'font-semibold mt-2'
              : 'font-light'}">{listItem.name}</a
          >
        {/each}
      </div>
    </div>
  {/if}
</aside>

<style>
  aside {
    left: -100%;
    transition: left 0.3s ease-in-out;
  }

  .open {
    left: 0;
  }
</style>
