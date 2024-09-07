<script>
  import Sidebar from "./(products)/Sidebar.svelte";

  export let open = false;
</script>

<aside
  class="absolute w-full h-full bg-gray-200 border-r-2 pt-16 shadow-lg z-20 lg:hidden"
  class:open
>
  <Sidebar hamburgerStyle={true} />
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
