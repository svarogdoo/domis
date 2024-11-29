<script>
  import UserPanelSidebar from "./UserPanelSidebar.svelte";
</script>

<div class="flex w-full gap-x-20 justify-center">
  <div class="hidden lg:flex max-h-48 mt-24 w-1/6 rounded-lg">
    <UserPanelSidebar />
  </div>
  <div class="flex w-full px-5 lg:px-0 lg:mt-12 lg:w-4/6">
    <slot />
  </div>
</div>
