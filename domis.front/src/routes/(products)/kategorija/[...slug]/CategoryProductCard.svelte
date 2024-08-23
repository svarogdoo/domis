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
  href="/proizvod/2103"
  class="product-card flex flex-col p-2 hover:scale-105 transition ease-in-out"
>
  <img
    class="w-full h-60 object-cover rounded-lg mb-4"
    src={featuredImage}
    alt={product.name}
    loading="lazy"
  />
  <div class="flex flex-col mx-2">
    <div class="flex justify-between">
      <div class="flex flex-col gap-y-2 font-extralight">
        <p class="text-lg lg:text-xl">{product?.name}</p>
        <div class="flex gap-x-1">
          <p>Retificirana</p>
          <p>|</p>
          <p>Anka</p>
        </div>
      </div>
    </div>
    <div
      class="flex flex-col gap-y-2 lg:flex-row mt-6 mb-3 justify-between items-center"
    >
      <div class="flex gap-x-1">
        <p class="text-xl">{formatPrice(product.price)}</p>
        <p class="text-lg text-gray-800 font-thin">RSD</p>
        <p class="text-sm text-gray-400 pt-1 font-thin">po m²</p>
      </div>
      <a
        href="/category/product/product"
        class="order text-sm px-4 py-2 bg-gray-100 hover:bg-gray-200">NARUČI</a
      >
    </div>
  </div>
</a>

<style>
  .product-card {
    box-shadow: 0px 0px 9.53711px 2.86113px rgba(0, 0, 0, 0.25);
  }

  .order {
    letter-spacing: 0.1em;
  }
</style>
