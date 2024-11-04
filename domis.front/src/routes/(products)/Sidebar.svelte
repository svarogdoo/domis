<script lang="ts">
  import { page } from "$app/stores";
  import { categories } from "../../stores/categories";

  interface UnwrappedCategory {
    name: string;
    level: number;
    id: string;
  }

  let categoriesList: Array<UnwrappedCategory>;
  let selectedCategory: Category | undefined;

  // Get the category ID from the route
  $: $page.params.id && fetchSelectedCategory($page.params.id);

  // Subscribe to categories store and filter the list based on the current id
  function fetchSelectedCategory(id: string) {
    categories.subscribe((value) => {
      selectedCategory = findCategoryById(value, id);
      if (selectedCategory?.subcategories)
        categoriesList = flattenCategories(selectedCategory.subcategories);
      else categoriesList = [];
      console.info(categoriesList);
    });
  }

  function findCategoryById(
    categories: Category[],
    id: string
  ): Category | undefined {
    for (const category of categories) {
      if (category.id == id) {
        return category;
      }

      if (category.subcategories) {
        const found = findCategoryById(category.subcategories, id);
        if (found) {
          return found;
        }
      }
    }

    return undefined;
  }

  function flattenCategories(categories: Category[]): UnwrappedCategory[] {
    const result: UnwrappedCategory[] = [];

    function unwrap(category: Category, level: number) {
      // Add the category to the result with its level set, and without subcategories
      result.push({ ...category, level });

      // Recursively unwrap each subcategory, increasing the level by 1
      category.subcategories?.forEach((subcategory) =>
        unwrap(subcategory, level + 1)
      );
    }

    // Start with level 1 for top-level categories
    categories.forEach((category) => unwrap(category, 1));
    return result;
  }
</script>

<aside class="w-full h-full flex flex-col gap-y-4">
  <!-- TODO: Loader -->
  {#if selectedCategory}
    <h2 class="font-bold text-lg">Kategorije</h2>
    <ul class="flex flex-col gap-y-2">
      <a href="/kategorija/{selectedCategory.id}">{selectedCategory.name}</a>

      {#each categoriesList as category}
        <a
          href="/kategorija/{category.id}"
          class="text-md {category.level === 1
            ? 'font-normal'
            : 'font-light'} ml-{category.level * 2}">{category.name}</a
        >
      {/each}
    </ul>
  {/if}
</aside>
