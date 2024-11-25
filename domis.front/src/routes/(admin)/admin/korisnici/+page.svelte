<script lang="ts">
  import { onMount } from "svelte";
  import { getAdminUsers } from "../../../../services/admin-service";
  import AdminUserItem from "./AdminUserItem.svelte";

  let users: Array<AdminUser>;

  onMount(() => {
    setAdminUsers();
  });

  async function setAdminUsers() {
    users = await getAdminUsers();
    console.info(users);
  }
</script>

<div class="flex flex-col px-12">
  {#if users?.length && users.length > 0}
    <table class="table table-hover">
      <thead class="w-full bg-domis-dark text-white">
        <th class="text-start w-40">Ime</th>
        <th class="text-start w-40">Prezime</th>
        <th class="text-start w-40">Email</th>
        <th class="text-start w-40">Tip</th>
      </thead>
      <tbody class="divide-y divide-gray-200">
        {#each users as user (user.userId)}
          <AdminUserItem bind:user />
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
