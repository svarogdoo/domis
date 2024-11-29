<script lang="ts">
  import Checkbox from "../../../(navigation)/Checkbox.svelte";
  import InputString from "../../../../components/InputString.svelte";
  import { userStore } from "../../../../stores/user";

  interface Address {
    firstName: string;
    lastName: string;
    addressLine: string;
    apartment: string;
    city: string;
    postalCode: string;
    country: string;
    county: string;
    phoneNumber: string;
  }

  export let data;

  let isUserDataPopulated = false;
  let userRole: string | undefined;
  let useSameAddress = true;

  let addressInvoice: Address = data.props.user;
  let addressDelivery: Address = {
    firstName: "",
    lastName: "",
    addressLine: "",
    apartment: "",
    city: "",
    postalCode: "",
    country: "",
    county: "",
    phoneNumber: "",
  };

  async function updateProfile() {
    console.info(addressDelivery, addressInvoice);
    // const request = {
    //   firstName: firstName,
    //   lastName: lastName,
    //   addressLine: addressLine,
    //   apartment: apartment,
    //   city: city,
    //   postalCode: postalCode,
    //   country: country,
    //   county: county,
    //   phoneNumber: phoneNumber,
    // };

    // await userStore.updateProfile(request);
  }
</script>

<section class="flex flex-col">
  <h1 class="text-2xl">Adrese</h1>
  <div class="w-full flex gap-x-12">
    <!-- Za naplatu -->
    <div class="flex flex-col">
      <div class="flex flex-col gap-y-2 mt-4">
        <p class="text-lg font-light">
          Adresa za naplatu {#if useSameAddress}
            <span>i isporuku</span>
          {/if}
        </p>
        <div
          class="flex flex-col gap-y-4 p-4 border border-gray-300 rounded-lg"
        >
          <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
            <InputString
              bind:value={addressInvoice.country}
              title="Država"
              placeholder="Srbija"
              width={"32"}
            />
            <InputString
              bind:value={addressInvoice.city}
              title="Grad"
              placeholder="Novi Sad"
              width={"64"}
            />
          </div>
          <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
            <InputString
              bind:value={addressInvoice.addressLine}
              title="Ulica i broj"
              placeholder="Kneza Miloša 23"
              width={"64"}
            />
            <InputString
              bind:value={addressInvoice.apartment}
              title="Stan"
              placeholder="Apt 12B"
              width={"32"}
            />
          </div>
          <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
            <InputString
              bind:value={addressInvoice.postalCode}
              title="Poštanski broj"
              placeholder="11000"
              width={"32"}
            />
            <InputString
              bind:value={addressInvoice.county}
              title="Opština"
              placeholder="Novi Sad"
              width={"64"}
            />
          </div>
          <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
            <InputString
              bind:value={addressInvoice.phoneNumber}
              title="Broj telefona"
              placeholder="602244552"
              width={"48"}
              prefix="+381"
            />
          </div>
        </div>
      </div>
      <Checkbox
        bind:show={useSameAddress}
        title="Koristi istu adresu za naplatu i isporuku"
      />
    </div>

    <!-- Za isporuku -->
    {#if !useSameAddress}
      <div class="flex flex-col gap-y-2 mt-4">
        <p class="text-lg font-light">Adresa za isporuku</p>
        <div
          class="flex flex-col gap-y-4 p-4 border border-gray-300 rounded-lg"
        >
          <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
            <InputString
              bind:value={addressDelivery.country}
              title="Država"
              placeholder="Srbija"
              width={"32"}
            />
            <InputString
              bind:value={addressDelivery.city}
              title="Grad"
              placeholder="Novi Sad"
              width={"64"}
            />
          </div>
          <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
            <InputString
              bind:value={addressDelivery.addressLine}
              title="Ulica i broj"
              placeholder="Kneza Miloša 23"
              width={"64"}
            />
            <InputString
              bind:value={addressDelivery.apartment}
              title="Stan"
              placeholder="Apt 12B"
              width={"32"}
            />
          </div>
          <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
            <InputString
              bind:value={addressDelivery.postalCode}
              title="Poštanski broj"
              placeholder="11000"
              width={"32"}
            />
            <InputString
              bind:value={addressDelivery.county}
              title="Opština"
              placeholder="Novi Sad"
              width={"64"}
            />
          </div>
          <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
            <InputString
              bind:value={addressDelivery.phoneNumber}
              title="Broj telefona"
              placeholder="602244552"
              width={"48"}
              prefix="+381"
            />
            <InputString
              bind:value={addressDelivery.firstName}
              title="Kontakt osoba"
              placeholder="Petar Petrović"
              width={"48"}
            />
          </div>
        </div>
      </div>
    {/if}
  </div>
  <button
    on:click={updateProfile}
    class="mt-4 text-light bg-domis-dark text-white py-2 px-4 rounded-lg hover:bg-gray-600"
  >
    Sačuvaj promene
  </button>
</section>
