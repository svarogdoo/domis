<script lang="ts">
  import arrow from "$lib/icons/dropdown.svg";
  import { handleImageError } from "../../../../helpers/imageFallback";

  export let images: Array<Image>;
  export let selectedImageUrl: string | undefined = undefined;

  let currentIndex = calculateIndex();
  let startX = 0; // Starting touch position
  let currentTranslate = 0; // Current translation of the slider
  let prevTranslate = 0; // Translation before the gesture
  let isDragging = false; // Flag to check if dragging is active
  let isTransitioning = false; // Flag to enable smooth animation

  function calculateIndex() {
    if (!selectedImageUrl) return 0;

    let index = images.findIndex((image) => image.url === selectedImageUrl);
    console.info(index);

    return index >= 0 ? index : 0;
  }

  const slideTo = (index: number) => {
    isTransitioning = true; // Enable transition
    currentIndex = Math.max(0, Math.min(index, images.length - 1));
    currentTranslate = -currentIndex * 100;
    prevTranslate = currentTranslate;

    // Disable transition after animation completes
    setTimeout(() => {
      isTransitioning = false;
    }, 300); // Match CSS transition duration (0.3s)
  };

  const handleTouchStart = (event: TouchEvent) => {
    startX = event.touches[0].clientX;
    isDragging = true;
    prevTranslate = -currentIndex * 100; // Initial position
  };

  const handleTouchMove = (event: TouchEvent) => {
    if (!isDragging) return;
    const currentX = event.touches[0].clientX;
    const deltaX = currentX - startX;

    // Update translation based on touch movement
    currentTranslate = prevTranslate + (deltaX / window.innerWidth) * 100;
  };

  const handleTouchEnd = () => {
    if (!isDragging) return;
    isDragging = false;

    // Determine swipe direction and adjust index
    const delta = (currentTranslate - prevTranslate) / 100;

    if (delta > 0.3) {
      currentIndex = Math.max(currentIndex - 1, 0);
    } else if (delta < -0.3) {
      currentIndex = Math.min(currentIndex + 1, images.length - 1);
    }

    // Snap to the closest slide
    currentTranslate = -currentIndex * 100;
    prevTranslate = currentTranslate;
  };
</script>

<div
  class="relative w-full max-w-lg mx-auto overflow-x-hidden touch-pan-x"
  on:touchstart={handleTouchStart}
  on:touchmove={handleTouchMove}
  on:touchend={handleTouchEnd}
>
  <!-- Slider Container -->
  <div
    class="flex"
    style="transform: translateX({currentTranslate}%); transition: {isDragging
      ? 'none'
      : 'transform 0.3s ease-in-out'};"
  >
    {#each images as image}
      <img
        src={image.url}
        on:error={handleImageError}
        alt="Photo {currentIndex + 1}"
        class="min-w-full object-cover h-auto aspect-square lg:rounded-lg"
      />
    {/each}
  </div>

  <!-- Left Button -->
  {#if currentIndex != 0}
    <button
      on:click={() => slideTo(currentIndex - 1)}
      class="absolute top-1/2 -translate-y-1/2 left-0 bg-white py-2 pr-1 lg:py-4 lg:pr-2 shadow-lg hover:bg-gray-200"
    >
      <img src={arrow} alt="&rarr;" class="rotate-90" />
    </button>
  {/if}

  <!-- Right Button -->
  {#if currentIndex != images.length - 1}
    <button
      on:click={() => slideTo(currentIndex + 1)}
      class="absolute top-1/2 -translate-y-1/2 right-0 bg-white py-2 pl-1 lg:py-4 lg:pl-2 shadow-lg hover:bg-gray-200"
    >
      <img src={arrow} alt="&rarr;" class="-rotate-90" />
    </button>
  {/if}
</div>
