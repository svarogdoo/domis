<script lang="ts">
  import { createEventDispatcher } from "svelte";
  import DualRangeSlider from "../../../../components/DualRangeSlider.svelte";

  export let filter: FilterData;
  export let initMin: number;
  export let initMax: number;

  const dispatch = createEventDispatcher<{ change: string }>();
</script>

<div class="w-full flex gap-x-8 items-center">
  <p class="mb-5">{filter.displayName}</p>
  <DualRangeSlider
    min={filter.minValue}
    max={filter.maxValue}
    {initMin}
    {initMax}
    unit={filter.unit}
    on:change={(change) => dispatch("change", change.detail)}
  />
</div>
