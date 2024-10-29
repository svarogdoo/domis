<script lang="ts">
  import { syncService } from "../../services/sync-service";

  async function subscribeToNewsletter() {
    //TODO: Mikica double-check
    const emailInput = document.getElementById(
      "newsletter-email"
    ) as HTMLInputElement;
    const email = emailInput?.value;

    if (!email) {
      alert("Molimo vas da unesete email adresu.");
      return;
    }

    try {
      await syncService.subscribeToNewsletter(email);
      alert("Uspešno ste se prijavili na newsletter!");
    } catch (error) {
      alert("Došlo je do greške. Molimo vas pokušajte ponovo.");
      console.error("Error:", error);
    }
  }
</script>

<footer class="w-full flex flex-col pt-6 pb-1">
  <div class="flex flex-col lg:flex-row gap-y-4 pl-4 lg:pl-0 justify-around">
    <!-- Removed hidden class -->
    <div class="flex flex-col gap-y-4">
      <div class="header-title text-lg">
        <a href="/">domis</a>
        <a href="/">enterijeri</a>
      </div>
      <div class="flex flex-col gap-y-1 font-light">
        <p>PIB: 103758656</p>
        <p>Matični broj: 20014938</p>
        <p>Tekući račun: 265-3300310018509-64</p>
        <p>Sremska Mitrovica - Laćarak 1. novembar 10</p>
        <a
          href="https://domisenterijeri.com/domis/img/2020/10/Potvrda-o-izvrsenom-evidentiranju-za-pdv.pdf"
          >Potvrda o registraciji</a
        >
      </div>
    </div>
    <div class="flex flex-col gap-y-2">
      <p class="font-semibold tracking-wider">Korisnički kutak</p>
      <div class="flex flex-col gap-y-1 font-light">
        <a href="/kako-kupovati">Kako kupovati</a>
        <a href="/narucivanje">Naručivanje</a>
        <a href="/cesta-pitanja">Česta pitanja</a>
        <a href="/podaci-o-trgovcu">Podaci o trgovcu</a>
      </div>
    </div>
    <div class="flex flex-col gap-y-2">
      <p class="font-semibold tracking-wider">Kupovina</p>
      <div class="flex flex-col gap-y-1 font-light">
        <a href="/napomena-za-robu-poslatu-kurirskom-sluzbom"
          >Napomena za robu poslatu kurirskom sluzbom</a
        >
        <a href="/nacin-placanja">Način plaćanja</a>
        <a href="/uslovi-koriscenja">Uslovi korišćenja</a>
        <a href="/koriscenje-i-kupovina">Korišćenje i kupovina</a>
        <a href="/prava-obaveze-potrosaca">Prava i obaveze potrošača</a>
        <a href="/reklamacije">Reklamacije</a>
        <a href="/odustanak-od-ugovora">Pravo na odustanak od ugovora</a>
        <a
          href="https://domisenterijeri.com/domis/img/2020/10/DomisUgovoroprodajinadaljinu.docx"
          >Ugovor o prodaji na daljinu</a
        >
      </div>
    </div>
    <div class="flex flex-col gap-y-2">
      <a href="/prodajna-mesta" class="font-semibold tracking-wider"
        >Prodajna mesta</a
      >
      <a href="/montaza-ugradnja" class="font-semibold tracking-wider"
        >Montaža i ugradnja - video</a
      >
      <div class="newsletter flex flex-col gap-y-1 mt-4">
        <label for="newsletter-email" class="font-semibold tracking-wider"
          >Prijavi se na newsletter</label
        >
        <input
          type="email"
          id="newsletter-email"
          placeholder="Unesite vašu email adresu"
          class="p-2 border border-gray-300 rounded text-sm"
        />
        <button
          type="button"
          on:click={subscribeToNewsletter}
          class="mt-2 p-1 bg-domis-dark text-white text-xs rounded w-24 ml-auto"
        >
          Prijavi se
        </button>
      </div>
    </div>
  </div>
  <p class="text-center font-light text-xs text-gray-600 mt-2">
    © Copyright <a href="https://svarog.io/">Domis enterijeri</a> 2024
  </p>
</footer>

<style>
  footer {
    background-color: #d9d9d9;
  }

  .header-title {
    display: flex;
    flex-direction: column;
  }
  .header-title a {
    color: black;
    text-decoration: none;
    font-weight: 600;
    letter-spacing: 0.3em;
    line-height: 1.3em;
  }
</style>
