<script lang="ts">
  import { page } from "$app/stores";
  import searchIcon from "$lib/icons/search.svg";
  import cartIcon from "$lib/icons/cart.svg";
  import Hamburger from "../../components/Hamburger.svelte";
  import { cart } from "../../stores/cart";
  import { onDestroy, onMount } from "svelte";
  import UserDropdown from "./UserDropdown.svelte";
  import { productService } from "../../services/product-service";
  import { goto } from "$app/navigation";
  import Catalogue from "./Catalogue.svelte";

  export let sidebar = false;

  let searchTerm: string = "";
  let searchResults: Array<ProductBasicInfo> = [];
  let cartProducts: Array<CartProduct> | undefined;
  let isDropdownOpen: boolean = false;
  let showCatalogue: boolean = false;

  $: cartProducts = $cart?.items;

  // Function to handle search input and call the product service
  let debounceTimer: number;

  async function handleSearchInput() {
    if (searchTerm.trim().length >= 3) {
      searchResults = await productService.searchProducts(searchTerm);
    } else {
      searchResults = [];
    }
  }

  function debounceSearch() {
    isDropdownOpen = true;
    clearTimeout(debounceTimer);
    debounceTimer = setTimeout(() => {
      handleSearchInput();
    }, 700);
  }

  const handleClickOutside = (event: MouseEvent) => {
    const searchField = document.getElementById("search-option");
    if (searchField && !searchField.contains(event.target as Node)) {
      isDropdownOpen = false;
    }
  };

  function handleSearchOptionClick(product: ProductBasicInfo) {
    isDropdownOpen = false;
    searchTerm = product.name;
    goto(`/proizvod/${product.id}`);
  }

  onMount(() => {
    document.addEventListener("click", handleClickOutside);
    return () => {
      document.removeEventListener("click", handleClickOutside);
    };
  });
</script>

<header class="flex flex-col items-center w-full mb-4 shadow-md">
  <!-- Top bar -->
  <div
    class="hidden lg:flex justify-center w-full bg-black text-xs text-domis-light"
  >
    <div class="flex w-full xl:w-4/5 2xl:3/5 py-2 items-center justify-between">
      <div class="flex gap-x-12">
        <p>Beograd</p>
        <p>Kontakt</p>
      </div>
      <div class="flex gap-x-12">
        <p>Usluge</p>
        <p>Isporuke i preuzimanje</p>
      </div>
    </div>
  </div>

  <nav class="flex w-full justify-center xl:w-4/5 2xl:3/5">
    <ul class="flex w-full justify-between items-center px-2 lg:px-0 lg:my-4">
      <div class="flex items-center gap-x-4">
        <li>
          <Hamburger bind:open={sidebar} />
        </li>
        <li
          class="header-title pl-4 lg:pl-0 text-sm lg:text-xl"
          aria-current={$page.url.pathname === "/" ? "page" : undefined}
        >
          <a href="/" class="text-domis-primary">domis</a>
          <a href="/" class="text-domis-dark">enterijeri</a>
        </li>
      </div>

      <li class="w-full hidden lg:flex justify-center">
        <div class="relative w-2/3 flex items-center">
          <input
            class="pl-5 py-3 rounded-lg font-extralight border bg-domis-light border-gray-300 placeholder:text-gray-500 placeholder:text-sm placeholder:tracking-wide w-full"
            type="text"
            id="search-field"
            placeholder="Upišite ime ili šifru proizvoda"
            autocomplete="off"
            bind:value={searchTerm}
            on:input={debounceSearch}
          />
          <div
            class="absolute right-0 flex items-center justify-center h-full bg-domis-dark w-16 rounded-lg"
          >
            <img src={searchIcon} alt="search" class="w-6 h-auto" />
          </div>

          {#if isDropdownOpen && searchResults.length > 0}
            <ul
              class="absolute top-full left-0 w-full bg-white border border-gray-300 rounded-md mt-1 z-50"
            >
              {#each searchResults as product}
                <li
                  id="search-option"
                  class="px-4 py-2 hover:bg-gray-100 cursor-pointer"
                >
                  <button on:click={() => handleSearchOptionClick(product)}
                    >{product.name} ({product.sku})</button
                  >
                </li>
              {/each}
            </ul>
          {/if}
        </div>
      </li>
      <div class="flex items-center lg:gap-x-4">
        <li>
          <UserDropdown />
        </li>
        <li
          class="relative h-8 w-8 lg:h-10 lg:w-10"
          aria-current={$page.url.pathname === "/shop" ? "page" : undefined}
        >
          <a href="/korpa">
            <img src={cartIcon} alt="cart" class="h-8 w-8 lg:h-10 lg:w-10" />
          </a>
          {#if cartProducts && cartProducts?.length > 0}
            <div
              class="absolute top-0 right-0 text-center text-white text-sm rounded-full h-5 w-5 bg-domis-primary"
            >
              {cartProducts.length}
            </div>
          {/if}
        </li>
      </div>
    </ul>
  </nav>

  <!-- Bottom nav -->
  <div
    class="hidden lg:flex w-full xl:w-4/5 2xl:3/5 items-center justify-between my-2 tracking-wide"
  >
    <button
      on:click={() => {
        showCatalogue = !showCatalogue;
      }}
      class="bg-domis-dark text-domis-light rounded-lg px-8 py-3"
      >Katalog</button
    >
    <p>Podovi i obloge</p>
    <p>Pločice i graniti</p>
    <p>Kupatila</p>
    <p>Zid i dekoracije</p>
    <p>Rasveta</p>
    <p>Građevinski materijal i alati</p>
    <p class=" text-domis-primary">Akcija</p>
  </div>

  {#if showCatalogue}
    <div class="flex w-full justify-center xl:w-4/5 2xl:3/5 slide-down">
      <Catalogue />
    </div>
  {/if}
</header>

<style>
  .header-title {
    display: flex;
    flex-direction: column;
  }
  .header-title a {
    text-decoration: none;
    font-weight: 600;
    letter-spacing: 0.3em;
    line-height: 1.3em;
  }
</style>
