<script lang="ts">
  import { page } from "$app/stores";

  let searchTerm: string;
</script>

<header>
  <nav>
    <ul>
      <li aria-current={$page.url.pathname === "/" ? "page" : undefined}>
        <a href="/">domis enterijeri</a>
      </li>
      <input
        type="text"
        id="search-field"
        placeholder="Enter Search Term"
        autocomplete="off"
        bind:value={searchTerm}
        on:input
      />
      <li aria-current={$page.url.pathname === "/shop" ? "page" : undefined}>
        <a href="/shop">ŠOP</a>
      </li>
    </ul>
  </nav>
</header>

<style>
  header {
    display: flex;
    width: 100%;
    justify-content: space-between;
  }

  nav {
    display: flex;
    width: 100%;
    justify-content: center;
  }

  ul {
    position: relative;
    display: flex;
    width: 100%;
    margin-top: 2.3em;
    padding: 0em 1em;
    list-style: none;
    justify-content: space-around;
    align-items: center;
    color: whitesmoke;
    letter-spacing: 0.2em;
    font-size: smaller;
    max-width: 800px;
  }

  a:hover {
    color: white;
  }
</style>
