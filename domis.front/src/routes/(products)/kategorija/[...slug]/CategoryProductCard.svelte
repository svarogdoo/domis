<script lang="ts">
  import backup from "$lib/assets/backup.jpg";
  import { formatPrice } from "../../../../helpers/numberFormatter";

  export let product: CategoryProduct;
  let featuredImage = backup;

  $: if (product?.featuredImageUrl) {
    featuredImage = product.featuredImageUrl;
  }
</script>

<a
  href="/proizvod/{product.id}"
  class="product-card flex flex-col h-full w-full p-4 transition ease-in-out duration-300"
>
  <div class="w-full">
    <img
      class="w-full h-auto object-cover aspect-square rounded-lg"
      src={featuredImage}
      alt={product.name}
      loading="lazy"
    />
  </div>
  <div class="flex flex-col h-full mx-2 my-2">
    <div class="flex flex-col gap-y-2 font-extralight">
      <p class="text-md font-medium">{product?.name}</p>
      <div class="flex gap-x-1">
        <p>Retificirana</p>
        <p>|</p>
        <p>Anka</p>
      </div>
    </div>
    <div
      class="flex flex-col flex-grow gap-y-2 mt-6 mb-3 justify-end items-center"
    >
      <div class="flex items-center gap-x-1">
        <p class="text-xl">{formatPrice(product.price)}</p>
        <p class="text-sm text-gray-800 font-thin">RSD</p>
        <p class="text-sm text-gray-400 font-thin">po m²</p>
      </div>
      <a
        href="/category/product/product"
        class="w-full text-center tracking-widest text-sm px-4 py-2 rounded-md bg-gray-200 hover:bg-gray-200"
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
