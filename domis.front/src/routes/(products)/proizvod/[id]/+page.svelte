<script lang="ts">
  import {
    formatPrice,
    formatToTwoDecimals,
  } from "../../../../helpers/numberFormatter";
  import SurfaceQuantity from "./SurfaceQuantity.svelte";
  import { mapQuantityTypeToString, QuantityType } from "../../../../enums";
  import { handleImageError } from "../../../../helpers/imageFallback";
  import { getCurrencyString } from "../../../../helpers/stringFormatter";
  import { userStore } from "../../../../stores/user";
  import MobileGallery from "./MobileGallery.svelte";

  export let data;

  $: product = data.props.product;
  $: productPrice = data.props.productPrice;

  let quantityType: QuantityType = QuantityType.Piece;
  let quantityTypeString = mapQuantityTypeToString(quantityType);
  let isExtraChecked = false;
  let lastCategoryPath: CategoryPath | undefined;

  $: if (product?.categoryPaths) {
    lastCategoryPath = product.categoryPaths[0].at(-1);
  }

  $: if (product?.attributes?.quantityType) {
    quantityType = product.attributes.quantityType;
    quantityTypeString = mapQuantityTypeToString(quantityType);
  }
</script>

{#if product}
  <section
    class="w-full flex flex-col gap-y-2 font-light lg:w-4/5 lg:self-center"
  >
    <div class="hidden lg:flex space-x-1 text-sm">
      {#each product.categoryPaths[0] as categoryPath, index (categoryPath.id)}
        <a
          href="/kategorija/{categoryPath.id}"
          class="relative after:content-[''] after:block after:w-0 after:h-[1px] after:bg-domis-primary after:transition-all after:duration-300 hover:after:w-full"
          >{categoryPath.name}</a
        >
        {#if index < product.categoryPaths[0].length - 1}
          <span class="mx-1 text-domis-primary">/</span>
        {/if}
      {/each}
    </div>
    {#if lastCategoryPath}
      <a
        href="/kategorija/{lastCategoryPath.id}"
        class="flex lg:hidden text-sm pl-2"
        ><span class="pr-3">←</span>{lastCategoryPath.name}</a
      >
    {/if}
    <p class="text-sm"></p>
    <div
      class="w-full flex flex-col items-center lg:flex-row lg:items-start gap-x-12 gap-y-4 cursor-pointer"
    >
      <div class="w-full flex lg:hidden">
        <!-- TODO: pass array of images -->
        <!-- TODO: Dekstop Gallery -->
        <MobileGallery />
      </div>

      <!-- <img
        src={product?.featuredImageUrl}
        alt={product?.name}
        on:error={handleImageError}
        class="w-full h-auto aspect-square object-cover rounded-lg"
      /> -->
      <div class="w-full px-2 flex flex-col justify-start">
        <!-- Title -->
        <div
          class="flex pb-2 justify-between border-b border-gray-400 items-end px-2"
        >
          <div class="flex flex-col gap-y-2">
            <h2 class="text-lg lg:text-2xl">{product.name}</h2>
            <!-- <div
            class="flex gap-x-2 font-extralight tracking-wide text-sm lg:text-lg"
          >
            <p>Retificirana</p>
            <p>|</p>
            <p>Anka</p>
            <p>|</p>
            <p>45x45</p>
          </div> -->
          </div>
          <p class="text-gray-400 font-thin">SKU:{product.sku}</p>
        </div>
        <!-- Pricing -->
        <div class="w-full flex flex-col relative">
          <p
            class="absolute top-0 right-0 text-end font-light mt-2 text-green-700 tracking-wider text-sm lg:text-lg"
          >
            {product.stock}
            <span class="text-domis-accent text-xs lg:text-sm">na zalihama</span
            >
          </p>
          <div
            class="flex flex-col px-3 py-3 gap-y-2 tracking-wide font-extralight border-b border-gray-400"
          >
            {#if productPrice?.perUnit}
              <p class="text-xs lg:text-sm">
                {getCurrencyString()}
                <span class="font-light text-domis-dark text-sm lg:text-lg px-2"
                  >{formatPrice(productPrice.perUnit)}</span
                >
                po {quantityTypeString}
              </p>
            {/if}
            {#if productPrice?.perBox && product?.size?.box}
              <p class="text-xs lg:text-sm">
                {getCurrencyString()}
                <span class="font-light text-domis-dark text-sm lg:text-lg px-2"
                  >{formatPrice(productPrice.perBox)}</span
                >
                po paketu ({product?.size.box}
                {quantityTypeString})
              </p>
            {/if}
            {#if userStore.isUserVP() && productPrice?.perPallet && product?.size?.pallet && productPrice?.perPalletUnit}
              <p class="text-xs lg:text-sm">
                {getCurrencyString()}
                <span class="font-light text-domis-dark text-sm lg:text-lg px-2"
                  >{formatPrice(productPrice.perPallet)}</span
                >
                po paleti ({product?.size.pallet}
                {quantityTypeString} | {formatToTwoDecimals(
                  product?.size?.pallet / product?.size?.box
                )} paketa | cena komada {getCurrencyString()}
                {formatToTwoDecimals(productPrice.perPalletUnit)})
              </p>
            {/if}
          </div>
        </div>

        <SurfaceQuantity
          bind:isExtraChecked
          {product}
          {quantityType}
          {productPrice}
        />
      </div>
    </div>
  </section>
{/if}
