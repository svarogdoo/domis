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

<tr class="group hover:bg-domis-light cursor-pointer w-full">
  <button
    on:click={() => {
      showEditUserRole = true;
    }}
    class="flex flex-col text-center gap-y-1 py-2 w-full items-center"
  >
    {#if user.firstName || user.lastName}
      <p>{user.firstName ?? ""} {user.lastName ?? ""}</p>
    {/if}
    <p>{user.userName}</p>
    {#if user.role}
      <p class="py-1 w-20 rounded-full text-md {getUserRoleColor(user.role)}">
        {user.role}
      </p>
    {/if}
  </button>
  {#if showEditUserRole}
    <AdminEditUserRolePopup
      bind:show={showEditUserRole}
      {user}
      on:save={handleSave}
    />
  {/if}
</tr>

<style>
  td {
    padding: 8px 12px;
  }
</style>
