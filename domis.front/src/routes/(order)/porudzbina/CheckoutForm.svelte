<script lang="ts">
  import { onMount } from "svelte";
  import Input from "../../(admin)/admin/products/Input.svelte";
  import InputString from "../../../components/InputString.svelte";

  let name = "";
  let lastName = "";
  let companyName = "";
  const country = "Srbija"; // Readonly field
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
      { value: "belgrade", name: "Beograd" },
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
  <div class="flex flex-col gap-y-6">
    <h2
      class="text-xl tracking-wide border-b border-gray-400 pb-2 border-b-0.5"
    >
      Detalji za naplatu
    </h2>
    <div class="flex gap-x-12">
      <!-- Name -->
      <InputString
        bind:value={name}
        title="Ime"
        placeholder="Petar"
        error={errors?.name}
        isRequired={true}
        width={"64"}
      />

      <!-- Last Name -->
      <InputString
        bind:value={lastName}
        title="Prezime"
        placeholder="Petrović"
        error={errors?.lastName}
        isRequired={true}
        width={"64"}
      />
    </div>

    <div class="flex gap-x-12">
      <!-- Company Name (Optional) -->
      <InputString
        bind:value={companyName}
        title="Naziv kompanije (Opciono)"
        placeholder="Kompanija d.o.o"
        isRequired={false}
        width={"64"}
      />

      <!-- Country (Readonly) -->
      <div class="flex flex-col gap-y-2">
        <label for="country">Zemlja</label>
        <input
          id="country"
          type="text"
          class="block w-32 rounded-md border-0 py-1.5 pl-3 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-blue-600 text-md leading-6"
          value={country}
          readonly
          disabled
        />
      </div>
    </div>

    <!-- Street and Number -->
    <InputString
      bind:value={streetAndNumber}
      title="Ulica i broj"
      placeholder="Kneza Miloša 23"
      error={errors?.streetAndNumber}
      isRequired={true}
      width={"96"}
    />

    <!-- Municipality (Dropdown) -->
    <div class="flex flex-col gap-y-2">
      <label for="municipality">Okrug (Opciono)</label>
      <select
        id="municipality"
        bind:value={municipality}
        class="flex w-80 px-4 py-2 rounded-lg bg-white border border-gray-300"
      >
        <option value="" disabled selected>Odaberite okrug</option>
        {#each municipalityOptions as option}
          <option
            class="bg-white px-4 py-2 hover:bg-gray-100"
            value={option.value}>{option.name}</option
          >
        {/each}
      </select>
    </div>

    <div class="flex gap-x-12">
      <!-- Zip Code -->
      <InputString
        bind:value={zipCode}
        title="Poštanski broj"
        placeholder="11000"
        error={errors?.zipCode}
        isRequired={true}
        width={"32"}
      />

      <!-- Email -->
      <InputString
        bind:value={email}
        title="Email"
        placeholder="kompanija@gmail.com"
        error={errors?.email}
        isRequired={true}
        width={"80"}
      />
    </div>

    <!-- Special Notes (Optional) -->
    <div>
      <label for="special-notes">Napomene o narudžbini (opciono)</label>
      <textarea
        id="special-notes"
        class="block w-full rounded-md border-0 py-1.5 pl-3 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-blue-600 text-md leading-6"
        bind:value={specialNotes}
        placeholder="Posebne napomene o narudžbini ili isporuci."
      ></textarea>
    </div>

    <!-- Submit Button -->
    <button
      type="submit"
      class="w-full bg-blue-600 text-white py-2 px-4 rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
    >
      Potvrdi
    </button>
  </div>
</form>
