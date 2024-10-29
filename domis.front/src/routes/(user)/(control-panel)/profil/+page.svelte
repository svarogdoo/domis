<script lang="ts">
  import { userStore } from "../../../../stores/user";
  import { onMount } from "svelte";
  import { requireAuth } from "../../../../utils/AuthGuard";
  import InputString from "../../../../components/InputString.svelte";
  import { userService } from "../../../../services/user-service";

  let isUserDataPopulated = false;

  let firstName = "";
  let lastName = "";
  let addressLine = "";
  let apartment = "";
  let city = "";
  let postalCode = "";
  let country = "";
  let county = "";
  let email = "";
  let phoneNumber = "";
  let companyName = "";

  $: if ($userStore.user && !isUserDataPopulated) {
    firstName = $userStore.user.firstName ?? "";
    lastName = $userStore.user.lastName ?? "";
    addressLine = $userStore.user.addressLine ?? "";
    apartment = $userStore.user.apartment ?? "";
    city = $userStore.user.city ?? "";
    postalCode = $userStore.user.postalCode ?? "";
    country = $userStore.user.country ?? "";
    county = $userStore.user.county ?? "";
    email = $userStore.user.email ?? "";
    phoneNumber = $userStore.user.phoneNumber ?? "";
    companyName =
      ($userStore.user as UserWholesaleProfileDto).companyName ?? "";

    isUserDataPopulated = true;
  }

  async function updateProfile() {
    const request = {
      firstName: firstName,
      lastName: lastName,
      addressLine: addressLine,
      apartment: apartment,
      city: city,
      postalCode: postalCode,
      country: country,
      county: county,
      phoneNumber: phoneNumber,
      ...(companyName && { companyName: companyName }), // Add companyName only if not empty
    };

    await userStore.updateProfile(request);
  }
</script>

<section class="flex flex-col gap-y-12">
  <h1 class="text-2xl">Korisnički profil</h1>
  <div class="flex flex-col gap-y-4">
    {#if isUserDataPopulated}
      <div class="flex gap-x-4">
        <strong>Email:</strong>
        <div>{email}</div>
      </div>
      <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
        <InputString
          bind:value={firstName}
          title="Ime"
          placeholder="Petar"
          width={"48"}
        />
        <InputString
          bind:value={lastName}
          title="Prezime"
          placeholder="Petrović"
          width={"48"}
        />
      </div>
      <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
        <InputString
          bind:value={country}
          title="Država"
          placeholder="Srbija"
          width={"32"}
        />
        <InputString
          bind:value={city}
          title="Grad"
          placeholder="Novi Sad"
          width={"64"}
        />
      </div>
      <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
        <InputString
          bind:value={addressLine}
          title="Ulica i broj"
          placeholder="Kneza Miloša 23"
          width={"80"}
        />
        <InputString
          bind:value={apartment}
          title="Stan"
          placeholder="Apt 12B"
          width={"32"}
        />
      </div>
      <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
        <InputString
          bind:value={postalCode}
          title="Poštanski broj"
          placeholder="11000"
          width={"32"}
        />
        <InputString
          bind:value={county}
          title="Opština"
          placeholder="Novi Sad"
          width={"48"}
        />
      </div>
      <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
        <InputString
          bind:value={phoneNumber}
          title="Broj telefona"
          placeholder="60224455221"
          width={"48"}
          prefix="+381"
        />
      </div>
      <!-- TODO: Special Role type field -->
      {#if companyName}
        <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
          <InputString
            bind:value={companyName}
            title="Naziv firme"
            placeholder="Ime firme"
            width={"80"}
          />
        </div>
      {/if}
      <button
        on:click={updateProfile}
        class="mt-4 text-light bg-domis-dark text-white py-2 px-4 rounded-lg hover:bg-gray-600"
      >
        Sačuvaj promene
      </button>
    {:else}
      <p>učitavanje...</p>
    {/if}
  </div>
</section>
