<script lang="ts">
  import { handleImageError } from "../../../../helpers/imageFallback";
  import SlidingGalleryPopup from "./SlidingGalleryPopup.svelte";

  export let images: Array<Image>;

  let showSlidingGallery = false;
  let selectedImageUrl = images[0].url;

  function handleShowSlidingGallery(image: Image) {
    selectedImageUrl = image.url;
    showSlidingGallery = true;
  }
</script>

{#each images as image, index (index)}
  <button on:click={() => handleShowSlidingGallery(image)}>
    <img
      src={image.url}
      alt="product"
      class="w-full h-auto aspect-square object-cover rounded-lg"
      on:error={handleImageError}
    />
  </button>
{/each}
{#if showSlidingGallery}
  <SlidingGalleryPopup
    {selectedImageUrl}
    {images}
    bind:show={showSlidingGallery}
  />
{/if}
