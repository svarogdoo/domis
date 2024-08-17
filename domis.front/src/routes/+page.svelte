<script lang="ts">
  import { onMount } from "svelte";
  import CategoryCard from "./CategoryCard.svelte";

  let url = "https://localhost:7210/WeatherForecast";
  let test = [];

  onMount(async () => {
    const res = await fetch(url);
    test = await res.json();
    console.info(test);
  });
</script>

<section class="mt-4">
  <div class="category-cards grid grid-cols-3 gap-x-4 gap-y-4">
    <CategoryCard />
    <CategoryCard />
    <CategoryCard />
    <CategoryCard />
    <CategoryCard />
    <CategoryCard />
  </div>
</section>

<style>
  section {
    width: 100%;
    display: flex;
    justify-content: center;
  }

  .category-cards {
    width: 80%;
  }
</style>
