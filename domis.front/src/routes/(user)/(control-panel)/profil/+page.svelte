<script lang="ts">
  import { userStore } from "../../../../stores/user";
  import InputString from "../../../../components/InputString.svelte";

  let isUserDataPopulated = false;
  let isSameUser = true;

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

  let userRole: string | undefined;

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

<section class="flex flex-col gap-y-12">
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

      <div class="flex flex-col gap-y-2 mt-4">
        <p class="text-lg font-light">Podaci adrese</p>
        <div
          class="flex flex-col gap-y-4 p-4 border border-gray-300 rounded-lg"
        >
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
              width={"64"}
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
              width={"64"}
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
        </div>
      </div>

      <button
        on:click={() => (isCompany = !isCompany)}
        class="relative flex mt-4 gap-x-2 items-center"
      >
        <input
          type="checkbox"
          class="appearance-none h-5 w-5 cursor-pointer rounded-md border border-green-700 checked:bg-green-700"
          checked={isCompany}
        />
        <span class="absolute text-white left-0.5">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            class="h-3.5 w-3.5"
            viewBox="0 0 20 20"
            fill="currentColor"
            stroke="currentColor"
            stroke-width="1"
          >
            <path
              fill-rule="evenodd"
              d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
              clip-rule="evenodd"
            ></path>
          </svg>
        </span>
        <p class="text-green-700 pt-1 text-sm font-extralight tracking-wider">
          Postavi podatke za pravna lica
        </p>
      </button>
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
            <button
              on:click={() => (isSameUser = !isSameUser)}
              class="relative flex mt-4 gap-x-2 items-center"
            >
              <input
                type="checkbox"
                class="appearance-none h-5 w-5 cursor-pointer rounded-md border border-green-700 checked:bg-green-700"
                checked={isSameUser}
              />
              <span class="absolute text-white left-0.5">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-3.5 w-3.5"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                  stroke="currentColor"
                  stroke-width="1"
                >
                  <path
                    fill-rule="evenodd"
                    d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
                    clip-rule="evenodd"
                  ></path>
                </svg>
              </span>
              <p
                class="text-green-700 pt-1 text-sm font-extralight tracking-wider"
              >
                Podaci korisnika su podaci poručioca
              </p>
            </button>
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
