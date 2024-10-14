<script lang="ts">
    const contactPhone = "+38122671119"

    const infoItems = [
      { label: "Poslovno ime", value: "PREDUZEĆE ZA PROIZVODNJU, USLUGE I EXPORT-IMPORT DOMIS ENTERIJERI DOO LAĆARAK" },
      { label: "Skraćeno poslovno ime", value: "DOMIS ENTERIJERI DOO" },
      { label: "Adresa", value: "1. novembar 10, Sremska Mitrovica-Laćarak" },
      { label: "Delatnost i šifra delatnosti", value: "Trgovina na veliko drvetom, građevinskim materijalom i sanitarnom opremom – 4637" },
      { label: "Tekući račun", value: "330-0000051001115-82" },
      { label: "Matični broj", value: "20014938" },
      { label: "PIB", value: "103758656" },
      { label: "Potvrda o registraciji", value: "Potvrda o izvrsenom evidentiranju za PDV" },
      { label: "Web adresa", value: "www.domisenterijeri.com", link: "https://www.domisenterijeri.com" },
      { label: "Kontakt telefon", value: "+381 (22) 671 119", link: `tel:${contactPhone}` },
      { label: "Fax", value: "+381 (22) 671 396" },
      { label: "Veleprodaja", value: "+381 (22) 210 18 98" },
      { label: "Kontakt email", value: "office@domisenterijeri.com", link: "mailto:office@domisenterijeri.com" },
      { label: "Komercijala", value: "komercijala@domisenterijeri.com", link: "mailto:komercijala@domisenterijeri.com" },
      { label: "Računovodstvo", value: "racunovodstvo@domisenterijeri.com", link: "mailto:racunovodstvo@domisenterijeri.com" },
      { label: "Nabavka", value: "nabavka@domisenterijeri.com", link: "mailto:nabavka@domisenterijeri.com" },
      { label: "Web admin/marketing", value: "domis.webshop@gmail.com", link: "mailto:domis.webshop@gmail.com" },
    ];
  </script>
  
  <section class="flex flex-col w-full items-center">
    <h1 class="mt-4 text-xl lg:text-2xl">Podaci o trgovcu</h1>
    <div class="mt-12 flex flex-col w-5/6 lg:w-2/3 gap-y-6">
      {#each infoItems as item}
        <p class="w-full">
          <span class="font-bold">{item.label}:</span> 
          {#if item.link}
            <a href={item.link} class="text-blue-500 hover:underline">{item.value}</a>
          {:else}
            {item.value}
          {/if}
        </p>
      {/each}
    </div>
  </section>
  
  <style>
    /* Add any additional styles here if needed */
  </style>