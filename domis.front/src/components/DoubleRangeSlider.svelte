<script lang="ts">
  import { createEventDispatcher } from "svelte";

  export let min: number = 0;
  export let max: number = 1000;
  export let start: number = 0;
  export let end: number = 1000;
  export let unit: string = "RSD";

  const dispatch = createEventDispatcher<{ change: string }>();

  $: if (start > end) {
    start = end;
  }

  $: if (start || end) {
    dispatch("change", { start, end });
  }
</script>

<div class="w-full flex flex-col price-range cursor-pointer">
  <span class="font-light text-center text-sm mb-2">{start} - {end} {unit}</span
  >

  <div class="relative flex flex-col gap-y-4 w-full">
    <div class="w-full h-1 bg-gray-300 rounded-full z-10 cursor-pointer">
      <div
        class="h-full rounded-full bg-gradient-to-r"
        style="background: linear-gradient(to right, transparent 0%, transparent {((start -
          min) /
          (max - min)) *
          100}%, #2F2F2F {((start - min) / (max - min)) *
          100}%, #2F2F2F {((end - min) / (max - min)) *
          100}%, transparent {((end - min) / (max - min)) *
          100}%, transparent 100%);"
      ></div>
    </div>

    <input
      class="absolute mt-0.5 h-0 w-full accent-domis-accent appearance-none z-20 cursor-pointer"
      type="range"
      bind:value={start}
      {min}
      {max}
      step="10"
      style="pointer-events: all;"
    />

    <input
      class="absolute mt-0.5 h-0 w-full accent-domis-accent appearance-none z-20 cursor-pointer"
      type="range"
      bind:value={end}
      {min}
      {max}
      step="10"
      style="pointer-events: all;"
    />
  </div>
</div>
