<script lang="ts">
  import { userStore } from "../../../../stores/user";
  import InputString from "../../../../components/InputString.svelte";
  import Checkbox from "../../../(navigation)/Checkbox.svelte";
  import { getUpdatedFields } from "../../../../helpers/objectHelpers";

  export let data;

  let user = { ...data.props.user };
  let initialCompanyInfo = {
    name: "",
    number: "",
    firstName: "",
    lastName: "",
  };

  let companyInfo = data.props.user.companyInfo
    ? { ...data.props.user.companyInfo }
    : initialCompanyInfo;

  let isSameUser =
    user.firstName === companyInfo.firstName &&
    user.lastName === companyInfo.lastName
      ? true
      : false;
  let userRole = data.props.userRole;

  let isCompany = user.companyInfo ? true : false;

  $: if (isSameUser) {
    companyInfo.firstName = user.firstName;
    companyInfo.lastName = user.lastName;
  }

  async function updateProfile() {
    const changedUserFields = getUpdatedFields(data.props.user, user);
    let changedCompanyInfoFields =
      data.props.user.companyInfo === undefined
        ? companyInfo
        : getUpdatedFields(data.props.user.companyInfo, companyInfo);

    if (companyInfo === initialCompanyInfo) changedCompanyInfoFields = null;

    const changedEntity = {
      ...changedUserFields,
      companyInfo: changedCompanyInfoFields,
    };

    await userStore.updateProfile(changedEntity);
  }
</script>

<section class="flex flex-col gap-y-8">
  <h1 class="text-2xl">Korisnički profil</h1>
  <div class="flex flex-col gap-y-4">
    {#if userRole}
      <p class="mb-4">
        <span class="font-semibold">Tip korisnika:</span>
        {userRole}
      </p>
    {/if}
    <div class="flex gap-x-4">
      <strong>Email:</strong>
      <div>{user.email}</div>
    </div>
    <div class="flex flex-col gap-y-4 lg:flex-row gap-x-12">
      <InputString
        bind:value={user.firstName}
        title="Ime"
        placeholder="Petar"
        width={"48"}
      />
      <InputString
        bind:value={user.lastName}
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

          <Checkbox bind:show={isSameUser} title="Korisnik je odgovorno lice" />
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

    <button
      on:click={updateProfile}
      class="mt-4 text-light bg-domis-dark text-white py-2 px-4 rounded-lg hover:bg-gray-600"
    >
      Sačuvaj promene
    </button>
  </div>
</section>
