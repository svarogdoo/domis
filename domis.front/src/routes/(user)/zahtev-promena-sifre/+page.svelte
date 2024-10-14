<script lang="ts">
  import { userStore } from "../../../stores/user";

  let email = "";
  let requestSent = false;
  let errorMessage = "";

  const handlePasswordReset = async () => {
    console.log("Requesting password reset for:", email);
    
    try {
      await userStore.forgotPassword(email);
      requestSent = true;
    } catch (error: any) {
      "Greška u obradi zahteva, pokušajte ponovo.";
    }
  };
</script>

<div class="flex h-full w-full items-center justify-center p-6">
  <div class="bg-white p-8 border rounded-xl w-full max-w-md">
    <h2 class="text-2xl font-bold mb-6 text-center">Promena lozinke</h2>
    <p class="text-gray-600 text-sm mb-6 text-center">
      Unesite svoj email, mi ćemo vam poslati uputstva za promenu lozinke.
    </p>

    {#if errorMessage}
      <p class="text-red-500 text-sm mb-4">{errorMessage}</p>
    {/if}

    {#if requestSent}
      <div class="text-green-500 text-center mb-6">
        Zahtev za promenu lozinke je poslat. Proverite svoj email.
      </div>
    {/if}

    <form on:submit|preventDefault={handlePasswordReset}>
      <!-- Email Input -->
      <div class="mb-4">
        <label for="email" class="block font-medium text-gray-700 mb-2"
          >Email</label
        >
        <input
          id="email"
          type="email"
          class="w-full p-2 border rounded"
          bind:value={email}
          placeholder="Enter your email"
          required
        />
      </div>

      <!-- Submit Button -->
      <div class="mt-6">
        <button
          type="submit"
          class="w-full bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700"
        >
          Pošalji
        </button>
      </div>
    </form>
  </div>
</div>
