<script lang="ts">
  import { goto } from "$app/navigation";
  import { userStore } from "../../../stores/user";

  let isCompany = false;
  let errorMessage = "";
  let formData = {
    email: "",
    firstName: "",
    lastName: "",
    password: "",
    confirmPassword: "",
    companyName: "",
    companyAddress: "",
    taxId: "",
  };

  const handleSubmit = async () => {
    if (formData.password !== formData.confirmPassword) {
      errorMessage = "Lozinke se ne poklapaju."
      return;
    }

    try {
      if (isCompany) {
        //TODO:
      } else {
          const request = {
              firstName: formData.firstName,
              lastName: formData.lastName,
              email: formData.email,
              password: formData.password,
          };

          await userStore.registerUser(request);
          goto('/');
      }
    } catch (error: any) {
        console.error("Registration failed:", error.message);

        if (error.errors && error.errors.DuplicateUserName) {
            errorMessage = "Uneta imejl adresa već postoji."
        } else if (error.errors && Object.keys(error.errors).length > 0) {
            // If there are other validation errors, grab the first one
            const firstErrorKey = Object.keys(error.errors)[0];
            errorMessage = error.errors[firstErrorKey][0];
        } else {
            errorMessage = 'Došlo je do greške. Pokušajte ponovo.'; // General error message
        }
    }
  };
</script>

<div class="flex items-center justify-center p-6">
  <div class="bg-white p-8 border rounded-xl w-full max-w-md">
    <h2 class="text-2xl font-bold mb-6 text-center">Registracija</h2>

    <!-- Error Message Display -->
    {#if errorMessage}
    <p class="text-red-500 text-sm mb-4 text-center">{errorMessage}</p>
    {/if}

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
        <label for="firstName" class="block font-medium text-gray-700 mb-2"
          >Ime</label
        >
        <input
          id="firstName"
          type="text"
          class="w-full p-2 border rounded"
          bind:value={formData.firstName}
          placeholder="(uneti ime)"
          required
        />
      </div>

      <div class="mb-4">
        <label for="lastName" class="block font-medium text-gray-700 mb-2">
          Prezime
        </label>
        <input
          id="lastName"
          type="text"
          class="w-full p-2 border rounded"
          bind:value={formData.lastName}
          placeholder="(uneti prezime)"
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
          placeholder="(uneti imejl adresu)"
          required
        />
      </div>

      <!-- Password -->
      <div class="mb-4">
        <label for="password" class="block font-medium text-gray-700 mb-2"
          >Lozinka</label
        >
        <input
          id="password"
          type="password"
          class="w-full p-2 border rounded"
          bind:value={formData.password}
          placeholder="(uneti lozinku)"
          required
        />
      </div>

      <!-- Confirm Password -->
      <div class="mb-4">
        <label
          for="confirmPassword"
          class="block font-medium text-gray-700 mb-2">Ponovljena lozinka</label
        >
        <input
          id="confirmPassword"
          type="password"
          class="w-full p-2 border rounded"
          bind:value={formData.confirmPassword}
          placeholder="(ponoviti lozinku)"
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
