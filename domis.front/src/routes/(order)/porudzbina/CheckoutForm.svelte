<script lang="ts">
  import { onMount } from "svelte";

  let name = "";
  let lastName = "";
  let companyName = "";
  let country = "Serbia"; // Readonly field
  let streetAndNumber = "";
  let municipality = "";
  let zipCode = "";
  let email = "";
  let specialNotes = "";

  let municipalityOptions: { value: string; name: string }[] = [];

  // Form errors
  let errors = {
    name: "",
    lastName: "",
    streetAndNumber: "",
    zipCode: "",
    email: "",
    municipality: "",
  };

  // Load municipalities from backend
  onMount(async () => {
    // Simulate a backend call to fetch municipality options
    municipalityOptions = await fetchMunicipalities();
  });

  async function fetchMunicipalities() {
    // Replace this with a real API call
    return [
      { value: "belgrade", name: "Belgrade" },
      { value: "novisad", name: "Novi Sad" },
      { value: "nis", name: "Niš" },
    ];
  }

  function validateForm() {
    let valid = true;

    // Reset errors
    errors = {
      name: "",
      lastName: "",
      streetAndNumber: "",
      zipCode: "",
      email: "",
      municipality: "",
    };

    // Name validation
    if (!name.trim()) {
      errors.name = "Ime je obavezno polje";
      valid = false;
    }

    // Last name validation
    if (!lastName.trim()) {
      errors.lastName = "Prezime je obavezno polje";
      valid = false;
    }

    // Street and number validation
    if (!streetAndNumber.trim()) {
      errors.streetAndNumber = "Ulica i broj su neophodni";
      valid = false;
    }

    // Zip code validation
    if (!zipCode.trim()) {
      errors.zipCode = "Poštanski broj je obavezan";
      valid = false;
    }

    // Email validation
    const emailPattern = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/;
    if (!email.match(emailPattern)) {
      errors.email = "Nevalidna vrednost email-a";
      valid = false;
    }

    return valid;
  }

  function submitForm() {
    if (validateForm()) {
      console.log("Form is valid!");
      // Submit the form data
    }
  }
</script>

<form
  class="flex flex-col w-full space-y-4 p-4"
  on:submit|preventDefault={submitForm}
>
  <div class="flex flex-col gap-y-8">
    <div class="flex gap-x-12">
      <!-- Name -->
      <div class="w-full">
        <label for="name" class="block text-sm font-medium text-gray-700"
          >Ime <span class="text-red-500">*</span></label
        >
        <input
          id="name"
          type="text"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
          bind:value={name}
          placeholder="Petar"
        />
        {#if errors.name}
          <p class="text-red-500 text-sm">{errors.name}</p>
        {/if}
      </div>

      <!-- Last Name -->
      <div class="w-full">
        <label for="lastname" class="block text-sm font-medium text-gray-700"
          >Prezime <span class="text-red-500">*</span></label
        >
        <input
          id="lastname"
          type="text"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
          bind:value={lastName}
          placeholder="Petrović"
        />
        {#if errors.lastName}
          <p class="text-red-500 text-sm">{errors.lastName}</p>
        {/if}
      </div>
    </div>

    <!-- Company Name (Optional) -->
    <div>
      <label for="company-name" class="block text-sm font-medium text-gray-700"
        >Naziv kompanije (Opciono)</label
      >
      <input
        id="company-name"
        type="text"
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
        bind:value={companyName}
        placeholder="Kompanija d.o.o"
      />
    </div>

    <!-- Country (Readonly) -->
    <div>
      <label for="country" class="block text-sm font-medium text-gray-700"
        >Country</label
      >
      <input
        id="country"
        type="text"
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
        value={country}
        readonly
      />
    </div>

    <!-- Street and Number -->
    <div>
      <label for="street-number" class="block text-sm font-medium text-gray-700"
        >Street and Number <span class="text-red-500">*</span></label
      >
      <input
        id="street-number"
        type="text"
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
        bind:value={streetAndNumber}
        placeholder="Enter your street and number"
      />
      {#if errors.streetAndNumber}
        <p class="text-red-500 text-sm">{errors.streetAndNumber}</p>
      {/if}
    </div>

    <!-- Municipality (Dropdown) -->
    <div>
      <label for="municipality" class="block text-sm font-medium text-gray-700"
        >Municipality (Optional)</label
      >
      <select
        id="municipality"
        bind:value={municipality}
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
      >
        <option value="" disabled selected>Select your municipality</option>
        {#each municipalityOptions as option}
          <option value={option.value}>{option.name}</option>
        {/each}
      </select>
    </div>

    <!-- Zip Code -->
    <div>
      <label for="zip-code" class="block text-sm font-medium text-gray-700"
        >Zip Code <span class="text-red-500">*</span></label
      >
      <input
        id="zip-code"
        type="text"
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
        bind:value={zipCode}
        placeholder="Enter your zip code"
      />
      {#if errors.zipCode}
        <p class="text-red-500 text-sm">{errors.zipCode}</p>
      {/if}
    </div>

    <!-- Email -->
    <div>
      <label for="email" class="block text-sm font-medium text-gray-700"
        >Email <span class="text-red-500">*</span></label
      >
      <input
        id="email"
        type="email"
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
        bind:value={email}
        placeholder="Enter your email"
      />
      {#if errors.email}
        <p class="text-red-500 text-sm">{errors.email}</p>
      {/if}
    </div>

    <!-- Special Notes (Optional) -->
    <div>
      <label class="block text-sm font-medium text-gray-700" for="special-notes"
        >Napomene o narudžbini (opciono)</label
      >
      <textarea
        id="special-notes"
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
        bind:value={specialNotes}
        placeholder="Posebne napomene o narudžbini ili isporuci."
      ></textarea>
    </div>

    <!-- Submit Button -->
    <button
      type="submit"
      class="w-full bg-indigo-600 text-white py-2 px-4 rounded-md hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
    >
      Submit
    </button>
  </div>
</form>
