<script lang="ts">
  import { userStore } from "../../../stores/user";

  const handleLogout = () => {
    userStore.logoutUser();
  };
</script>

<ul class="flex flex-col w-full text-center text-lg">
  <a
    href="/profil"
    class="w-full py-3 flex items-center justify-center hover:bg-gray-100 border-b border-t border-gray-300"
  >
    Profil
  </a>
  <a
    href="/porudzbine"
    class="w-full py-3 flex items-center justify-center hover:bg-gray-100 border-b border-gray-300"
  >
    Porudžbine
  </a>
  <a
    href="/adrese"
    class="w-full py-3 flex items-center justify-center hover:bg-gray-100 border-b border-gray-300"
  >
    Adrese
  </a>
  <li
    class="w-full py-3 flex items-center justify-center hover:bg-gray-100 border-b border-gray-300"
  >
    <button on:click={handleLogout}> Izloguj se </button>
  </li>
</ul>
