<script lang="ts">
  import { page } from "$app/stores";
  import searchIcon from "$lib/icons/search.svg";
  import cartIcon from "$lib/icons/cart.svg";
  import Hamburger from "../components/Hamburger.svelte";
  import { cart } from "../stores/cart";
  import { onDestroy, onMount } from "svelte";
  import UserDropdown from "./UserDropdown.svelte";
  import { productService } from "../services/product-service";
  import { goto } from "$app/navigation";

  export let sidebar = false;

  let searchTerm: string = "";
  let searchResults: Array<ProductBasicInfo> = [];
  let cartProducts: Array<CartProduct> | undefined;
  let isDropdownOpen: boolean = false;

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

<header class="flex flex-col items-center w-full mb-4">
  <nav class="flex w-full justify-center">
    <ul class="flex w-full justify-between items-center my-4 px-8">
      <div class="flex items-center gap-x-4">
        <li>
          <Hamburger bind:open={sidebar} />
        </li>
        <li
          class="header-title pl-4"
          aria-current={$page.url.pathname === "/" ? "page" : undefined}
        >
          <a href="/">domis</a>
          <a href="/">enterijeri</a>
        </li>
      </div>

      <li class="w-full hidden lg:flex justify-center">
        <div class="relative w-2/3 flex items-center">
          <img src={searchIcon} alt="search" class="absolute ml-4" />
          <input
            class="pl-12 py-3 rounded-md font-extralight border border-black w-full"
            type="text"
            id="search-field"
            placeholder="Pretražite prodavnicu (upišite ime ili šifru proizvoda)"
            autocomplete="off"
            bind:value={searchTerm}
            on:input={debounceSearch}
          />

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
      <div class="flex items-center gap-x-4">
        <li>
          <UserDropdown />
        </li>
        <li
          class="relative h-10 w-10"
          aria-current={$page.url.pathname === "/shop" ? "page" : undefined}
        >
          <a href="/korpa">
            <img src={cartIcon} alt="cart" class="h-10 w-10" />
          </a>
          {#if cartProducts?.length > 0}
            <div
              class="absolute top-0 right-0 text-center text-white text-sm rounded-full h-4 w-4 bg-red-500"
            >
              {cartProducts.length}
            </div>
          {/if}
        </li>
      </div>
    </ul>
  </nav>
  <div class="category h-1 gap-x-12 pl-4">
    <!-- <p>Keramika</p>
    <p>Lajsne</p>
    <p>Kamen</p>
    <p>Leksan</p> -->
  </div>
</header>

<style>
  .category {
    display: flex;
    width: 95%;
    border-bottom-width: 1px;
    border-bottom-color: #787878;
  }

  .header-title {
    display: flex;
    flex-direction: column;
  }
  .header-title a {
    color: black;
    text-decoration: none;
    font-weight: 600;
    letter-spacing: 0.3em;
    line-height: 1.3em;
  }
</style>
