<script>
  let password = "";
  let confirmPassword = "";
  let resetToken = ""; // This would usually be extracted from the URL query params

  const handlePasswordChange = () => {
    if (password !== confirmPassword) {
      alert("Passwords do not match!");
      return;
    }

    // Add your logic to handle the password change, including the resetToken
    console.log("Changing password with token:", resetToken);
    console.log("New password:", password);

    // You can call your password reset service here
  };
</script>

<div class="flex w-full h-full items-center justify-center p-6">
  <div class="bg-white p-8 rounded-xl border w-full max-w-md">
    <h2 class="text-2xl font-bold mb-6 text-center">Promena Šifre</h2>

    <p class="text-gray-600 text-sm mb-6 text-center">
      Unesite novu šifru za Vaš nalog.
    </p>

    <form on:submit|preventDefault={handlePasswordChange}>
      <!-- New Password -->
      <div class="mb-4">
        <label for="password" class="block font-medium text-gray-700 mb-2"
          >New Password</label
        >
        <input
          id="password"
          type="password"
          class="w-full p-2 border rounded"
          bind:value={password}
          placeholder="Enter your new password"
          required
        />
      </div>

      <!-- Confirm New Password -->
      <div class="mb-4">
        <label
          for="confirmPassword"
          class="block font-medium text-gray-700 mb-2"
          >Confirm New Password</label
        >
        <input
          id="confirmPassword"
          type="password"
          class="w-full p-2 border rounded"
          bind:value={confirmPassword}
          placeholder="Confirm your new password"
          required
        />
      </div>

      <!-- Submit Button -->
      <div class="mt-6">
        <button
          type="submit"
          class="w-full bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700"
        >
          Change Password
        </button>
      </div>
    </form>
  </div>
</div>
