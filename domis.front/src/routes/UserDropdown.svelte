<script lang="ts">
  import { onMount } from "svelte";
  import userIcon from "$lib/icons/user.svg";
  import { userStore } from "../stores/user";
  import { goto } from "$app/navigation";

  let loggedIn = false;
  let initials = "";
  let user: UserState;

  $: userStore.subscribe((value) => {
    user = value;
    setInitials();
  });

  function setInitials() {
    if (user.isAuthenticated) {
      loggedIn = true;
      initials = `${user.user?.firstName} ${user.user?.lastName}`
        .split(" ")
        .map((n) => n[0])
        .join("");
    } else {
      loggedIn = false;
    }
  }

  onMount(() => {
    setInitials();
  });

  const handleLogin = () => {};

  const handleRegister = () => {};

  const handleLogout = () => {
    // loggedIn = false;
    userStore.logoutUser();
    goto("/");
  };
</script>

<div class="relative flex items-center justify-end p-4 text-white">
  <div class="relative group">
    {#if loggedIn}
      <div
        class="cursor-pointer flex items-center justify-center w-10 h-10 border-2 border-gray-500 rounded-full text-black font-bold"
      >
        {initials}
      </div>
    {:else}
      <div class="cursor-pointer">
        <img src={userIcon} alt="User Icon" class="w-12 h-12 rounded-full" />
      </div>
    {/if}

    <div
      class="z-10 absolute right-0 w-48 bg-white text-black rounded-lg shadow-lg p-2 hidden group-hover:block group-hover:pointer-events-auto before:content-[''] before:absolute before:top-0 before:right-0 before:-translate-y-full before:w-full before:h-10"
    >
      {#if loggedIn}
        <ul>
          <li>
            <a
              href="/profil"
              class="w-full text-left block px-4 py-2 hover:bg-gray-100"
            >
              Profil
            </a>
          </li>
          <li>
            <a
              href="/porudzbine"
              class="w-full text-left block px-4 py-2 hover:bg-gray-100"
            >
              Porudžbine
            </a>
          </li>
          <li class="border-t">
            <button
              on:click={handleLogout}
              class="w-full text-left block px-4 py-2 hover:bg-gray-100"
            >
              Izloguj se
            </button>
          </li>
        </ul>
      {:else}
        <ul>
          <li>
            <a
              href="/login"
              on:click={handleLogin}
              class="w-full text-left block px-4 py-2 hover:bg-gray-100"
              >Uloguj se</a
            >
          </li>
          <li>
            <a
              href="/register"
              on:click={handleRegister}
              class="w-full text-left block px-4 py-2 hover:bg-gray-100"
              >Registracija</a
            >
          </li>
        </ul>
      {/if}
    </div>
  </div>
</div>
