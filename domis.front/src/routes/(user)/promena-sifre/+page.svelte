 <script lang="ts">
  import { userStore } from "../../../stores/user";
  import { page } from '$app/stores'

  let resetCode: string = $page.url.searchParams.get('code') || "";
  let email: string = $page.url.searchParams.get('email') || "";

  let password = "";
  let confirmPassword = "";

  let errorMessage = "";

  const handlePasswordChange = async () => {
    if (password !== confirmPassword) {
      errorMessage = "Lozinke se ne poklapaju.";
      return;
    }

    try {
      await userStore.resetPassword(email, resetCode, password);
    }
    catch (error: any) {
      console.error("Password change failed:", error.message);
      errorMessage = 'Došlo je do greške. Pokušajte ponovo.';
    }
  };
</script>

<div class="flex w-full h-full items-center justify-center p-6">
  <div class="bg-white p-8 rounded-xl border w-full max-w-md">
    <h2 class="text-2xl font-bold mb-6 text-center">Promena lozinke</h2>

    <p class="text-gray-600 text-sm mb-6 text-center">
      Unesite novu lozinku
    </p>

    {#if errorMessage}
    <p class="text-red-500 text-sm mb-4 text-center">{errorMessage}</p>
    {/if}

    <form on:submit|preventDefault={handlePasswordChange}>
      <!-- New Password -->
      <div class="mb-4">
        <label for="password" class="block font-medium text-gray-700 mb-2"
          >Nova lozinka</label
        >
        <input
          id="password"
          type="password"
          class="w-full p-2 border rounded"
          bind:value={password}
          placeholder="(uneti novu lozinku)"
          required
        />
      </div>

      <!-- Confirm New Password -->
      <div class="mb-4">
        <label
          for="confirmPassword"
          class="block font-medium text-gray-700 mb-2"
          >Potvrdite novu lozinku</label
        >
        <input
          id="confirmPassword"
          type="password"
          class="w-full p-2 border rounded"
          bind:value={confirmPassword}
          placeholder="(potvrditi novu lozinku)"
          required
        />
      </div>

      <!-- Submit Button -->
      <div class="mt-6">
        <button
          type="submit"
          class="w-full bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700"
        >
          Promeni
        </button>
      </div>
    </form>
  </div>
</div>
