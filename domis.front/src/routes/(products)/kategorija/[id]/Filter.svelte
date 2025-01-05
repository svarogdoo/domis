<script lang="ts">
  import DoubleRangeSlider from "../../../../components/DoubleRangeSlider.svelte";
  import RangeSlider from "../../../../components/RangeSlider.svelte";

  export let filter: FilterData;
</script>

<div class="width-full flex flex-col gap-y-4">
  <p>{filter.displayName}</p>
  <!-- <RangeSlider
    min={filter.minValue}
    max={filter.maxValue}
    value={filter.maxValue}
    unit={filter.unit}
  /> -->
  <DoubleRangeSlider
    min={filter.minValue}
    max={filter.maxValue}
    start={filter.minValue}
    end={filter.maxValue}
    unit={filter.unit}
  />
</div>
