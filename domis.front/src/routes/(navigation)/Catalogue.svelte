<script lang="ts">
  import { categories } from "../../stores/categories";

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
    selectedCategory = categoriesList[0];
    setSubcategoriesList();
  });

  function setSubcategoriesList() {
    list = selectedCategory.subcategories
      ? flattenCategories(selectedCategory.subcategories)
      : [];
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

<div class="flex w-full pt-6 pb-4 items-start">
  <!-- Top level categories -->
  <div class="flex flex-col w-1/5">
    {#each categoriesList as category}
      <a
        class="hover:bg-domis-light py-3 pl-2 rounded-lg h-full"
        href="/kategorija/{category.id}"
        on:mouseenter={() => {
          selectedCategory = category;
          setSubcategoriesList();
        }}>{category.name}</a
      >
    {/each}
  </div>

  <!-- Subcategories -->
  {#if selectedCategory}
    <div class="flex flex-col w-4/5 pl-12 gap-y-2">
      <h2 class="font-semibold text-lg">{selectedCategory.name}</h2>

      <div class="flex flex-col flex-wrap gap-y-1 h-96">
        {#each list as listItem}
          <a
            href="/kategorija/{listItem.id}"
            class="text-md {listItem.isTopLevel
              ? 'font-semibold mt-2'
              : 'font-light'}">{listItem.name}</a
          >
        {/each}
      </div>
    </div>
  {/if}
</div>
