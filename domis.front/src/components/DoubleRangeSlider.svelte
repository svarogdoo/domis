<script lang="ts">
  export let min: number = 0;
  export let max: number = 1000;
  export let start: number = 0; // Start value of the range
  export let end: number = 1000; // End value of the range
  export let unit: string = "$";

  // Round values to the nearest 10
  function roundToNearest10(val: number): number {
    return Math.round(val / 10) * 10;
  }

  // Ensure values are rounded to the nearest 10
  $: start = roundToNearest10(start);
  $: end = roundToNearest10(end);

  // Ensure start is not greater than end
  $: if (start > end) {
    start = end;
  }
</script>

<div>
  <div class="price-range">
    <span class="text-sm">{start} - {end} {unit}</span>

    <div class="relative w-full">
      <!-- Background for the range -->
      <div
        class="absolute w-full h-1 bg-gray-300 rounded-full"
        style="top: 50%; left: 0; z-index: 1;"
      >
        <!-- This is the fill section between start and end -->
        <div
          class="h-full bg-green-500 rounded-full"
          style="width: {((end - start) / (max - min)) * 100}%"
        ></div>
      </div>

      <!-- Start slider -->
      <input
        class="absolute w-full accent-domis-accent"
        type="range"
        bind:value={start}
        {min}
        {max}
        step="10"
        style="z-index: 2; pointer-events: all;"
      />

      <!-- End slider with background fill -->
      <input
        class="absolute w-full accent-domis-accent"
        type="range"
        bind:value={end}
        {min}
        {max}
        step="10"
        style="z-index: 3; pointer-events: all;"
      />
    </div>

    <div class="-mt-2 flex w-full justify-between">
      <span class="text-sm text-gray-600">{min}</span>
      <span class="text-sm text-gray-600">{max}</span>
    </div>
  </div>
</div>
