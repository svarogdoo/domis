<script lang="ts">
  import Checkbox from "../../(navigation)/Checkbox.svelte";
  import InputString from "../../../components/InputString.svelte";
  import { AddressType } from "../../../enums";

  export const validate: () => CheckoutFormData | null = validateForm;
  export let userInitial: UserProfileResponse;

  let user = { ...userInitial };
  let companyInfo: CompanyInfo | null = user.companyInfo
    ? { ...user.companyInfo }
    : { name: "", number: "", firstName: "", lastName: "" };

  let specialNotes = "";
  let showCompany = false;
  let isSameUser =
    user.firstName === companyInfo.firstName &&
    user.lastName === companyInfo.lastName
      ? true
      : false;

  let errorsInit = {
    name: "",
    lastName: "",
    email: "",
    phoneNumber: "",
    addressInvoice: {
      city: "",
      addressLine: "",
      apartment: "",
      postalCode: "",
      county: "",
    },
    addressDelivery: {
      city: "",
      addressLine: "",
      apartment: "",
      postalCode: "",
      county: "",
      contactPerson: "",
      contactPhone: "",
    },
  };

  // Form errors
  let errors = { ...errorsInit };

  function validateForm(): CheckoutFormData | null {
    let valid = true;

    // Reset errors
    errors = { ...errorsInit };

    if (!user.firstName.trim()) {
      errors.name = "Ime je obavezno polje";
      valid = false;
    }

    if (!user.lastName.trim()) {
      errors.lastName = "Prezime je obavezno polje";
      valid = false;
    }

    if (!user.addressInvoice.city?.trim()) {
      errors.addressInvoice.city = "Grad je obavezno polje";
      valid = false;
    }

    if (!user.addressInvoice.addressLine?.trim()) {
      errors.addressInvoice.addressLine = "Ulica i broj su neophodni";
      valid = false;
    }

    if (!user.addressInvoice.postalCode?.trim()) {
      errors.addressInvoice.postalCode = "Poštanski broj je obavezan";
      valid = false;
    }

    const emailPattern = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/;
    if (!user.email?.match(emailPattern)) {
      errors.email = "Nispravna vrednost email-a";
      valid = false;
    }

    const phonePattern = /^6\d{7,8}$/;
    if (!user.phoneNumber?.match(phonePattern)) {
      errors.phoneNumber = "Neispravna vrednost broja telefona";
      valid = false;
    }

    if (!user.useSameAddress) {
      if (!user.addressDelivery.city?.trim()) {
        errors.addressDelivery.city = "Grad je obavezno polje";
        valid = false;
      }

      if (!user.addressDelivery.addressLine?.trim()) {
        errors.addressDelivery.addressLine = "Ulica i broj su neophodni";
        valid = false;
      }

      if (!user.addressDelivery.postalCode?.trim()) {
        errors.addressDelivery.postalCode = "Poštanski broj je obavezan";
        valid = false;
      }

      if (!user.addressDelivery.contactPerson?.trim()) {
        errors.addressDelivery.contactPerson = "Kontakt osoba je obavezna";
        valid = false;
      }

      if (!user.addressDelivery.contactPhone?.match(phonePattern)) {
        errors.addressDelivery.contactPhone =
          "Neispravna vrednost broja telefona";
        valid = false;
      }
    }

    if (valid) {
      const shippingInvoiceDetails: ShippingDetails = {
        firstName: user.firstName,
        lastName: user.lastName,
        phoneNumber: `+381${user.phoneNumber}`,
        countryId: 1, // hardcode to Serbia
        city: user.addressInvoice.city,
        addressLine: user.addressInvoice.addressLine,
        county: user.addressInvoice.county,
        apartment: user.addressInvoice.apartment,
        postalCode: user.addressInvoice.postalCode,
        email: user.email,
        addressType: AddressType.Invoice,
      };

      if (!showCompany) companyInfo = null;

      const shippingDeliveryDetails: ShippingDetails | null =
        user.useSameAddress
          ? null
          : {
              countryId: 1, // hardcode to Serbia
              city: user.addressDelivery.city,
              addressLine: user.addressDelivery.addressLine,
              county: user.addressDelivery.county,
              apartment: user.addressDelivery.apartment,
              postalCode: user.addressDelivery.postalCode,
              contactPerson: user.addressDelivery.contactPerson,
              contactPhone: `+381${user.addressDelivery.contactPhone}`,
              addressType: AddressType.Delivery,
            };

      return {
        addressInvoice: shippingInvoiceDetails,
        addressDelivery: shippingDeliveryDetails,
        companyInfo: companyInfo,
        comment: specialNotes,
      };
    } else return null;
  }
</script>

<form class="flex flex-col w-full space-y-4 lg:p-4" on:submit|preventDefault>
  <div class="flex flex-col gap-x-4">
    <div class="flex flex-col gap-y-2">
      <h2 class="text-lg font-light">
        Detalji za naplatu {#if user.useSameAddress}
          <span>i isporuku</span>
        {/if}
      </h2>
      <div class="flex flex-col gap-y-4 p-4 border border-gray-300 rounded-lg">
        <div class="flex flex-col gap-y-4 lg:flex-row gap-x-4">
          <!-- Name -->
          <InputString
            bind:value={user.firstName}
            title="Ime"
            placeholder="Petar"
            error={errors?.name}
            isRequired={true}
            width={"40"}
          />

          <!-- Last Name -->
          <InputString
            bind:value={user.lastName}
            title="Prezime"
            placeholder="Petrović"
            error={errors?.lastName}
            isRequired={true}
            width={"40"}
          />
          <!-- Email -->
          <InputString
            bind:value={user.email}
            title="Email"
            placeholder="kompanija@gmail.com"
            error={errors?.email}
            isRequired={true}
            width={"64"}
          />
          <!-- Phone number -->
          <div class="relative">
            <InputString
              bind:value={user.phoneNumber}
              title="Broj telefona"
              placeholder="602244552"
              error={errors?.phoneNumber}
              isRequired={true}
              width={"48"}
              prefix="+381"
            />
          </div>
        </div>

        <div class="flex flex-col gap-y-4 lg:flex-row gap-x-4">
          <!-- Country (Readonly) -->
          <div class="flex flex-col gap-y-2">
            <label for="country">Zemlja</label>
            <input
              id="country"
              type="text"
              class="block w-32 bg-gray-50 rounded-md border-0 py-1.5 pl-3 text-gray-900 ring-1 ring-inset ring-gray-300 font-light text-md leading-6"
              value={user.addressInvoice.country}
              readonly
              disabled
            />
          </div>
          <!-- Postal Code -->
          <InputString
            bind:value={user.addressInvoice.postalCode}
            title="Poštanski broj"
            placeholder="11000"
            error={errors?.addressInvoice.postalCode}
            isRequired={true}
            width={"32"}
          />
          <!-- City -->
          <InputString
            bind:value={user.addressInvoice.city}
            title="Grad"
            placeholder="Novi Sad"
            error={errors?.addressInvoice.city}
            isRequired={true}
            width={"48"}
          />
        </div>

        <div class="flex flex-col gap-y-4 lg:flex-row gap-x-4">
          <!-- County -->
          <InputString
            bind:value={user.addressInvoice.county}
            title="Opština"
            placeholder="Palilula"
            width={"48"}
          />
          <!-- Street and Number -->
          <InputString
            bind:value={user.addressInvoice.addressLine}
            title="Ulica i broj"
            placeholder="Kneza Miloša 23"
            error={errors?.addressInvoice.addressLine}
            isRequired={true}
            width={"64"}
          />
          <!-- Apartment -->
          <InputString
            bind:value={user.addressInvoice.apartment}
            title="Broj stana"
            placeholder="5"
            isRequired={false}
            width={"24"}
          />
        </div>

        <Checkbox
          bind:show={showCompany}
          title="Postavi podatke za pravna lica"
        />
        {#if showCompany}
          <div class="flex flex-col gap-y-2 mt-4">
            <p class="text-lg font-light">Podaci za pravna lica</p>
            <div
              class="flex flex-col gap-y-4 p-4 border border-gray-300 rounded-lg"
            >
              <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
                <InputString
                  bind:value={companyInfo.name}
                  title="Podaci kompanije"
                  placeholder="Firma d.o.o."
                  width={"64"}
                />
                <InputString
                  bind:value={companyInfo.number}
                  title="PIB"
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
                    bind:value={user.firstName}
                    title="Ime"
                    placeholder="Petar"
                    width={"48"}
                    isReadOnly={isSameUser}
                  />
                  <InputString
                    bind:value={user.lastName}
                    title="Prezime"
                    placeholder="Petrović"
                    width={"48"}
                    isReadOnly={isSameUser}
                  />
                {:else}
                  <InputString
                    bind:value={companyInfo.firstName}
                    title="Ime"
                    placeholder="Petar"
                    width={"48"}
                  />
                  <InputString
                    bind:value={companyInfo.lastName}
                    title="Prezime"
                    placeholder="Petrović"
                    width={"48"}
                  />
                {/if}
              </div>
            </div>
          </div>
        {/if}
      </div>
      <Checkbox
        bind:show={user.useSameAddress}
        title="Koristi istu adresu za naplatu i isporuku"
      />
    </div>
    {#if !user.useSameAddress}
      <div class="flex flex-col gap-y-2 mt-6">
        <h2 class="text-lg font-light">Detalji za isporuku</h2>

        <div
          class="flex flex-col gap-y-4 p-4 border border-gray-300 rounded-lg"
        >
          <div class="flex flex-col gap-y-4 lg:flex-row gap-x-4">
            <!-- Country (Readonly) -->
            <div class="flex flex-col gap-y-2">
              <label for="country">Zemlja</label>
              <input
                id="country"
                type="text"
                class="block w-32 bg-gray-50 rounded-md border-0 py-1.5 pl-3 text-gray-900 ring-1 ring-inset ring-gray-300 font-light text-md leading-6"
                value={user.addressDelivery.country}
                readonly
                disabled
              />
            </div>
            <!-- Postal Code -->
            <InputString
              bind:value={user.addressDelivery.postalCode}
              title="Poštanski broj"
              placeholder="11000"
              error={errors?.addressDelivery.postalCode}
              isRequired={true}
              width={"32"}
            />
            <!-- City -->
            <InputString
              bind:value={user.addressDelivery.city}
              title="Grad"
              placeholder="Novi Sad"
              error={errors?.addressDelivery.city}
              isRequired={true}
              width={"64"}
            />
          </div>

          <div class="flex flex-col gap-y-4 lg:flex-row gap-x-4">
            <!-- County -->
            <InputString
              bind:value={user.addressDelivery.county}
              title="Opština"
              placeholder="Palilula"
              width={"64"}
            />
            <!-- Street and Number -->
            <InputString
              bind:value={user.addressDelivery.addressLine}
              title="Ulica i broj"
              placeholder="Kneza Miloša 23"
              error={errors?.addressDelivery.addressLine}
              isRequired={true}
              width={"64"}
            />
            <!-- Apartment -->
            <InputString
              bind:value={user.addressDelivery.apartment}
              title="Broj stana"
              placeholder="5"
              isRequired={false}
              width={"24"}
            />
          </div>

          <div class="flex flex-col gap-y-4 lg:flex-row gap-x-4">
            <!-- Contact person -->
            <InputString
              bind:value={user.addressDelivery.contactPerson}
              title="Kontakt osoba"
              placeholder="Petar Petrović"
              error={errors?.addressDelivery.contactPerson}
              isRequired={true}
              width={"48"}
            />
            <!-- Contact phone number -->
            <div class="relative">
              <InputString
                bind:value={user.addressDelivery.contactPhone}
                title="Broj telefona"
                placeholder="602244552"
                error={errors?.addressDelivery.contactPhone}
                isRequired={true}
                width={"48"}
                prefix="+381"
              />
            </div>
          </div>
        </div>
      </div>
    {/if}

    <div class="flex flex-col gap-y-4">
      <!-- Special Notes (Optional) -->
      <div class="flex flex-col gap-y-2 my-4">
        <label for="special-notes">Napomene o narudžbini</label>
        <textarea
          id="special-notes"
          class="block w-full rounded-md border-0 py-1.5 pl-3 font-light text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-inset focus:ring-blue-600 text-md leading-6"
          bind:value={specialNotes}
          placeholder="Posebne napomene o narudžbini ili isporuci."
        ></textarea>
      </div>
    </div>
  </div>
</form>
