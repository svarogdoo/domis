<script lang="ts">
  import { userStore } from "../../../stores/user";

  const handleLogout = () => {
    userStore.logoutUser();
  };
</script>

<ul class="flex flex-col w-full text-center text-lg">
  <a
    href="/admin/proizvodi"
    class="w-full py-3 flex items-center justify-center hover:bg-gray-100 border-b border-t border-gray-300"
  >
    Proizvodi
  </a>
  <!-- <a
    href="/admin/kategorije"
    class="w-full py-3 flex items-center justify-center hover:bg-gray-100 border-b border-gray-300"
  >
    Kategorije
  </a> -->
  <a
    href="/admin/korisnici"
    class="w-full py-3 flex items-center justify-center hover:bg-gray-100 border-b border-gray-300"
  >
    Korisnici
  </a>

  <!-- <a
    href="/admin/porudzbine"
    class="w-full py-3 flex items-center justify-center hover:bg-gray-100 border-b border-gray-300"
  >
    Porudžbine
  </a> -->

  <li
    class="w-full py-3 flex items-center justify-center hover:bg-gray-100 border-b border-gray-300"
  >
    <button on:click={handleLogout}> Izloguj se </button>
  </li>
</ul>
