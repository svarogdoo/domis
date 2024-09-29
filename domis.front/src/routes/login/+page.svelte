<!-- src/routes/Login.svelte -->
<script lang="ts">
  //   import { userStore } from "../stores/userStore";

  let userName = "";
  let password = "";
  let errorMessage = "";

  const handleLogin = async () => {
    //     errorMessage = ""; // Reset the error message
    //     try {
    //       const response = await fetch("https://example.com/api/login", {
    //         method: "POST",
    //         headers: { "Content-Type": "application/json" },
    //         body: JSON.stringify({ userName, password }),
    //       });
    //       if (response.ok) {
    //         const userData = await response.json();
    //         userStore.loginUser(userData);
    //       } else {
    //         errorMessage = "Invalid username or password";
    //       }
    //     } catch (error) {
    //       errorMessage = "An error occurred during login";
    //       console.error(error);
    //     }
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
          >Username</label
        >
        <input
          type="text"
          id="username"
          bind:value={userName}
          required
          class="mt-1 p-2 border border-gray-300 rounded-md w-full focus:ring-blue-500 focus:border-blue-500"
          placeholder="Enter your username"
        />
      </div>

      <div class="flex flex-col">
        <label for="password" class="block text-sm font-medium text-gray-700"
          >Password</label
        >
        <input
          type="password"
          id="password"
          bind:value={password}
          required
          class="mt-1 p-2 border border-gray-300 rounded-md w-full focus:ring-blue-500 focus:border-blue-500"
          placeholder="Enter your password"
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
      Don't have an account?
      <a href="/register" class="text-blue-600 hover:underline">Register here</a
      >
    </p>
  </div>
</div>
