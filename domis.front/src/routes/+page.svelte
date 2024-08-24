<script lang="ts">
  import { categories } from "../stores/categories";
  import CategoryCard from "./CategoryCard.svelte";

  let categoriesList: Array<Category>;
  $: categories.subscribe((value) => {
    categoriesList = value;
  });
</script>

<section class="mt-4 w-full flex justify-center">
  <div
    class="w-11/12 xl:w-3/4 category-cards grid grid-cols-1 lg:grid-cols-3 gap-x-4 gap-y-4"
  >
    {#each categoriesList as category}
      <CategoryCard {category} />
    {/each}
  </div>
</section>
