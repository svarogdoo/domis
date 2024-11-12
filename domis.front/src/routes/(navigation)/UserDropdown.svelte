<script lang="ts">
  import { onMount } from "svelte";
  import userIcon from "$lib/icons/user.svg";
  import { userStore } from "../../stores/user";
  import UserDropdownItem from "./UserDropdownItem.svelte";

  let loggedIn = false;
  let initials = "";
  let user: UserState;
  let isUserAdmin = false;

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
    userStore.logoutUser();
  };
</script>

<div class="relative flex items-center justify-end p-4 text-white">
  <div class="relative group">
    {#if loggedIn}
      <div
        class="cursor-pointer text-sm lg:text-base flex items-center justify-center w-8 h-8 lg:w-10 lg:h-10 border-2 border-gray-500 rounded-full text-domis-dark font-bold"
      >
        {initials}
      </div>
    {:else}
      <div class="cursor-pointer">
        <img
          src={userIcon}
          alt="User Icon"
          class="w-6 h-6 lg:w-12 lg:h-12 rounded-full"
        />
      </div>
    {/if}

    <div
      class="z-10 absolute right-0 w-48 bg-white text-domis-dark rounded-lg shadow-lg p-2 hidden group-hover:block group-hover:pointer-events-auto before:content-[''] before:absolute before:top-0 before:right-0 before:-translate-y-full before:w-full before:h-10"
    >
      {#if loggedIn}
        <ul>
          {#if userStore.isUserAdmin()}
            <UserDropdownItem href="/admin/proizvodi" text="Proizvodi" />
            <UserDropdownItem href="/admin/korisnici" text="Korisnici" />
            <UserDropdownItem href="/admin/porudzbine" text="Porudžbine" />
          {:else}
            <UserDropdownItem href="/profil" text="Profil" />
            <UserDropdownItem href="/porudzbine" text="Porudžbine" />
          {/if}
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
