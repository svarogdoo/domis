<script>
  let isCompany = false;

  let formData = {
    username: "",
    email: "",
    password: "",
    confirmPassword: "",
    companyName: "",
    companyAddress: "",
    taxId: "",
  };

  const handleSubmit = () => {
    if (isCompany) {
      // Handle company registration logic
      console.log("Registering as a company:", formData);
    } else {
      // Handle regular user registration logic
      console.log("Registering as a regular user:", formData);
    }
  };
</script>

<div class="flex items-center justify-center p-6">
  <div class="bg-white p-8 border rounded-xl w-full max-w-md">
    <h2 class="text-2xl font-bold mb-6 text-center">Registracija</h2>

    <!-- User Type Selection -->
    <div class="mb-6">
      <label for="userType" class="block font-medium text-gray-700 mb-2">
        Tip korisnika
      </label>
      <div id="userType" class="flex items-center">
        <input
          type="radio"
          id="regularUser"
          name="userType"
          value="user"
          class="mr-2"
          checked={!isCompany}
          on:change={() => (isCompany = false)}
        />
        <label for="regularUser" class="mr-4 cursor-pointer">
          Fizičko lice
        </label>

        <input
          type="radio"
          id="company"
          name="userType"
          value="company"
          class="mr-2"
          checked={isCompany}
          on:change={() => (isCompany = true)}
        />
        <label for="company" class="cursor-pointer">Pravno lice</label>
      </div>
    </div>

    <form on:submit|preventDefault={handleSubmit}>
      <div class="mb-4">
        <label for="username" class="block font-medium text-gray-700 mb-2"
          >Ime</label
        >
        <input
          id="username"
          type="text"
          class="w-full p-2 border rounded"
          bind:value={formData.username}
          placeholder="Enter your username"
          required
        />
      </div>

      <!-- Email -->
      <div class="mb-4">
        <label for="email" class="block font-medium text-gray-700 mb-2"
          >Email</label
        >
        <input
          id="email"
          type="email"
          class="w-full p-2 border rounded"
          bind:value={formData.email}
          placeholder="Enter your email"
          required
        />
      </div>

      <!-- Password -->
      <div class="mb-4">
        <label for="password" class="block font-medium text-gray-700 mb-2"
          >Password</label
        >
        <input
          id="password"
          type="password"
          class="w-full p-2 border rounded"
          bind:value={formData.password}
          placeholder="Enter your password"
          required
        />
      </div>

      <!-- Confirm Password -->
      <div class="mb-4">
        <label
          for="confirmPassword"
          class="block font-medium text-gray-700 mb-2">Confirm Password</label
        >
        <input
          id="confirmPassword"
          type="password"
          class="w-full p-2 border rounded"
          bind:value={formData.confirmPassword}
          placeholder="Confirm your password"
          required
        />
      </div>

      <!-- Company-Specific Fields (shown only if 'Company' is selected) -->
      {#if isCompany}
        <div class="mb-4">
          <label for="companyName" class="block font-medium text-gray-700 mb-2">
            Ime firme
          </label>
          <input
            id="companyName"
            type="text"
            class="w-full p-2 border rounded"
            bind:value={formData.companyName}
            placeholder="Enter company name"
            required
          />
        </div>

        <div class="mb-4">
          <label
            for="companyAddress"
            class="block font-medium text-gray-700 mb-2">Adresa firme</label
          >
          <input
            id="companyAddress"
            type="text"
            class="w-full p-2 border rounded"
            bind:value={formData.companyAddress}
            placeholder="Enter company address"
            required
          />
        </div>

        <div class="mb-4">
          <label for="taxId" class="block font-medium text-gray-700 mb-2">
            PIB
          </label>
          <input
            id="taxId"
            type="text"
            class="w-full p-2 border rounded"
            bind:value={formData.taxId}
            placeholder="Enter tax ID"
            required
          />
        </div>
      {/if}

      <!-- Submit Button -->
      <div class="mt-6">
        <button
          type="submit"
          class="w-full bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700"
        >
          Registruj se
        </button>
      </div>
    </form>
  </div>
</div>
