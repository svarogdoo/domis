<script lang="ts">
  import { page } from "$app/stores";
  import searchIcon from "$lib/icons/search.svg";
  import cartIcon from "$lib/icons/cart.svg";
  import Hamburger from "../components/Hamburger.svelte";

  export let sidebar = false;
  let searchTerm: string;
</script>

<header class="mb-4">
  <nav>
    <ul class="my-4 px-8">
      <Hamburger bind:open={sidebar} />
      <li
        class="header-title pl-4"
        aria-current={$page.url.pathname === "/" ? "page" : undefined}
      >
        <a href="/">domis</a>
        <a href="/">enterijeri</a>
      </li>
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
            on:input
          />
        </div>
      </li>
      <li aria-current={$page.url.pathname === "/shop" ? "page" : undefined}>
        <a href="/korpa"> <img src={cartIcon} alt="cart" /></a>
      </li>
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
  header {
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 100%;
    justify-content: space-between;
  }

  .category {
    display: flex;
    width: 95%;
    border-bottom-width: 1px;
    border-bottom-color: #787878;
  }

  nav {
    display: flex;
    width: 100%;
    justify-content: center;
  }

  ul {
    display: flex;
    flex-direction: row;
    width: 100%;
    list-style: none;
    justify-content: space-between;
    align-items: center;
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
