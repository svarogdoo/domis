<script lang="ts">
  import { goto } from "$app/navigation";
  import { page } from "$app/stores";
  import { filters } from "../filter";
  import Filter from "../Filter.svelte";

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

  $: {
    const params = $page.url.searchParams;

    // Update price filters
    filterChoices.price.minPrice =
      parseInt(params.get("minPrice") ?? "") || filters.price.minValue;
    filterChoices.price.maxPrice =
      parseInt(params.get("maxPrice") ?? "") || filters.price.maxValue;

    // Update width filters
    filterChoices.width.minWidth =
      parseInt(params.get("minWidth") ?? "") || filters.width.minValue;
    filterChoices.width.maxWidth =
      parseInt(params.get("maxWidth") ?? "") || filters.width.maxValue;

    // Update height filters
    filterChoices.height.minHeight =
      parseInt(params.get("minHeight") ?? "") || filters.height.minValue;
    filterChoices.height.maxHeight =
      parseInt(params.get("maxHeight") ?? "") || filters.height.maxValue;
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

<div class="flex flex-col gap-y-6">
  <Filter
    filter={filters.price}
    initMin={filterChoices.price.minPrice}
    initMax={filterChoices.price.maxPrice}
    on:change={handlePriceFilterChange}
  />
  <Filter
    filter={filters.width}
    initMin={filterChoices.width.minWidth}
    initMax={filterChoices.width.maxWidth}
    on:change={handleWidthFilterChange}
  />
  <Filter
    filter={filters.height}
    initMin={filterChoices.height.minHeight}
    initMax={filterChoices.height.maxHeight}
    on:change={handleHeightFilterChange}
  />
  <button
    on:click={setFilters}
    class="text-light bg-domis-dark text-white py-1 px-2 rounded-lg text-center tracking-widest hover:bg-gray-600 disabled:bg-gray-400"
    >Primeni</button
  >
</div>
