<script lang="ts">
  import Header from "./(navigation)/Header.svelte";
  import "./styles.css";
  import "../app.css";
  import Footer from "./(navigation)/Footer.svelte";
  import HeaderSidebar from "./(navigation)/HeaderSidebar.svelte";
  import { page } from "$app/stores";
  import { onMount } from "svelte";
  import { cart } from "../stores/cart";
  import { userStore } from "../stores/user";

  let open = false;
  let previousSlug: string;

  $: if (previousSlug !== $page.url.pathname && open) {
    // If not the first load, toggle the state when slug changes
    if (previousSlug !== undefined) {
      open = false;
    }

    // Update the previous slug
    previousSlug = $page.url.pathname;
  }

  onMount(() => {
    initializeUserAndCart();
  });

  async function initializeUserAndCart() {
    await userStore.initialize();
    cart.initialize();
  }
</script>

<div class="relative w-full h-full bg-white flex flex-col items-center">
  <HeaderSidebar bind:open />
  <Header bind:sidebar={open} />

  <main class="w-full flex flex-col flex-grow pb-8 2xl:w-3/4 2xl:m-auto">
    <slot />
  </main>

  <Footer />
</div>
