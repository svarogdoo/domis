<script>
  import { fly } from "svelte/transition";
  export let showSpinner = false;
</script>

{#if showSpinner}
  <div
    class="fixed bottom-4 right-4 bg-domis-light shadow-lg border border-gray-300 p-3 rounded-full flex items-center justify-center"
    in:fly={{ x: 50, opacity: 1, duration: 200 }}
    out:fly={{ x: 50, opacity: 1, duration: 200 }}
  >
    <div
      class="w-6 h-6 border-4 border-gray-600 border-t-transparent rounded-full animate-spin"
    ></div>
  </div>
{/if}

<style>
  div {
    z-index: 100;
  }
</style>
