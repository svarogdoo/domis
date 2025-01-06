<script lang="ts">
  import { createEventDispatcher } from "svelte";
  import DoubleRangeSlider from "../../../../components/DoubleRangeSlider.svelte";

  export let filter: FilterData;

  const dispatch = createEventDispatcher<{ change: string }>();
</script>

<div class="w-full flex gap-x-8 items-center">
  <p class="mt-6">{filter.displayName}</p>
  <DoubleRangeSlider
    min={filter.minValue}
    max={filter.maxValue}
    start={filter.minValue}
    end={filter.maxValue}
    unit={filter.unit}
    on:change={(change) => dispatch("change", change.detail)}
  />
</div>
