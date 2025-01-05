<script lang="ts">
  export let min: number;
  export let max: number;
  export let value: number;
  export let unit: string;

  function roundToNearest10(val: number): number {
    return Math.round(val / 10) * 10;
  }

  $: value = roundToNearest10(value);
</script>

<div>
  <div class="price-range">
    <span class="text-sm">{value}</span>
    <span class="text-sm">{unit}</span>
    <input
      class="w-full accent-domis-accent"
      type="range"
      name=""
      bind:value
      {min}
      {max}
    />
    <div class="-mt-2 flex w-full justify-between">
      <span class="text-sm text-gray-600">{min}</span>
      <span class="text-sm text-gray-600">{max}</span>
    </div>
  </div>
</div>
