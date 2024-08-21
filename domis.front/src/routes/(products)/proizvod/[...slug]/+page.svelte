<script lang="ts">
  import { onMount } from "svelte";
  import { page } from "$app/stores";
  import { getLastSlug } from "../../../../helpers/slugParsing";
  import { getProduct } from "../../../../services/product-service";
  import { formatPrice } from "../../../../helpers/numberFormatter";

  let product: Product = {};
  let featureImage: string | undefined;
  let slug;

  $: slug = $page.params.slug;
  $: if (slug) {
    setProduct(slug);
  }
  $: if (product.images) {
    featureImage = product.images.find((x) => x.type === "Featured")?.url;
  }

  async function setProduct(slug: string) {
    let lastSlug = getLastSlug(slug);
    if (lastSlug) {
      product = await getProduct(Number.parseInt(lastSlug));
    }
  }

  onMount(async () => {
    setProduct(slug);
  });
</script>

<section class="w-full flex gap-x-12">
  <img
    class="w-4/5 h-auto object-cover rounded-lg"
    src={"https://cdn.speedsize.com/e0ef94ef-bbea-450b-a400-575c3145c135/www.tilebar.com/media/wysiwyg/Homepage/Hero/hp-all-collections-new.jpg?01"}
    alt={product.name}
  />
  <div class="w-full flex flex-col">
    <div class="flex pb-2 justify-between border-b border-gray-400 items-end">
      <h2 class="text-2xl">{product.name}</h2>
      <p class="text-gray-400 font-thin">SKU:{product.sku}</p>
    </div>
    <div class="flex mt-4 gap-x-2 font-extralight tracking-wide text-lg">
      <p>Retificirana</p>
      <p>|</p>
      <p>Anka</p>
      <p>|</p>
      <p>45x45</p>
    </div>
    <div
      class="flex mt-6 pb-4 gap-x-20 tracking-wide font-extralight border-b border-gray-400"
    >
      <div class="flex flex-col gap-y-4">
        <p>Pakovanje:1.62m²</p>
        <p class="text-xl">{formatPrice(product.price)} RSD</p>
      </div>
      <div class="flex flex-col gap-y-4">
        <p>Paleta:84.42m²</p>
        <p class="text-xl">{formatPrice(product.price)} RSD</p>
      </div>
    </div>
    <!-- Input -->
    <div class="flex justify-between mt-4">
      <input
        type="text"
        placeholder="Unesite broj m2"
        class="py-3 px-5 outline-none border border-gray-400 text-sm font-extralight tracking-wider"
      />
      <input
        type="text"
        placeholder="ili unesite broj pakovanja"
        class="py-3 px-5 outline-none border border-gray-400 text-sm font-extralight tracking-wider"
      />
    </div>
    <!-- Checkbox -->
    <div class="relative flex mt-4 gap-x-2 items-center">
      <input
        type="checkbox"
        class="appearance-none h-5 w-5 cursor-pointer rounded-md border border-gray-500 checked:bg-gray-500"
        checked
      />
      <span class="absolute text-white left-0.5">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          class="h-3.5 w-3.5"
          viewBox="0 0 20 20"
          fill="currentColor"
          stroke="currentColor"
          stroke-width="1"
        >
          <path
            fill-rule="evenodd"
            d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
            clip-rule="evenodd"
          ></path>
        </svg>
      </span>
      <p class="text-gray-500 pt-1 text-sm font-extralight tracking-wider">
        Dodajte 10% za škart (preporučeno)
      </p>
    </div>
    <!-- Naplata -->
    <div class="flex mt-6 justify-between">
      <p class="tracking-wider font-semibold text-lg">Ukupan iznos</p>
      <div class="flex flex-col">
        <p class="text-2xl">2544,00 RSD</p>
        <p class="text-gray-500 font-extralight">2 kutije pokrivaju 3.24 m²</p>
      </div>
    </div>
    <button
      class="bg-black mt-2 text-white py-3 uppercase tracking-widest hover:bg-gray-700"
      >Dodaj u korpu</button
    >
  </div>
</section>
