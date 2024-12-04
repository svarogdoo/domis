<script lang="ts">
  import { onMount } from "svelte";
  import { getAdminUsers } from "../../../../services/admin-service";
  import AdminUserItem from "./AdminUserItem.svelte";
  import Snackbar from "../../../../components/Snackbar.svelte";

  let users: Array<AdminUser>;

  let snackbarMessage: string;
  let isSnackbarSuccess: boolean;
  let showSnackbar = false;

  onMount(() => {
    setAdminUsers();
  });

  function handleShowSnackbar() {
    showSnackbar = true;
    setTimeout(function () {
      showSnackbar = false;
    }, 3000); // Close after 3s
  }

  async function setAdminUsers() {
    const tempUsers = await getAdminUsers();
    users = tempUsers.sort((a, b) => {
      if (a.userName < b.userName) return -1;
      if (a.userName > b.userName) return 1;
      return 0;
    });
  }

  async function handleSave(event: any) {
    if (event?.detail?.success) {
      snackbarMessage = "Uspešno sačuvana rola korisnika!";
      isSnackbarSuccess = true;
      await setAdminUsers();
    } else {
      snackbarMessage = "Greška pri čuvanju role korisnika!";
      isSnackbarSuccess = false;
    }

    handleShowSnackbar();
  }
</script>

<div class="flex flex-col px-12">
  {#if users?.length && users.length > 0}
    <table class="table table-hover">
      <thead class="w-full bg-domis-dark text-white">
        <th class="text-start w-40">Ime</th>
        <th class="text-start w-40">Prezime</th>
        <th class="text-start w-40">Email</th>
        <th class="text-start w-32">Tip</th>
      </thead>
      <tbody class="divide-y divide-gray-200">
        {#each users as user (user.userId)}
          <AdminUserItem bind:user on:save={handleSave} />
        {/each}
      </tbody>
    </table>
  {/if}
  <Snackbar
    message={snackbarMessage}
    isSuccess={isSnackbarSuccess}
    {showSnackbar}
  />
</div>

<style>
  table {
    box-shadow: 0px 4px 18px 2.25px rgba(0, 0, 0, 0.18);
  }
  th {
    padding: 8px 12px;
  }
</style>
