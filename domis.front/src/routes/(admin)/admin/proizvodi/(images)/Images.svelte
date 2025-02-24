<script lang="ts">
  import AddImageBox from "./AddImageBox.svelte";
  import binIcon from "$lib/icons/bin.svg";
  import { snackbarStore } from "../../../../../stores/snackbar";
  import {
    deleteProductGalleryImages,
    postProductGalleryImages,
    upsertFeaturedImage,
  } from "../../../../../services/product-service";
  import { createEventDispatcher } from "svelte";

  export let productId: number;
  export let images: Array<Image>;

  const dispatch = createEventDispatcher<{ save: number }>();

  let featuredImage: Image | undefined;
  let galleryImages: Array<Image>;

  $: if (images) {
    featuredImage = images.find((x) => x.type === "Featured");
    galleryImages = images.filter((x) => x.type === "Gallery");
  }

  async function handleUpdateFeaturedImage(event: CustomEvent<string[]>) {
    let uploadedImage = event.detail[0];
    let res = await upsertFeaturedImage(productId, uploadedImage);

    if (res) {
      snackbarStore.showSnackbar("Uspešno sačuvana slika!", true);
      dispatch("save", productId);
    } else {
      snackbarStore.showSnackbar(
        "Ne podržavamo zamenu glavne slike, ali biċe uskoro!",
        false
      );
    }
  }

  async function handleAddGalleryImage(event: CustomEvent<string[]>) {
    let uploadedImages = event.detail;

    if (
      uploadedImages &&
      (await postProductGalleryImages(productId, { dataUrls: uploadedImages }))
    ) {
      snackbarStore.showSnackbar(
        `Uspešno sačuvan${uploadedImages.length > 1 ? "e nove slike" : "a nova slika"}!`,
        true
      );
      dispatch("save", productId);
    } else {
      snackbarStore.showSnackbar("Greška pri čuvanju nove slike!", false);
    }
  }

  async function handleDeleteGalleryImage(imageId: number) {
    let res = await deleteProductGalleryImages(imageId, productId);
    if (imageId && res) {
      snackbarStore.showSnackbar("Uspešno obrisana slika!", true);
      dispatch("save", productId);
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
        on:click={(event) => handleUpdateFeaturedImage(event)}
      />
      <img
        src={featuredImage.url}
        alt="Glavna slika"
        class="w-60 h-60 object-cover rounded-lg shadow-lg"
      />
    </div>
  {:else}
    <AddImageBox on:click={(event) => handleUpdateFeaturedImage(event)} />
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
