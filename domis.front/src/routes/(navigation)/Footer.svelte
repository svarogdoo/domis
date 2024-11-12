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

<footer class="w-full flex flex-col items-center pt-6 pb-1 bg-domis-light">
  <div class="flex flex-col w-full lg:w-4/5 lg:flex-row gap-y-4 px-4 lg:pl-0">
    <div class="lg:w-1/6 flex flex-col gap-y-4">
      <div class="header-title text-lg">
        <a class="text-domis-primary" href="/">domis</a>
        <a href="/">enterijeri</a>
      </div>
      <div class="flex flex-col gap-y-1 font-light">
        <a href="/podaci-o-trgovcu">Naši podaci</a>
        <a href="/prodajna-mesta">Prodajna mesta</a>
        <a href="/montaza-ugradnja">Montaža i ugradnja</a>
      </div>
    </div>
    <div class="lg:w-1/6 flex flex-col gap-y-2">
      <p class="font-semibold tracking-wider">Korisnički kutak</p>
      <div class="flex flex-col gap-y-1 font-light">
        <a href="/kako-kupovati">Kako kupovati</a>
        <a href="/narucivanje">Naručivanje</a>
        <a href="/cesta-pitanja">Česta pitanja</a>
        <a href="/nacin-placanja">Način plaćanja</a>
      </div>
    </div>
    <div class="lg:w-2/6 flex flex-col gap-y-2">
      <p class="font-semibold tracking-wider">Kupovina</p>
      <div class="flex flex-col gap-y-1 font-light">
        <a href="/napomena-za-robu-poslatu-kurirskom-sluzbom"
          >Napomena za robu poslatu kurirskom sluzbom</a
        >
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
    <div class="lg:w-2/6 flex flex-col gap-y-2">
      <div class="newsletter flex flex-col items-center gap-y-2 mt-4">
        <label for="newsletter-email" class="font-semibold tracking-wider"
          >Prijavi se na newsletter</label
        >
        <div class="relative">
          <input
            type="email"
            id="newsletter-email"
            placeholder="Unesite vašu email adresu"
            class="px-6 py-3 border border-gray-300 w-80 rounded text-sm"
          />
          <button
            type="button"
            on:click={subscribeToNewsletter}
            class="absolute right-0 h-full bg-domis-dark text-white text-xs tracking-wider rounded w-24"
          >
            Prijavite se
          </button>
        </div>
      </div>
    </div>
  </div>
  <p class="text-center font-light text-xs text-gray-600 mt-2">
    © Copyright <a href="https://svarog.io/"
      ><span class="text-domis-primary">Domis</span> enterijeri</a
    > 2024
  </p>
</footer>

<style>
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
