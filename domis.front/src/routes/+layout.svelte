<script>
  import Header from "./Header.svelte";
  import "./styles.css";
  import "../app.css";
  import Footer from "./Footer.svelte";
  import HeaderSidebar from "./HeaderSidebar.svelte";

  let open = false;
</script>

<div
  class="relative w-full h-full bg-white flex flex-col justify-between items-center 2xl:w-3/4 2xl:m-auto"
>
  <HeaderSidebar bind:open />
  <Header bind:sidebar={open} />

  <main class="w-full flex flex-col pb-8">
    <slot />
  </main>

  <Footer />
</div>
