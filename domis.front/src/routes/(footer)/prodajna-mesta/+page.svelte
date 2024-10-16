<script lang="ts">
  import { onMount } from "svelte";
  import SalesPointCard from "./SalesPointCard.svelte";
  import { API_URL } from "../../../config";
  import { fetchData } from "../../../helpers/fetch";

  // Hardcoded data
  const hardcodedSalesPoints: SalesPoint[] = [
    {
      id: 1,
      name: "Sremska Mitrovica - Laćarak",
      address: "1. Novembra 10",
      phoneNumbers: ["(+381 22) 671 119", "(+381 22) 671 396"],
      workingHours: "9:00 AM - 5:00 PM",
      image: "https://domisenterijeri.com/domis/img/2021/01/Maloprodaja.jpg",
      googleMapPin: "https://maps.app.goo.gl/nNyjFPrLDNz33JoF8",
    },
    {
      id: 2,
      name: "Novi Sad",
      address:
        "Ugao ulica Sentandrejski put i Partizanske (kod tržnog centra BIG)",
      phoneNumbers: ["(+381 21) 442 633", "(+381 21) 442 644"],
      workingHours: "Radnim danima 07h-19h, subotom 07h-14h",
      image: "https://domisenterijeri.com/domis/img/2021/01/NoviSad.jpg",
      googleMapPin: "https://maps.app.goo.gl/5GNmDtgS88nyg7vRA",
    },
    {
      id: 3,
      name: "Inđija",
      address: "Put za Staru Pazovu (preko puta OMV pumpe)",
      phoneNumbers: ["(+381 22) 560 940"],
      workingHours: "Radno vreme: Radnim danima 07h-19h, subotom 07h-14h",
      image: "https://domisenterijeri.com/domis/img/2021/01/Indjija.jpg",
      googleMapPin: "https://maps.app.goo.gl/9naXRhUUtgZNiA2H7",
    },
    {
      id: 4,
      name: "Veleprodaja",
      address: "1. Novembra 11",
      phoneNumbers: ["(+381 22) 210 18 98"],
      workingHours: "07h-19h, subotom 07h-14h",
      image: "https://domisenterijeri.com/domis/img/2021/01/VPLacarak.jpg",
      optionalInfo: "Fax (+381 22) 670 125",
      googleMapPin: "https://maps.app.goo.gl/JyDag5mutv7VH7RE8",
    },
    {
      id: 5,
      name: "Šabac",
      address: "Obilazni put za Loznicu (pored OMV pumpe)",
      phoneNumbers: ["(+381 15) 311 330"],
      workingHours: "Radnim danima 07h-19h, subotom 07h-14h",
      image: "https://domisenterijeri.com/domis/img/2021/01/Sabac-3.jpg",
      googleMapPin: "https://maps.app.goo.gl/16Cz5XjEygrNehnb6",
    },
    {
      id: 6,
      name: "Zlatibor",
      address: "Magistralni put za Zaltibor Sušica (pored Knez Petrol)",
      phoneNumbers: ["(+381 64) 887 82 52"],
      workingHours: "Radnim danima 08h-15h, subotom 09h-14h",
      image: "https://domisenterijeri.com/domis/img/2021/01/Zlatibor-1.jpg",
      googleMapPin: "https://maps.app.goo.gl/cZBMK9KmkHJnBmUP6",
    },
  ];

  let salesPoints: SalesPoint[] = [];

  async function fetchSalesPoints() {
    try {
      salesPoints = await fetchData<SalesPoint[]>(
        `${API_URL}/api/sales-points`
      );
    } catch (error) {
      console.error("Failed to fetch sales points:", error);
      salesPoints = hardcodedSalesPoints; // Use hardcoded data on failure
    }
  }

  onMount(() => {
    fetchSalesPoints();
  });
</script>

<section class="mt-4 w-full flex justify-center">
  <div
    class="w-11/12 xl:w-3/4 category-cards grid grid-cols-1 lg:grid-cols-3 gap-x-4 gap-y-4"
  >
    {#each salesPoints as salesPoint}
      <SalesPointCard {salesPoint} />
    {/each}
  </div>
</section>
