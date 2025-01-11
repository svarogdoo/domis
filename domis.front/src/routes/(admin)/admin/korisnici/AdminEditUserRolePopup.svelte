<script lang="ts">
  import { createEventDispatcher } from "svelte";
  import { updateUserRole } from "../../../../services/admin-service";

  export let show = false;
  export let user: AdminUser;

  let selectedRole: string = user?.role;

  const dispatch = createEventDispatcher();

  const closePopup = () => {
    show = false;
  };

  const saveNewUserRole = async () => {
    const res = await updateUserRole(user.userId, selectedRole);
    if (res) dispatch("save", { success: true });
    else dispatch("save", { success: false });

    closePopup();
  };
</script>

<div
  class="fixed inset-0 flex items-center justify-center bg-domis-dark bg-opacity-50 z-50"
>
  <div
    id="admin-edit-user-role-popup"
    class="w-full lg:w-auto flex flex-col gap-y-6 items-center bg-white p-6 rounded-lg shadow-lg text-center"
  >
    <p class="font-semibold text-lg">Promena role korisnika</p>
    <p>{user.userName}</p>
    <select
      bind:value={selectedRole}
      class="block w-32 border rounded-lg px-2 py-2 bg-white text-left font-light"
    >
      <option disabled value={0}>Izaberite novu rolu korisnika</option>
      <option value="User">User</option>
      <option value="VP1">VP1</option>
      <option value="VP2">VP2</option>
      <option value="VP3">VP3</option>
      <option value="VP4">VP4</option>
      <option value="Admin">Admin</option>
    </select>

    <div class="flex flex-wrap gap-y-2 gap-x-2 justify-center">
      <button
        class="text-light bg-green-600 text-white py-2 px-12 rounded-lg text-center tracking-widest hover:bg-green-700"
        on:click={saveNewUserRole}
      >
        Sačuvaj
      </button>
      <button
        class="text-light bg-domis-dark text-white py-2 px-12 rounded-lg text-center tracking-widest hover:bg-gray-600"
        on:click={closePopup}
        >Zatvori
      </button>
    </div>
  </div>
</div>
