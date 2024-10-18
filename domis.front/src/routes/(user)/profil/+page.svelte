<script lang="ts">
  import { userStore } from '../../../stores/user';
  import { onMount } from 'svelte';
    import { requireAuth } from '../../../utils/AuthGuard';

  let user: any;
  let updatedFirstName = '';
  let updatedLastName = '';
  let updatedAddressLine = '';
  let updatedCity = '';
  let updatedZipCode = '';
  let updatedCountry = '';
  let updatedPhoneNumber = '';

  // Subscribe to userStore to get the user info
  const unsubscribe = userStore.subscribe((state) => {

    user = state.user;
    console.log(user);
    if (user) {
      updatedFirstName = user.firstName || '';
      updatedLastName = user.lastName || '';
      updatedAddressLine = user.addressLine || '';
      updatedCity = user.city || '';
      updatedZipCode = user.zipCode || '';
      updatedCountry = user.country || '';
      updatedPhoneNumber = user.phoneNumber || '';
    }
  });

  //TODO: Mikica da vidi sta se ovde desava
  onMount(() => {
    requireAuth();
    return () => unsubscribe();
  });

  async function updateProfile() {
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

<div class="container">
  <div class="profile-card">
    <h1>Korisnički profil</h1>
    {#if user}
      <div class="profile-info">
        <strong>Email:</strong>
        <div class="email-display">{user.email}</div> <!-- Non-editable email display -->
      </div>
      <div class="profile-info">
        <strong>Ime:</strong>
        <input type="text" bind:value={updatedFirstName} />
      </div>
      <div class="profile-info">
        <strong>Prezime:</strong>
        <input type="text" bind:value={updatedLastName} />
      </div>
      <div class="profile-info">
        <strong>Adresa:</strong>
        <input type="text" bind:value={updatedAddressLine} />
      </div>
      <div class="profile-info">
        <strong>Grad:</strong>
        <input type="text" bind:value={updatedCity} />
      </div>
      <div class="profile-info">
        <strong>ZIP Kod:</strong>
        <input type="text" bind:value={updatedZipCode} />
      </div>
      <div class="profile-info">
        <strong>Država:</strong>
        <input type="text" bind:value={updatedCountry} />
      </div>
      <div class="profile-info">
        <strong>Broj telefona:</strong>
        <input type="tel" bind:value={updatedPhoneNumber} />
      </div>
      <button class="save-button" on:click={updateProfile}>Sačuvaj promene</button>
    {:else}
      <p class="loading">Loading user information...</p>
    {/if}
  </div>
</div>

<style>
  .container {
    display: flex;
    flex-direction: column;
    align-items: center; /* Center horizontally */
    height: 100vh;
    background-color: #f7f9fc;
    padding-top: 40px; /* Add padding to the top */
  }

  .profile-card {
    background: white;
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    padding: 20px;
    width: 300px;
    text-align: center;
  }

  h1 {
    font-size: 24px;
    margin-bottom: 16px;
  }

  .profile-info {
    margin: 10px 0;
    font-size: 16px;
    color: #333;
  }

  input {
    width: calc(100% - 10px); /* Full width minus padding */
    padding: 5px;
    border: 1px solid #ddd;
    border-radius: 4px;
    margin-top: 5px;
    font-size: 14px;
  }

  .email-display {
    padding: 5px; /* Adding some padding for better look */
    border: 1px solid #ddd; /* Optional: To visually separate from other inputs */
    border-radius: 4px;
    background-color: #f2f2f2; /* Light gray background */
    font-size: 14px;
    color: #555; /* Darker text color for better contrast */
  }

  .save-button {
    background-color: #007bff;
    color: white;
    padding: 10px;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    font-size: 16px;
    margin-top: 10px;
    transition: background-color 0.3s;
  }

  .save-button:hover {
    background-color: #0056b3;
  }

  .loading {
    font-size: 16px;
    color: #777;
  }
</style>
