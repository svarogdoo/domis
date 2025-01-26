<script lang="ts">
  export let selectedCategoryId: string = "";
  export let categoriesList: Array<FlattenedCategory>;

  interface FlattenedCategory {
    id: string;
    name: string;
    level: number;
  }
</script>

<div class="w-full flex flex-col gap-y-2">
  {#if categoriesList?.length && categoriesList.length > 0}
    <h2 class="font-bold">Kategorije</h2>
    <select
      bind:value={selectedCategoryId}
      class="block w-full border rounded-lg px-2 py-1 bg-white font-light"
    >
      <option disabled value="">Izaberite kategoriju</option>
      {#each categoriesList as category}
        <option value={category.id}>
          <p>
            {category.name}
          </p>
        </option>
      {/each}
    </select>
  {/if}
</div>
