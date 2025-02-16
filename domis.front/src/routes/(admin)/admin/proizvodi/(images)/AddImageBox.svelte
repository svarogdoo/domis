<script lang="ts">
  import { createEventDispatcher } from "svelte";
  import updateIcon from "$lib/icons/refresh.svg";

  export let isFeatured = false;

  const dispatch = createEventDispatcher();

  let inputRef: HTMLInputElement | null = null;

  async function handleFileSelect(event: Event) {
    const target = event.target as HTMLInputElement;
    if (!target.files || target.files.length === 0) return;

    const files: File[] = Array.from(target.files);

    const base64Images = await Promise.all(
      files.map(
        (file) =>
          new Promise<string>((resolve) => {
            const reader = new FileReader();
            reader.onload = () => {
              if (typeof reader.result === "string") {
                resolve(reader.result);
              }
            };
            reader.readAsDataURL(file);
          })
      )
    );

    dispatch("click", base64Images);
  }

  function triggerFileInput() {
    inputRef?.click();
  }
</script>

{#if isFeatured}
  <button
    on:click={triggerFileInput}
    class="absolute top-0 right-0 bg-blue-500 text-white rounded-sm px-3 py-3 shadow-l hover:bg-blue-400 cursor-pointer active:scale-95"
  >
    <img
      src={updateIcon}
      alt=""
      class="w-6 h-auto hover:scale-125 transition ease-in-out"
    />
  </button>
{:else}
  <button
    on:click={triggerFileInput}
    class="w-60 h-60 bg-gray-300 rounded-lg shadow-lg flex items-center justify-center text-2xl hover:bg-gray-400 hover:shadow-none cursor-pointer active:scale-95 transition ease-in-out"
    >+
  </button>
{/if}
<input
  type="file"
  accept="image/*"
  multiple
  bind:this={inputRef}
  class="hidden"
  on:change={handleFileSelect}
/>
