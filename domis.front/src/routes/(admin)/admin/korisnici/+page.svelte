<script lang="ts">
  import { onMount } from "svelte";
  import { getAdminUsers } from "../../../../services/admin-service";
  import AdminUserItem from "./AdminUserItem.svelte";
  import AdminUserItemMobile from "./AdminUserItemMobile.svelte";
  import { snackbarStore } from "../../../../stores/snackbar";

  let users: Array<AdminUser>;

  onMount(() => {
    setAdminUsers();
  });

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
      snackbarStore.showSnackbar("Uspešno sačuvana rola korisnika!", true);
      await setAdminUsers();
    } else {
      snackbarStore.showSnackbar("Greška pri čuvanju role korisnika!", false);
    }
  }
</script>

<div class="flex flex-col w-full items-center lg:px-12">
  {#if users?.length && users.length > 0}
    <table class="hidden w-full lg:table">
      <thead class=" bg-domis-dark text-white">
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
    <table class="table lg:hidden table-hover">
      <thead class=" bg-domis-dark text-white">
        <th class="text-start">Korisnici</th>
      </thead>
      <tbody class="divide-y divide-gray-200">
        {#each users as user (user.userId)}
          <AdminUserItemMobile bind:user on:save={handleSave} />
        {/each}
      </tbody>
    </table>
  {/if}
</div>

<style>
  table {
    box-shadow: 0px 4px 18px 2.25px rgba(0, 0, 0, 0.18);
  }
  th {
    padding: 8px 12px;
  }
</style>
