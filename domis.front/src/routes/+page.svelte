<script lang="ts">
  import { categories } from "../stores/categories";
  import CategoryCard from "./CategoryCard.svelte";

  let categoriesList: Array<Category>;
  $: categories.subscribe((value) => {
    categoriesList = value;
  });
  // let categories = [
  //   {
  //     id: 1,
  //     name: "Keramika",
  //     image:
  //       "https://cdn.speedsize.com/e0ef94ef-bbea-450b-a400-575c3145c135/www.tilebar.com/media/wysiwyg/Homepage/Hero/hp-all-collections-new.jpg?01",
  //   },
  //   {
  //     id: 2,
  //     name: "Leksan",
  //     image:
  //       "https://cdn.speedsize.com/e0ef94ef-bbea-450b-a400-575c3145c135/www.tilebar.com/media/wysiwyg/Homepage/Hero/hp-all-collections-new.jpg?01",
  //   },
  //   {
  //     id: 3,
  //     name: "Podovi",
  //     image:
  //       "https://cdn.speedsize.com/e0ef94ef-bbea-450b-a400-575c3145c135/www.tilebar.com/media/wysiwyg/Homepage/Hero/hp-all-collections-new.jpg?01",
  //   },
  // ];
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
