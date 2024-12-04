<script>
  import { fly } from "svelte/transition";

  export let showSnackbar = false;
  export let message;
  export let isSuccess;
  function closeSnackbar() {
    var snackbar = document.getElementById("snackbar");
    if (snackbar) snackbar.style.display = "none";
  }
</script>

{#if showSnackbar}
  <div
    id="snackbar"
    class="{isSuccess
      ? 'border-green-600'
      : 'border-red-600'} shadow-xl border-4 bg-domis-light text-domis-dark py-4 px-8 rounded-md fixed bottom-4 right-4 left-4 lg:left-auto flex justify-between items-center z-50"
    in:fly={{ x: 0, y: 50, duration: 300, opacity: 1 }}
    out:fly={{ x: 0, y: 100, duration: 300, opacity: 1 }}
  >
    <p>{message}</p>
    <button class="text-domis-dark pl-4" on:click={() => closeSnackbar()}>
      <svg
        xmlns="http://www.w3.org/2000/svg"
        fill="none"
        viewBox="0 0 24 24"
        stroke="currentColor"
        class="w-4 h-4"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          stroke-width="2"
          d="M6 18L18 6M6 6l12 12"
        />
      </svg>
    </button>
  </div>
{/if}
