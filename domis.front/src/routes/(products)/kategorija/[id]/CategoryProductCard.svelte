<script lang="ts">
  import backup from "$lib/assets/backup.jpg";
  import { onMount } from "svelte";
  import { mapQuantityTypeToString } from "../../../../enums";
  import { handleImageError } from "../../../../helpers/imageFallback";
  import { formatPrice } from "../../../../helpers/numberFormatter";
  import { getCurrencyString } from "../../../../helpers/stringFormatter";
  import { userStore } from "../../../../stores/user";

  export let product: CategoryProduct;
  let featuredImage = backup;
  let quantityTypeString: string;
  let productPrice: number;

  onMount(() => {
    if (userStore.isUserVP()) productPrice = product.vpPrice;
    else productPrice = product.price;

    featuredImage = product.featuredImageUrl ?? backup;
    quantityTypeString = product.quantityType
      ? mapQuantityTypeToString(product.quantityType)
      : "";
  });
</script>

<a
  href="/proizvod/{product.id}"
  class="product-card flex flex-col h-full w-full lg:p-4 gap-y-2 transition ease-in-out duration-300"
>
  <div class="w-full">
    <img
      class="w-full h-auto object-cover aspect-square rounded-md"
      src={featuredImage}
      alt={product.name}
      on:error={handleImageError}
      loading="lazy"
    />
  </div>
  <div class="flex flex-col h-full">
    <div class="flex flex-col gap-y-2 font-extralight px-1">
      <p class="text-sm lg:text-lg font-medium">{product?.name}</p>
      <!-- <div class="gap-x-1 text-normal hidden lg:flex">
        <p>Retificirana</p>
        <p>|</p>
        <p>Anka</p>
      </div> -->
    </div>
    <div class="flex flex-col flex-grow gap-y-2 mt-2 justify-end items-center">
      <div class="flex gap-x-1 items-center">
        <p class="lg:text-xl {product.saleInfo ? 'text-domis-primary' : ''}">
          {formatPrice(
            product.saleInfo?.salePrice
              ? product.saleInfo.salePrice
              : productPrice
          )}
        </p>
        <p class="text-xs lg:text-sm text-gray-800 font-thin">
          {getCurrencyString()}
        </p>
        {#if quantityTypeString}
          <p class="text-xs lg:text-sm text-gray-400 font-thin">
            po {quantityTypeString}
          </p>
        {/if}
        {#if product.saleInfo?.salePrice}
          <p class="text-xs lg:text-sm line-through ml-2">
            {formatPrice(productPrice)}
          </p>
        {/if}
      </div>
      <a
        href="/proizvod/{product.id}"
        class="w-full text-center tracking-widest text-xs lg:text-sm px-4 py-2 lg:rounded-md bg-gray-200 hover:bg-gray-200"
        >NARUČI</a
      >
    </div>
  </div>
</a>

<style>
  .product-card:hover {
    box-shadow: 0px 0px 9.53711px 2.86113px rgba(140, 140, 140, 0.25);
  }
</style>
