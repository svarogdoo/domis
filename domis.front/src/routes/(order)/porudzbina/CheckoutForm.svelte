<script lang="ts">
  import InputString from "../../../components/InputString.svelte";
  import { userStore } from "../../../stores/user";

  export const validate: () => CheckoutFormData | null = validateForm;

  let isUserDataPopulated = false;

  let name = "";
  let lastName = "";
  let companyName = "";
  const country = "Srbija"; // Readonly field
  let city = "";
  let apartment = "";
  let address = "";
  let county = "";
  let postalCode = "";
  let email = "";
  let phoneNumber = "";
  let specialNotes = "";

  $: if ($userStore.user && !isUserDataPopulated) {
    name = $userStore.user.firstName ?? "";
    lastName = $userStore.user.lastName ?? "";
    city = $userStore.user.city ?? "";
    address = $userStore.user.addressLine ?? "";
    apartment = $userStore.user.apartment ?? "";
    county = $userStore.user.county ?? "";
    postalCode = $userStore.user.postalCode ?? "";
    email = $userStore.user.email ?? "";
    phoneNumber = $userStore.user.phoneNumber ?? "";

    isUserDataPopulated = true;
  }

  // Form errors
  let errors = {
    name: "",
    lastName: "",
    city: "",
    address: "",
    apartment: "",
    postalCode: "",
    email: "",
    phoneNumber: "",
    county: "",
  };

  function validateForm(): CheckoutFormData | null {
    let valid = true;

    // Reset errors
    errors = {
      name: "",
      lastName: "",
      city: "",
      address: "",
      apartment: "",
      postalCode: "",
      email: "",
      phoneNumber: "",
      county: "",
    };

    if (!name.trim()) {
      errors.name = "Ime je obavezno polje";
      valid = false;
    }

    if (!lastName.trim()) {
      errors.lastName = "Prezime je obavezno polje";
      valid = false;
    }

    if (!city.trim()) {
      errors.city = "Grad je obavezno polje";
      valid = false;
    }

    if (!address.trim()) {
      errors.address = "Ulica i broj su neophodni";
      valid = false;
    }

    if (!postalCode.trim()) {
      errors.postalCode = "Poštanski broj je obavezan";
      valid = false;
    }

    const emailPattern = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/;
    if (!email.match(emailPattern)) {
      errors.email = "Nispravna vrednost email-a";
      valid = false;
    }

    const phonePattern = /^6\d{7,8}$/;
    if (!phoneNumber.match(phonePattern)) {
      errors.phoneNumber = "Neispravna vrednost broja telefona";
      valid = false;
    }

    if (valid) {
      const shippingDetails: ShippingDetails = {
        firstName: name,
        lastName: lastName,
        companyName: companyName,
        countryId: 1, // hardcode to Serbia
        city: city,
        address: address,
        county: county,
        apartment: apartment,
        postalCode: postalCode,
        phoneNumber: phoneNumber,
        email: email,
      };
      return {
        shippingDetails: shippingDetails,
        comment: specialNotes,
      };
    } else return null;
  }
</script>

<form class="flex flex-col w-full space-y-4 py-4 lg:p-4">
  <div class="flex flex-col gap-y-4 lg:gap-y-6">
    <h2
      class="text-xl tracking-wide border-b border-gray-400 pb-2 border-b-0.5"
    >
      Detalji za naplatu
    </h2>
    <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
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

    <!-- Company Name (Optional) -->
    <InputString
      bind:value={companyName}
      title="Naziv kompanije"
      placeholder="Kompanija d.o.o"
      isRequired={false}
      width={"80"}
    />
    <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
      <!-- Country (Readonly) -->
      <div class="flex flex-col gap-y-2">
        <label for="country">Zemlja</label>
        <input
          id="country"
          type="text"
          class="block w-32 bg-gray-50 rounded-md border-0 py-1.5 pl-3 text-gray-900 ring-1 ring-inset ring-gray-300 font-light text-md leading-6"
          value={country}
          readonly
          disabled
        />
      </div>

      <!-- City -->
      <InputString
        bind:value={city}
        title="Grad"
        placeholder="Novi Sad"
        error={errors?.city}
        isRequired={true}
        width={"64"}
      />
    </div>

    <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
      <!-- Postal Code -->
      <InputString
        bind:value={postalCode}
        title="Poštanski broj"
        placeholder="11000"
        error={errors?.postalCode}
        isRequired={true}
        width={"32"}
      />

      <!-- County -->
      <InputString
        bind:value={county}
        title="Opština"
        placeholder="Palilula"
        width={"64"}
      />
    </div>

    <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
      <!-- Street and Number -->
      <InputString
        bind:value={address}
        title="Ulica i broj"
        placeholder="Kneza Miloša 23"
        error={errors?.address}
        isRequired={true}
        width={"64"}
      />
      <!-- Apartment -->
      <InputString
        bind:value={apartment}
        title="Broj stana"
        placeholder="5"
        isRequired={false}
        width={"24"}
      />
    </div>

    <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
      <!-- Email -->
      <InputString
        bind:value={email}
        title="Email"
        placeholder="kompanija@gmail.com"
        error={errors?.email}
        isRequired={true}
        width={"80"}
      />
      <!-- Phone number -->
      <div class="relative">
        <InputString
          bind:value={phoneNumber}
          title="Broj telefona"
          placeholder="602244552"
          error={errors?.phoneNumber}
          isRequired={true}
          width={"48"}
          prefix="+381"
        />
      </div>
    </div>

    <!-- Special Notes (Optional) -->
    <div class="flex flex-col gap-y-2 mt-4">
      <label for="special-notes">Napomene o narudžbini</label>
      <textarea
        id="special-notes"
        class="block w-full rounded-md border-0 py-1.5 pl-3 font-light text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-inset focus:ring-blue-600 text-md leading-6"
        bind:value={specialNotes}
        placeholder="Posebne napomene o narudžbini ili isporuci."
      ></textarea>
    </div>
  </div>
</form>
