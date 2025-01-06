<script lang="ts">
  import { goto } from "$app/navigation";
  import { page } from "$app/stores";
  import { categories } from "../../../../stores/categories";
  import arrowIcon from "$lib/icons/dropdown.svg";
  import { filters } from "./filter";
  import Filter from "./Filter.svelte";

  interface UnwrappedCategory {
    name: string;
    level: number;
    id: string;
  }

  let categoriesList: Array<UnwrappedCategory>;
  let selectedCategory: Category | undefined;
  let selectedParentCategory: Category | null;
  let showCategories = true;

  let showFilters = false;
  let filterChoices = {
    price: {
      minPrice: filters.price.minValue,
      maxPrice: filters.price.maxValue,
    },
    width: {
      minWidth: filters.width.minValue,
      maxWidth: filters.width.maxValue,
    },
    height: {
      minHeight: filters.height.minValue,
      maxHeight: filters.height.maxValue,
    },
  };

  // Get the category ID from the route
  $: $page.params.id && fetchSelectedCategory($page.params.id);

  // Subscribe to categories store and filter the list based on the current id
  function fetchSelectedCategory(id: string) {
    categories.subscribe((value) => {
      selectedCategory = findCategoryById(value, id);
      if (selectedCategory?.subcategories)
        categoriesList = flattenCategories(selectedCategory.subcategories);
      else categoriesList = [];
      if (selectedCategory)
        selectedParentCategory = findParentCategory(value, id);
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

  function findParentCategory(
    categories: Category[],
    targetId: string,
    parent: Category | null = null
  ): Category | null {
    for (const category of categories) {
      if (category.id == targetId) {
        return parent; // Return the parent if the current category matches the targetId
      }

      if (category.subcategories) {
        const result = findParentCategory(
          category.subcategories,
          targetId,
          category
        );
        if (result) {
          return result; // Return the parent if found in subcategories
        }
      }
    }

    return null; // Return null if no match is found
  }

  function handlePriceFilterChange(data: any) {
    filterChoices.price.minPrice = data.detail.start;
    filterChoices.price.maxPrice = data.detail.end;
  }
  function handleWidthFilterChange(data: any) {
    filterChoices.width.minWidth = data.detail.start;
    filterChoices.width.maxWidth = data.detail.end;
  }
  function handleHeightFilterChange(data: any) {
    filterChoices.height.minHeight = data.detail.start;
    filterChoices.height.maxHeight = data.detail.end;
  }

  function setFilters() {
    const url = new URL(window.location.href);

    if (filterChoices.price.minPrice !== filters.price.minValue)
      url.searchParams.set("minPrice", filterChoices.price.minPrice.toString());
    else url.searchParams.delete("minPrice");
    if (filterChoices.price.maxPrice !== filters.price.maxValue)
      url.searchParams.set("maxPrice", filterChoices.price.maxPrice.toString());
    else url.searchParams.delete("maxPrice");

    if (filterChoices.width.minWidth !== filters.width.minValue)
      url.searchParams.set("minWidth", filterChoices.width.minWidth.toString());
    else url.searchParams.delete("minWidth");
    if (filterChoices.width.maxWidth !== filters.width.maxValue)
      url.searchParams.set("maxWidth", filterChoices.width.maxWidth.toString());
    else url.searchParams.delete("maxWidth");

    if (filterChoices.height.minHeight !== filters.height.minValue)
      url.searchParams.set(
        "minHeight",
        filterChoices.height.minHeight.toString()
      );
    else url.searchParams.delete("minHeight");
    if (filterChoices.height.maxHeight !== filters.height.maxValue)
      url.searchParams.set(
        "maxHeight",
        filterChoices.price.maxPrice.toString()
      );
    else url.searchParams.delete("maxHeight");

    goto(url.pathname + url.search);
  }
</script>

<aside class="w-full h-full flex flex-col gap-y-6">
  {#if selectedCategory}
    <button
      class="flex gap-x-4"
      on:click={() => (showCategories = !showCategories)}
    >
      <h2 class="font-bold text-lg">Kategorije</h2>
      <img src={arrowIcon} alt="" class="w-3 h-auto" />
    </button>
    {#if showCategories}
      <ul class="flex flex-col gap-y-2">
        {#if selectedParentCategory}
          <a href="/kategorija/{selectedParentCategory.id}">
            <div class="flex items-center gap-x-2 mb-4 italic">
              <img src={arrowIcon} alt="<-" class="h-1.5 w-auto rotate-90" />
              <p>{selectedParentCategory.name}</p>
            </div>
          </a>
        {/if}
        <a href="/kategorija/{selectedCategory.id}">{selectedCategory.name}</a>

        {#each categoriesList as category}
          <a
            href="/kategorija/{category.id}"
            class="text-md {category.level === 1
              ? 'font-normal'
              : 'font-light'} ml-{(category.level + 1) * 2}">{category.name}</a
          >
        {/each}
      </ul>
    {/if}
  {/if}
  {#if filters}
    <button class="flex gap-x-4" on:click={() => (showFilters = !showFilters)}>
      <h2 class="font-bold text-lg">Filteri</h2>
      <img src={arrowIcon} alt="" class="w-3 h-auto" />
    </button>
    {#if showFilters}
      <div class="flex flex-col gap-y-6 pr-8">
        <Filter filter={filters.price} on:change={handlePriceFilterChange} />
        <Filter filter={filters.width} on:change={handleWidthFilterChange} />
        <Filter filter={filters.height} on:change={handleHeightFilterChange} />
        <button
          on:click={setFilters}
          class="text-light bg-domis-dark text-white py-1 px-2 rounded-lg text-center tracking-widest hover:bg-gray-600 disabled:bg-gray-400"
          >Primeni</button
        >
      </div>
    {/if}
  {/if}
</aside>
