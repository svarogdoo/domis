<script>
  let email = "";

  const handlePasswordReset = () => {
    // Add your logic to handle password reset (e.g., API call)
    console.log("Requesting password reset for:", email);
    // You can call your password reset service here
  };
</script>

<div class="flex h-full w-full items-center justify-center p-6">
  <div class="bg-white p-8 border rounded-xl w-full max-w-md">
    <h2 class="text-2xl font-bold mb-6 text-center">Zaboravljena šifra</h2>
    <p class="text-gray-600 text-sm mb-6 text-center">
      Unesite svoj email, mi ćemo vam poslati uputstva za promenu šifre.
    </p>

    <form on:submit|preventDefault={handlePasswordReset}>
      <!-- Email Input -->
      <div class="mb-4">
        <label for="email" class="block font-medium text-gray-700 mb-2"
          >Email Address</label
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
          Reset Password
        </button>
      </div>
    </form>
  </div>
</div>
