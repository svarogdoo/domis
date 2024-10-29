<script lang="ts">
  import { goto } from "$app/navigation";

  export let show = false;
  export let title: string;
  export let text: string;
  export let closeButtonText: string;
  export let isSuccess: boolean;
  export let goTo = "";

  const closePopup = () => {
    show = false;
    if (goTo) goto(goTo);
  };
</script>

<div
  class="fixed inset-0 flex items-center justify-center bg-domis-dark bg-opacity-50 z-50"
>
  <div class="bg-white p-6 rounded-lg shadow-lg max-w-sm w-full text-center">
    <h2 class="text-2xl font-semibold mb-10">{title}</h2>
    {#if text}
      <p class="mb-10 text-lg">
        {text}
      </p>
    {/if}
    <button
      class="text-white py-2 px-4 rounded tracking-widest
      {isSuccess === false ? 'bg-red-700' : 'bg-green-700'}
      "
      on:click={closePopup}
      >{closeButtonText}
    </button>
  </div>
</div>
