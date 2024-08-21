<script>
  import Header from "./Header.svelte";
  import "./styles.css";
  import "../app.css";
</script>

<div
  class="w-full h-full bg-white flex flex-col items-center 2xl:w-3/4 2xl:m-auto"
>
  <Header />

  <main class="w-full h-full flex flex-col">
    <slot />
  </main>

  <!-- <footer>
      <p>© Copyright <a href="https://svarog.io/">svarog.io</a> 2024</p>
    </footer> -->
</div>
