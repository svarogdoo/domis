<script lang="ts">
  import { userStore } from "../../../stores/user";

  const handleLogout = () => {
    userStore.logoutUser();
  };
</script>

<ul class="flex flex-col w-full text-center text-lg">
  <a
    href="/profil"
    class="w-full py-3 flex items-center justify-center hover:bg-gray-100 border-b border-t border-gray-600"
  >
    Profil
  </a>
  <a
    href="/porudzbine"
    class="w-full py-3 flex items-center justify-center hover:bg-gray-100 border-b border-gray-600"
  >
    Porudžbine
  </a>
  <li
    class="w-full py-3 flex items-center justify-center hover:bg-gray-100 border-b border-gray-600"
  >
    <button on:click={handleLogout}> Izloguj se </button>
  </li>
</ul>
