<script lang="ts">
  import { userStore } from "../../../../stores/user";
  import InputString from "../../../../components/InputString.svelte";
  import Checkbox from "../../../(navigation)/Checkbox.svelte";

  let isUserDataPopulated = false;
  let isSameUser = true;
  let userRole: string | undefined;

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

  let isCompany = false;
  let companyName = "";
  let companyNumber = "";
  let companyFirstName = "";
  let companyLastName = "";

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
    companyName = $userStore.user.company?.companyName ?? "";
    companyNumber = $userStore.user.company?.companyNumber ?? "";
    companyFirstName = $userStore.user.company?.companyFirstName ?? "";
    companyLastName = $userStore.user.company?.companyLastName ?? "";

    if (userStore.isUserVP()) userRole = $userStore.userRole;

    if ($userStore.user.company) isCompany = true;

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
      ...(companyName && { companyName: companyName }), // Add company data only if not empty
      ...(companyNumber && { companyNumber: companyNumber }),
      ...(companyFirstName && { companyFirstName: companyFirstName }),
      ...(companyLastName && { companyLastName: companyLastName }),
    };

    await userStore.updateProfile(request);
  }
</script>

<section class="flex flex-col gap-y-8">
  <h1 class="text-2xl">Korisnički profil</h1>
  <div class="flex flex-col gap-y-4">
    {#if isUserDataPopulated}
      {#if userRole}
        <p class="mb-4">
          <span class="font-semibold">Tip korisnika:</span>
          {userRole}
        </p>
      {/if}
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

      <Checkbox bind:show={isCompany} title="Postavi podatke za pravna lica" />
      {#if isCompany}
        <div class="flex flex-col gap-y-2 mt-4">
          <p class="text-lg font-light">Podaci za pravna lica</p>
          <div
            class="flex flex-col gap-y-4 p-4 border border-gray-300 rounded-lg"
          >
            <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
              <InputString
                bind:value={companyName}
                title="Podaci kompanije"
                placeholder="Firma d.o.o."
                width={"64"}
              />
              <InputString
                bind:value={companyNumber}
                title="Matični broj"
                placeholder=""
                width={"32"}
              />
            </div>

            <Checkbox
              bind:show={isSameUser}
              title="Korisnik je odgovorno lice"
            />
            <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
              {#if isSameUser}
                <InputString
                  bind:value={firstName}
                  title="Ime"
                  placeholder="Petar"
                  width={"48"}
                  isReadOnly={isSameUser}
                />
                <InputString
                  bind:value={lastName}
                  title="Prezime"
                  placeholder="Petrović"
                  width={"48"}
                  isReadOnly={isSameUser}
                />
              {:else}
                <InputString
                  bind:value={companyFirstName}
                  title="Ime"
                  placeholder="Petar"
                  width={"48"}
                />
                <InputString
                  bind:value={companyLastName}
                  title="Prezime"
                  placeholder="Petrović"
                  width={"48"}
                />
              {/if}
            </div>
          </div>
        </div>
      {/if}

      <button
        on:click={updateProfile}
        class="mt-4 text-light bg-domis-dark text-white py-2 px-4 rounded-lg hover:bg-gray-600"
      >
        Sačuvaj promene
      </button>
    {/if}
  </div>
</section>
