<script lang="ts">
  export let isActive: boolean;
  export let activeText: string;
  export let inActiveText: string;
</script>

<label class="inline-flex items-center cursor-pointer">
  <input type="checkbox" bind:checked={isActive} class="sr-only peer" />
  <div
    class="relative w-11 h-6 bg-gray-600 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-green-500"
  ></div>
  {#if isActive}
    <span class="ms-3 font-medium">{activeText}</span>
  {:else}
    <span class="ms-3 font-medium">{inActiveText}</span>
  {/if}
</label>
