<script lang="ts">
  import { onMount } from "svelte";
  import { page } from "$app/stores";
  import { getLastSlug } from "../../../../helpers/slugParsing";
  import { getProduct } from "../../../../services/product-service";
  import {
    formatPrice,
    formatToTwoDecimals,
  } from "../../../../helpers/numberFormatter";
  import fallbackImage from "$lib/assets/backup.jpg";
  import SurfaceQuantity from "./SurfaceQuantity.svelte";
  import { mapQuantityTypeToString, QuantityType } from "../../../../enums";

  let product: Product;
  let quantityType: QuantityType = QuantityType.Piece;
  let quantityTypeString = mapQuantityTypeToString(quantityType);
  let featuredImage = fallbackImage;
  let slug;

  let isExtraChecked = false;
  let productPrice: ProductPricing = {
    perUnit: 123,
    perBox: 234,
    perPallet: 2234,
  };
  let productSize: ProductSizing = {
    box: 1.2,
    pallet: 60,
  };

  $: slug = $page.params.slug;
  $: if (product?.images) {
    let imageUrl = product?.images.find((x) => x.type === "Featured")?.url;
    if (imageUrl) featuredImage = imageUrl;
  }

  async function setProduct(slug: string) {
    let lastSlug = getLastSlug(slug);
    if (lastSlug) {
      product = await getProduct(Number.parseInt(lastSlug));
      quantityType = product.quantityType;
      quantityTypeString = mapQuantityTypeToString(quantityType);
      productPrice.perUnit = product.price;
      productPrice.perBox = product.price * productSize.box;
      productPrice.perPallet = product.price * productSize.pallet * 0.9;
    }
  }

  onMount(async () => {
    setProduct(slug);
  });
</script>

{#if product}
  <section
    class="w-full flex flex-col lg:flex-row items-center lg:items-start gap-x-12 gap-y-4"
  >
    <div class="w-4/5">
      <img
        src={featuredImage}
        alt={product?.name}
        class="w-full h-auto aspect-square object-cover rounded-lg"
      />
    </div>
    <div class="w-full flex flex-col justify-start">
      <!-- Title -->
      <div
        class="flex pb-2 justify-between border-b border-gray-400 items-end px-2"
      >
        <div class="flex flex-col gap-y-2">
          <h2 class="text-lg lg:text-2xl">{product.name}</h2>
          <div
            class="flex gap-x-2 font-extralight tracking-wide text-sm lg:text-lg"
          >
            <p>Retificirana</p>
            <p>|</p>
            <p>Anka</p>
            <p>|</p>
            <p>45x45</p>
          </div>
        </div>
        <p class="text-gray-400 font-thin">SKU:{product.sku}</p>
      </div>
      <!-- Pricing -->
      <div
        class="flex flex-col px-3 py-3 gap-y-2 tracking-wide font-extralight border-b border-gray-400"
      >
        {#if productPrice?.perUnit}
          <p class="text-xs lg:text-sm">
            RSD <span class="font-light text-black text-sm lg:text-lg px-2"
              >{formatPrice(productPrice.perUnit)}</span
            >
            po {quantityTypeString}
          </p>
        {/if}
        {#if productPrice?.perBox && productSize?.box}
          <p class="text-xs lg:text-sm">
            RSD <span class="font-light text-black text-sm lg:text-lg px-2"
              >{formatPrice(productPrice.perBox)}</span
            >
            po pakovanju ({productSize.box}
            {quantityTypeString})
          </p>
        {/if}
        {#if productPrice?.perPallet && productSize?.pallet}
          <p class="text-xs lg:text-sm">
            RSD <span class="font-light text-black text-sm lg:text-lg px-2"
              >{formatPrice(productPrice.perPallet)}</span
            >
            po paleti ({productSize.pallet}
            {quantityTypeString} | {formatToTwoDecimals(
              productSize?.pallet / productSize?.box
            )} pakovanja)
          </p>
        {/if}
      </div>

      <SurfaceQuantity
        bind:isExtraChecked
        {productPrice}
        {productSize}
        {quantityType}
      />
      <button
        class="bg-black mt-2 text-white py-3 uppercase tracking-widest hover:bg-gray-700"
        >Dodaj u korpu</button
      >
    </div>
  </section>
{/if}
