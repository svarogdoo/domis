<script lang="ts">
  import { getFlattenedCategories } from "../../../../stores/categories";
  import CategoryRow from "./CategoryRow.svelte";

  let allCategories = getFlattenedCategories();
</script>

<div class="flex flex-col w-full items-center">
  <div class="flex flex-col w-full lg:w-4/5">
    <table class="w-full">
      <thead class=" bg-domis-dark text-white">
        <th class="max-w-12">Kategorija</th>
      </thead>
      <tbody class="divide-y divide-gray-200">
        {#if allCategories && allCategories.length > 0}
          {#each allCategories as category}
            <CategoryRow {category} />
          {/each}
        {/if}
      </tbody>
    </table>
  </div>
</div>

<style>
  table {
    box-shadow: 0px 4px 18px 2.25px rgba(0, 0, 0, 0.18);
  }
  th {
    padding: 8px 12px;
  }
</style>
