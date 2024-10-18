<script>
  import UserPanelSidebar from "./UserPanelSidebar.svelte";
</script>

<div class="flex w-full gap-x-20 justify-between">
  <div class="hidden lg:flex max-h-48 mt-14 w-1/5 rounded-lg">
    <UserPanelSidebar />
  </div>
  <div class="flex overflow-y-scroll w-full px-5 lg:px-0 lg:w-3/5">
    <slot />
  </div>
</div>
