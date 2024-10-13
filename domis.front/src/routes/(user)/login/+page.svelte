<!-- src/routes/Login.svelte -->
<script lang="ts">
  import { goto } from "$app/navigation";
  import { userStore } from "../../../stores/user";
  let userName = "";
  let password = "";
  let errorMessage = "";

  const handleLogin = async () => {
    errorMessage = "";

    try {
      const loginResponse = await userStore.loginUser(userName, password);
      goto("/");
    } catch (error: any) {
      if (error.status === 401) {
        errorMessage = "Korisničko ime i/ili lozinka su neispravni.";
      } else {
        errorMessage =
          error.message || "Greška u obradi zahteva, pokušajte ponovo.";
      }
    }
  };
</script>

<div class="flex justify-center items-center h-full w-full">
  <div class="flex flex-col p-12 border rounded-xl">
    <h2 class="text-2xl font-bold mb-4 text-center">Login</h2>

    {#if errorMessage}
      <p class="text-red-500 text-sm mb-4">{errorMessage}</p>
    {/if}

    <form class="flex flex-col gap-y-4" on:submit|preventDefault={handleLogin}>
      <div class="flex flex-col">
        <label for="username" class="block text-sm font-medium text-gray-700"
          >Email</label
        >
        <input
          type="text"
          id="username"
          bind:value={userName}
          required
          class="mt-1 p-2 border border-gray-300 rounded-md w-full focus:ring-blue-500 focus:border-blue-500 placeholder:font-light text-sm"
          placeholder="korisnicko-ime@email.com"
        />
      </div>

      <div class="flex flex-col">
        <label for="password" class="block text-sm font-medium text-gray-700"
          >Lozinka</label
        >
        <input
          type="password"
          id="password"
          bind:value={password}
          required
          class="mt-1 p-2 border border-gray-300 rounded-md w-full focus:ring-blue-500 focus:border-blue-500 placeholder:font-light text-sm"
          placeholder="Unesite lozinku"
        />
      </div>

      <button
        type="submit"
        class="w-full py-2 px-4 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition duration-200"
      >
        Login
      </button>
    </form>

    <p class="mt-4 text-sm text-center">
      Nemate nalog?
      <a href="/register" class="text-blue-600 hover:underline"
        >Registrujte se ovde</a
      >
    </p>
    <p class="mt-2 text-sm text-center">
      Zaboravili ste šifru?
      <a href="/zahtev-promena-sifre" class="text-blue-600 hover:underline">
        Promenite je ovde
      </a>
    </p>
  </div>
</div>
