<script lang="ts">
  import { createEventDispatcher } from "svelte";
  import DoubleRangeSlider from "../../../../components/DoubleRangeSlider.svelte";

  export let filter: FilterData;

  const dispatch = createEventDispatcher<{ change: string }>();
</script>

<div class="width-full flex flex-col gap-y-4">
  <p>{filter.displayName}</p>
  <DoubleRangeSlider
    min={filter.minValue}
    max={filter.maxValue}
    start={filter.minValue}
    end={filter.maxValue}
    unit={filter.unit}
    on:change={(change) => dispatch("change", change.detail)}
  />
</div>
