<script>
  import Header from "./Header.svelte";
  import "./styles.css";
</script>

<div class="app">
  <Header />

  <main>
    <slot />
  </main>

  <!-- <footer>
      <p>© Copyright <a href="https://svarog.io/">svarog.io</a> 2024</p>
    </footer> -->
</div>

<style>
  .app {
    display: flex;
    flex-direction: column;
    height: 100%;
    width: 100%;
    background-color: white;
    align-items: center;
  }

  main {
    display: flex;
    flex-direction: column;
    width: 100%;
    height: 100%;
  }
</style>
