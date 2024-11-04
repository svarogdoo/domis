<script>
  import Sidebar from "../(products)/Sidebar.svelte";

  export let open = false;
</script>

<aside
  class="fixed overflow-y-scroll w-full h-full bg-gray-200 border-r-2 pt-16 shadow-lg z-10 lg:hidden"
  class:open
>
  <Sidebar />
</aside>

<style>
  aside {
    left: -100%;
    transition: left 0.3s ease-in-out;
  }

  .open {
    left: 0;
  }
</style>
