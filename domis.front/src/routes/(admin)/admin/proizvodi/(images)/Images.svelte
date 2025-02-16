<script lang="ts">
  import AddImageBox from "./AddImageBox.svelte";
  import binIcon from "$lib/icons/bin.svg";
  import updateIcon from "$lib/icons/refresh.svg";
  import { snackbarStore } from "../../../../../stores/snackbar";

  export let productId: number;
  export let images: Array<Image>;

  let featuredImage: Image | undefined;
  let galleryImages: Array<Image>;

  $: if (images) {
    featuredImage = images.find((x) => x.type === "Featured");
    galleryImages = images.filter((x) => x.type === "Gallery");
  }

  function handleUpdateFeaturedImage(
    isNew: boolean,
    event: CustomEvent<string[]>,
    imageId?: number
  ) {
    let uploadedImage = event.detail[0];
    console.info(uploadedImage);

    //TODO: call upsert endpoint

    if (imageId) {
      console.info(imageId);
      snackbarStore.showSnackbar("Uspešno sačuvana slika!", true);
    } else {
      snackbarStore.showSnackbar("Greška pri čuvanju  slike!", false);
    }
  }

  function handleAddGalleryImage(event: CustomEvent<string[]>) {
    let uploadedImages = event.detail;

    //TODO: call post endpoint

    if (uploadedImages) {
      console.info(uploadedImages);

      snackbarStore.showSnackbar(
        `Uspešno sačuvan${uploadedImages.length > 1 ? "e nove slike" : "a nova slika"}!`,
        true
      );
    } else {
      snackbarStore.showSnackbar("Greška pri čuvanju nove slike!", false);
    }
  }

  function handleDeleteGalleryImage(imageId: number) {
    console.info(imageId);

    //TODO: call delete endpoint

    if (imageId) {
      console.info(imageId);
      snackbarStore.showSnackbar("Uspešno obrisana slika!", true);
    } else {
      snackbarStore.showSnackbar("Greška pri brisanju  slike!", false);
    }
  }
</script>

<div
  class="flex w-full flex-col items-center lg:items-start gap-y-2 lg:gap-y-6 lg:px-8"
>
  <h2 class="text-lg">Glavna slika</h2>

  {#if featuredImage}
    <div class="relative">
      <AddImageBox
        isFeatured={true}
        on:click={(event) =>
          handleUpdateFeaturedImage(true, event, featuredImage?.id)}
      />
      <img
        src={featuredImage.url}
        alt="Glavna slika"
        class="w-60 h-60 object-cover rounded-lg shadow-lg"
      />
    </div>
  {:else}
    <AddImageBox on:click={(event) => handleUpdateFeaturedImage(true, event)} />
  {/if}

  <div class="w-full h-0.5 my-8 bg-gray-400"></div>

  <h2 class="text-lg">Galerija</h2>
  <div class="flex flex-wrap justify-center lg:justify-start gap-x-4 gap-y-4">
    {#if galleryImages}
      {#each galleryImages as galleryImage}
        <div class="relative">
          <button
            on:click={() => handleDeleteGalleryImage(galleryImage.id)}
            class="absolute top-0 right-0 bg-red-700 text-white rounded-sm px-3 py-3 shadow-l hover:bg-red-600 cursor-pointer active:scale-95"
          >
            <img
              src={binIcon}
              alt=""
              class="w-6 h-auto hover:scale-125 invert transition ease-in-out"
            />
          </button>
          <img
            src={galleryImage.url}
            alt="Galerija"
            class="w-60 h-60 object-cover rounded-lg shadow-lg"
          />
        </div>
      {/each}
    {/if}

    <AddImageBox on:click={handleAddGalleryImage} />
  </div>
</div>
