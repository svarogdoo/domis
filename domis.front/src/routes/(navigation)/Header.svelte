<script lang="ts">
  import { page } from "$app/stores";
  import searchIcon from "$lib/icons/search.svg";
  import hamburgerIcon from "$lib/icons/hamburger.svg";
  import cartIcon from "$lib/icons/cart.svg";
  import Hamburger from "../../components/Hamburger.svelte";
  import { cart } from "../../stores/cart";
  import { onMount } from "svelte";
  import UserDropdown from "./UserDropdown.svelte";
  import { productService } from "../../services/product-service";
  import { goto } from "$app/navigation";
  import Catalogue from "./Catalogue.svelte";

  export let sidebar = false;

  let searchTerm: string = "";
  let searchResults: Array<SearchResult> = [];
  let cartProducts: Array<CartProduct> | undefined;
  let isDropdownOpen: boolean = false;
  let showCatalogue: boolean = false;
  let currentUrl = $page.url.pathname;

  $: cartProducts = $cart?.items;
  $: if ($page.url.pathname !== currentUrl) {
    showCatalogue = false;
    currentUrl = $page.url.pathname;
  }

  // Function to handle search input and call the product service
  let debounceTimer: number;

  async function handleSearchInput() {
    if (searchTerm.trim().length >= 3) {
      searchResults =
        await productService.searchProductsOrCategories(searchTerm);
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

  function handleSearchOptionClick(item: SearchResult) {
    isDropdownOpen = false;
    searchTerm = item.name;

    if (item.type === "Product") {
      goto(`/proizvod/${item.id}`);
    } else if (item.type === "Category") {
      goto(`/kategorija/${item.id}`);
    }
  }

  onMount(() => {
    document.addEventListener("click", handleClickOutside);
    return () => {
      document.removeEventListener("click", handleClickOutside);
    };
  });
</script>

<header class="flex flex-col items-center w-full mb-4 shadow-md">
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
              {#each searchResults as item}
                <li
                  id="search-option"
                  class="px-4 py-2 hover:bg-gray-100 cursor-pointer"
                >
                  <button on:click={() => handleSearchOptionClick(item)}>
                    {#if item.type === "Category"}
                      <strong>{item.name}</strong>
                    {:else if item.type === "Product"}
                      {item.name} ({item.sku})
                    {/if}
                  </button>
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
  <div class="relative hidden lg:flex flex-col w-full xl:w-4/5 2xl:3/5">
    <div class="flex w-full items-center justify-between my-2 tracking-wide">
      <button
        on:click={() => {
          showCatalogue = !showCatalogue;
        }}
        class="bg-domis-primary text-domis-light rounded-lg px-6 py-3 flex gap-x-2 items-center"
      >
        <img src={hamburgerIcon} alt="icon" class="h-5 w-5" />
        <p>Katalog</p></button
      >
      <a href="/kategorija/701">Pločice i graniti</a>
      <a href="/kategorija/1">Podovi i obloge</a>
      <a href="/kategorija/105">Kupatila</a>
      <a href="/kategorija/3">Zid i dekoracije</a>
      <a href="/kategorija/688">Rasveta</a>
      <a href="/kategorija/4">Građevinski materijal i alati</a>
      <a href="/kategorija/907" class="font-semibold text-domis-primary"
        >Akcija</a
      >
    </div>
    {#if showCatalogue}
      <div class="flex w-full">
        <Catalogue />
      </div>
    {/if}
  </div>

  <!-- Mobile search -->
  <div class="w-full flex lg:hidden justify-center px-2 mb-2">
    <div class="relative w-full flex items-center">
      <input
        class="pl-4 py-1 rounded-lg font-extralight border bg-domis-light border-gray-300 placeholder:text-gray-500 placeholder:text-sm placeholder:tracking-wide w-full"
        type="text"
        id="search-field"
        placeholder="Upišite ime ili šifru proizvoda"
        autocomplete="off"
        bind:value={searchTerm}
        on:input={debounceSearch}
      />
      <div
        class="absolute right-0 flex items-center justify-center h-full bg-domis-dark w-10 rounded-lg"
      >
        <img src={searchIcon} alt="search" class="w-4 h-auto" />
      </div>

      {#if isDropdownOpen && searchResults.length > 0}
        <ul
          class="absolute top-full left-0 w-full bg-white border border-gray-300 rounded-md mt-1 z-50"
        >
          {#each searchResults as item}
            <li
              id="search-option"
              class="px-2 py-1 hover:bg-gray-100 cursor-pointer"
            >
              <button on:click={() => handleSearchOptionClick(item)}>
                {#if item.type === "Category"}
                  <strong>{item.name}</strong>
                {:else if item.type === "Product"}
                  {item.name} ({item.sku})
                {/if}
              </button>
            </li>
          {/each}
        </ul>
      {/if}
    </div>
  </div>
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
