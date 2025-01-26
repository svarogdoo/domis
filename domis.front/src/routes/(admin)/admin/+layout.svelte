<script>
  import Snackbar from "../../../components/Snackbar.svelte";
  import { snackbarStore } from "../../../stores/snackbar";
  import AdminPanelSidebar from "./AdminPanelSidebar.svelte";
</script>

<div class="flex w-full gap-x-4 justify-center px-2 lg:px-12">
  <div class="hidden lg:flex max-h-48 mt-14 w-1/6 rounded-lg">
    <AdminPanelSidebar />
  </div>
  <div class="flex w-full lg:w-5/6 lg:mt-14">
    <slot />
  </div>
</div>

<Snackbar
  showSnackbar={$snackbarStore.show}
  message={$snackbarStore.message}
  isSuccess={$snackbarStore.isSuccess}
/>
