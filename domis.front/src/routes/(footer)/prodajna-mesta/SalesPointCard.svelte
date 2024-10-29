<script lang="ts">
  import backup from "$lib/assets/backup.jpg"; // Fallback image

  export let salesPoint: SalesPoint;

  // Generate Google Maps link based on the address
  const mapsUrl = `https://www.google.com/maps/search/?api=1&query=${encodeURIComponent(salesPoint.address)}`;
</script>

<div
  class="w-full relative display flex flex-col justify-center items-center p-4 shadow-lg rounded-lg bg-white"
>
  <!-- Card Image with dark overlay and blur -->
  <div class="w-full relative display flex justify-center items-center">
    <div
      class="absolute w-full h-64 bg-domis-dark bg-opacity-70 backdrop-blur-sm"
    ></div>
    <img
      class="w-full h-64 object-cover opacity-45 bg-slate-500 hover:opacity-50 rounded-lg"
      src={salesPoint.image || backup}
      alt={salesPoint.name}
    />
    <p
      class="absolute text-white tracking-widest text-xl px-10 text-center font-bold z-10"
    >
      {salesPoint.name}
    </p>
  </div>

  <!-- Card Details (Address, Phone Numbers, Working Hours) -->
  <div class="w-full mt-4 text-center space-y-2">
    <!-- Add space-y-2 for vertical spacing -->
    <p class="text-sm font-semibold">{salesPoint.address}</p>
    <p class="text-sm text-gray-500">
      Telefon:
      {#each salesPoint.phoneNumbers as phoneNumber, i}
        {phoneNumber}{#if i < salesPoint.phoneNumbers.length - 1},
        {/if}
        <!-- Space after the comma -->
        {#if i < salesPoint.phoneNumbers.length - 1}{/if}
        <!-- Add a space after the comma -->
      {/each}
    </p>
    <p class="text-sm text-gray-500">Radno vreme: {salesPoint.workingHours}</p>
    {#if salesPoint.optionalInfo}
      <p class="text-sm font">{salesPoint.optionalInfo}</p>
    {/if}
  </div>

  <!-- Show on map button -->
  <div class="w-full mt-4 flex justify-center">
    {#if salesPoint.googleMapPin}
      <a
        href={salesPoint.googleMapPin}
        target="_blank"
        rel="noopener noreferrer"
        class="text-white bg-gray-500 hover:bg-blue-600 font-medium py-2 px-4 rounded-lg"
      >
        Prikaži na mapi
      </a>
    {:else}
      <a
        href={mapsUrl}
        target="_blank"
        rel="noopener noreferrer"
        class="text-white bg-gray-500 hover:bg-blue-600 font-medium py-2 px-4 rounded-lg"
      >
        Prikaži na mapi
      </a>
    {/if}
  </div>
</div>
