<script lang="ts">
  import { userStore } from "../../../../stores/user";
  import { onMount } from "svelte";
  import { requireAuth } from "../../../../utils/AuthGuard";
  import InputString from "../../../../components/InputString.svelte";
  import { userService } from "../../../../services/user-service";

  let user: UserProfileResponse | null = null;
  let updatedFirstName = "";
  let updatedLastName = "";
  let updatedAddressLine = "";
  let updatedApartment = "";
  let updatedCity = "";
  let updatedPostalCode = "";
  let updatedCountry = "";
  let updatedCounty = "";
  let updatedPhoneNumber = "";
  let updatedCompanyName = "";

  const unsubscribe = userStore.subscribe((state) => {
    user = state.user as UserProfileResponse | UserWholesaleProfileDto;

    if (user) {
      updatedFirstName = user.firstName;
      updatedLastName = user.lastName;
      updatedAddressLine = user.addressLine || "";
      updatedApartment = user.apartment || "";
      updatedCity = user.city || "";
      updatedPostalCode = user.postalCode || "";
      updatedCountry = user.country || "";
      updatedCounty = user.county || "";
      updatedPhoneNumber = user.phoneNumber || "";
      updatedCompanyName = (user as UserWholesaleProfileDto).companyName || "";
    }
  });

  onMount(() => {
    requireAuth(); // Ensure the user is authenticated

    const fetchProfile = async () => {
      const profile = await userService.getProfile();
      console.log(profile);
      if (profile) {
        userStore.set({
          user: profile,
          isAuthenticated: true,
          token: localStorage.getItem('token'),
          refreshToken: localStorage.getItem('refreshToken'),
        });
      }
    };

    fetchProfile();

    return () => unsubscribe();
  });

  async function updateProfile() {
    const request = {
      firstName: updatedFirstName,
      lastName: updatedLastName,
      addressLine: updatedAddressLine,
      apartment: updatedApartment,
      city: updatedCity,
      postalCode: updatedPostalCode,
      country: updatedCountry,
      county: updatedCounty,
      phoneNumber: updatedPhoneNumber,
      ...(updatedCompanyName && { companyName: updatedCompanyName }) // Add companyName only if not empty
    };

    await userStore.updateProfile(request);
  }
</script>

<section class="flex flex-col gap-y-12">
  <h1 class="text-2xl">Korisnički profil</h1>
  <div class="flex flex-col gap-y-4">
    {#if user}
      <div class="flex gap-x-4">
        <strong>Email:</strong>
        <div>{user.email}</div>
      </div>
      <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
        <InputString bind:value={updatedFirstName} title="Ime" placeholder="Petar" width={"48"} />
        <InputString bind:value={updatedLastName} title="Prezime" placeholder="Petrović" width={"48"} />
      </div>
      <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
        <InputString bind:value={updatedCountry} title="Država" placeholder="Srbija" width={"32"} />
        <InputString bind:value={updatedCity} title="Grad" placeholder="Novi Sad" width={"64"} />
      </div>
      <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
        <InputString bind:value={updatedAddressLine} title="Ulica i broj" placeholder="Kneza Miloša 23" width={"80"} />
        <InputString bind:value={updatedApartment} title="Stan" placeholder="Apt 12B" width={"32"} />
      </div>
      <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
        <InputString bind:value={updatedPostalCode} title="Poštanski broj" placeholder="11000" width={"32"} />
        <InputString bind:value={updatedPhoneNumber} title="Broj telefona" placeholder="60224455221" width={"48"} prefix="+381" />
      </div>
      <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
        <InputString bind:value={updatedCounty} title="Opština" placeholder="Novi Sad" width={"48"} />
      </div>
      {#if 'companyName' in user}
        <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
          <InputString bind:value={updatedCompanyName} title="Naziv firme" placeholder="Ime firme" width={"80"} />
        </div>
      {/if}
      <button on:click={updateProfile} class="mt-4 text-light bg-black text-white py-2 px-4 rounded-lg hover:bg-gray-600">
        Sačuvaj promene
      </button>
    {:else}
      <p>učitavanje...</p>
    {/if}
  </div>
</section>
