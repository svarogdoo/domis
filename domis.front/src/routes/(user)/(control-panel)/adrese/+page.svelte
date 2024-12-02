<script lang="ts">
  import Checkbox from "../../../(navigation)/Checkbox.svelte";
  import InputString from "../../../../components/InputString.svelte";
  import {
    getUpdatedFields,
    hasAnyFieldSet,
  } from "../../../../helpers/objectHelpers";
  import { userStore } from "../../../../stores/user";

  export let data;

  let addressInvoice: Address = { ...data.props.user.addressInvoice };
  let addressDelivery: Address = { ...data.props.user.addressDelivery };

  let phoneNumber = data.props.user.phoneNumber ?? "";
  let useSameAddress = hasAnyFieldSet(addressDelivery, ["addressType"])
    ? false
    : true;

  async function updateProfile() {
    const changedInvoiceFields = getUpdatedFields(
      data.props.user.addressInvoice,
      addressInvoice
    );
    const changedDeliveryFields = getUpdatedFields(
      data.props.user.addressDelivery,
      addressDelivery
    );

    const changedEntity = {
      addressInvoice: changedInvoiceFields,
      addressDelivery: changedDeliveryFields,
      useSameAddress: useSameAddress,
      phoneNumber:
        phoneNumber !== data.props.user.phoneNumber ? phoneNumber : null,
    };

    await userStore.updateProfile(changedEntity);
  }
</script>

<section class="flex flex-col">
  <h1 class="text-2xl">Adrese</h1>
  <div class="w-full flex flex-col lg:flex-row gap-x-12">
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
              bind:value={phoneNumber}
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
              bind:value={addressDelivery.contactPhone}
              title="Kontakt telefon"
              placeholder="602244552"
              width={"48"}
              prefix="+381"
            />
            <InputString
              bind:value={addressDelivery.contactPerson}
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
