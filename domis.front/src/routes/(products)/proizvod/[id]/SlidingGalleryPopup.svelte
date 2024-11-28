<script lang="ts">
  import SlidingGallery from "./SlidingGallery.svelte";

  export let show = false;
  export let images: Array<Image>;
  export let selectedImageUrl: string;

  const closePopup = () => {
    show = false;
  };
</script>

<div
  class="fixed inset-0 flex items-center justify-center bg-domis-dark bg-opacity-50 z-50"
>
  <div
    id="sliding-gallery-popup"
    class="relative w-auto flex flex-col gap-y-6 items-center bg-white p-10 rounded-lg shadow-lg text-center"
  >
    <div class="absolute top-2 right-2">
      <button
        class="text-light bg-domis-dark text-white px-2 py-2 rounded-lg text-center tracking-widest hover:bg-gray-600"
        on:click={closePopup}
        ><svg
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          stroke-width="2"
          stroke="currentColor"
          class="size-4"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            d="M6 18 18 6M6 6l12 12"
          />
        </svg>
      </button>
    </div>
    <SlidingGallery {images} {selectedImageUrl} />
  </div>
</div>
