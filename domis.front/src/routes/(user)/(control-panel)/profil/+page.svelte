<script lang="ts">
  import { userStore } from "../../../../stores/user";
  import { onMount } from "svelte";
  import { requireAuth } from "../../../../utils/AuthGuard";
  import InputString from "../../../../components/InputString.svelte";

  let user: any;
  let updatedFirstName = "";
  let updatedLastName = "";
  let updatedAddressLine = "";
  let updatedCity = "";
  let updatedZipCode = "";
  let updatedCountry = "";
  let updatedPhoneNumber = "";

  // Subscribe to userStore to get the user info
  const unsubscribe = userStore.subscribe((state) => {
    user = state.user;
    console.log(user);
    if (user) {
      updatedFirstName = user.firstName || "";
      updatedLastName = user.lastName || "";
      updatedAddressLine = user.addressLine || "";
      updatedCity = user.city || "";
      updatedZipCode = user.zipCode || "";
      updatedCountry = user.country || "";
      updatedPhoneNumber = user.phoneNumber || "";
    }
  });

  //TODO: Mikica da vidi sta se ovde desava
  onMount(() => {
    requireAuth();
    return () => unsubscribe();
  });

  async function updateProfile() {
    //TODO: Validacija - primer je validacija u CheckoutForm.svelte

    const request: UserProfileUpdateRequest = {
      firstName: updatedFirstName,
      lastName: updatedLastName,
      addressLine: updatedAddressLine,
      city: updatedCity,
      zipCode: updatedZipCode,
      country: updatedCountry,
      phoneNumber: updatedPhoneNumber,
    };

    await userStore.updateProfile(request);
  }
</script>

<section class="flex flex-col gap-y-12 my-4">
  <h1 class="text-xl">Korisnički profil</h1>
  <div class="flex flex-col gap-y-4">
    {#if user}
      <div class="flex gap-x-4">
        <strong>Email:</strong>
        <div>{user.email}</div>
        <!-- Non-editable email display -->
      </div>
      <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
        <InputString
          bind:value={updatedFirstName}
          title="Ime"
          placeholder="Petar"
          width={"48"}
        />
        <InputString
          bind:value={updatedLastName}
          title="Prezime"
          placeholder="Petrović"
          width={"48"}
        />
      </div>
      <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
        <!-- Country (Readonly) -->
        <InputString
          bind:value={updatedCountry}
          title="Država"
          placeholder="Srbija"
          width={"32"}
        />

        <!-- City -->
        <InputString
          bind:value={updatedCity}
          title="Grad"
          placeholder="Novi Sad"
          width={"64"}
        />
      </div>
      <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
        <!-- Street and Number -->
        <InputString
          bind:value={updatedAddressLine}
          title="Ulica i broj"
          placeholder="Kneza Miloša 23"
          width={"80"}
        />
        <!-- Apartment -->
        <!-- <InputString
          bind:value={apartment}
          title="Broj stana"
          placeholder="5"
          isRequired={false}
          width={"24"}
        /> -->
      </div>
      <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
        <!-- Zip Code -->
        <InputString
          bind:value={updatedZipCode}
          title="Poštanski broj"
          placeholder="11000"
          isRequired={true}
          width={"32"}
        />
        <!-- Phone number -->
        <div class="relative">
          <InputString
            bind:value={updatedPhoneNumber}
            title="Broj telefona"
            placeholder="60224455221"
            width={"48"}
            prefix="+381"
          />
        </div>
      </div>
      <button
        on:click={updateProfile}
        class="mt-4 text-light bg-black text-white py-2 px-4 rounded-lg text-center tracking-widest hover:bg-gray-600 disabled:bg-gray-400"
      >
        Sačuvaj promene
      </button>
    {:else}
      <p>Loading user information...</p>
    {/if}
  </div>
</section>

<style>
</style>
