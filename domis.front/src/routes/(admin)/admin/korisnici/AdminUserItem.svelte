<script lang="ts">
  import { createEventDispatcher } from "svelte";
  import AdminEditUserRolePopup from "./AdminEditUserRolePopup.svelte";
  import { getUserRoleColor } from "../../../../constants";

  export let user: AdminUser;

  let showEditUserRole = false;

  const dispatch = createEventDispatcher();

  const handleSave = (event: any) => {
    dispatch("save", event?.detail);
  };
</script>

<tr
  class="group hover:bg-slate-100 cursor-pointer"
  on:click={() => {
    showEditUserRole = true;
  }}
>
  <td>{user.firstName ?? ""}</td>
  <td>{user.lastName ?? ""}</td>
  <td>{user.userName ?? ""}</td>
  <td>
    <div class="flex w-full justify-between items-center">
      {#if user.role}
        <p
          class="py-2 w-20 text-center rounded-full text-md
          {getUserRoleColor(user.role)}"
        >
          {user.role}
        </p>
      {/if}
    </div>
  </td>
</tr>
{#if showEditUserRole}
  <AdminEditUserRolePopup
    bind:show={showEditUserRole}
    {user}
    on:save={handleSave}
  />
{/if}

<style>
  td {
    padding: 8px 12px;
  }
</style>
