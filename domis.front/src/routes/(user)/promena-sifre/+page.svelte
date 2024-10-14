 <script>
  let password = "";
  let confirmPassword = "";
  let resetToken = ""; // This would usually be extracted from the URL query params

//http://localhost:5173/promena-sifre?email=lukardvn@gmail.com&code=Q2ZESjhLbTd4aEZyWTRSRnR1MERnSVV4RXMrSkFFTlVrVzFVbWduVDVoM01ENFRQc2VEdDBKMTY1TlVxVnhXbXdRYk1pQ2M4QngvMjdmMTdSSG9UYnVaZDhCTDFqV0FZcWgxWmN2WjU1VVAxSThjYm5ONGdaTWRGVXlJTlhSY2Q4UTFQQWtGZjFQU0loZlZEUlFhVXR5ekxXT3loaENLZ1prdnczNldrV1lvT0ZoRU04TUtRNzFVUzkrY21xb1hleElsRTNpTEZyRWlrSzY1QVVITWxwVHFYRFpRWkduMERDSWZNSTYyT1J6T2NOUEpQ

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
    <h2 class="text-2xl font-bold mb-6 text-center">Promena lozinke</h2>

    <p class="text-gray-600 text-sm mb-6 text-center">
      Unesite novu lozinku za Vaš nalog
    </p>

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
