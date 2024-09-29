<script>
  import { onMount } from "svelte";
  import userIcon from "$lib/icons/user.svg";

  let loggedIn = false;
  let initials = "";

  onMount(() => {
    const user = { isLoggedIn: true, name: "John Doe" };

    if (user.isLoggedIn) {
      loggedIn = false;
      initials = user.name
        .split(" ")
        .map((n) => n[0])
        .join("");
    } else {
      loggedIn = false;
    }
  });

  const handleLogin = () => {};

  const handleRegister = () => {};

  const handleLogout = () => {
    loggedIn = false;
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
        <img src={userIcon} alt="User Icon" class="w-10 h-10 rounded-full" />
      </div>
    {/if}

    <div
      class="z-10 absolute right-0 w-48 bg-white text-black rounded-lg shadow-lg p-2 hidden group-hover:block group-hover:pointer-events-auto"
    >
      {#if loggedIn}
        <ul>
          <li>
            <a
              href="/logout"
              on:click={handleLogout}
              class="block px-4 py-2 hover:bg-gray-100">Logout</a
            >
          </li>
        </ul>
      {:else}
        <ul>
          <li>
            <a
              href="/login"
              on:click={handleLogin}
              class="block px-4 py-2 hover:bg-gray-100">Login</a
            >
          </li>
          <li>
            <a
              href="/register"
              on:click={handleRegister}
              class="block px-4 py-2 hover:bg-gray-100">Register</a
            >
          </li>
        </ul>
      {/if}
    </div>
  </div>
</div>
