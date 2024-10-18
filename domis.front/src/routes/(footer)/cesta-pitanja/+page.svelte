<script lang="ts">
    let faqs = [
        {
            question: "Gde mogu da vidim koje proizvode nudi Domis enterijeri DOO?",
            answer: "U meniju koji sadrži sve kategorije i podkategorije proizvoda a nalazi se sa leve strane zaglavlja sajta, između logotipa Domis enterijeri i polja za pretragu. Svi proizvodi iz naše ponude grupisani su po kategorijama i raspoređeni u podkategorije prema vrsti proizvoda. Klikom na neki link iz menija dobijete detaljniji pregled."
        },
        {
            question: "Da li mogu dobiti informaciju o dužini trajanja garancije?",
            answer: "Država garantuje 24 meseca saobraznosti za svaki proizvod. Termin garancija upotrebljava se isključivo za vreme dužine trajanja saobraznosti koje je duže od 24 meseca, primera radi 3 god, 5 god…"
        },
        {
            question: "Gde mogu da proverim dostupnost proizvoda?",
            answer: "Ispod cene proizvoda imate polje „Na zalihama“ ili pre odlaska u salon možete pozvati naš call centar na telefon +381 22 679 125"
        },
        {
            question: "Za koje vreme ću dobiti naručene proizvode?",
            answer: "U zavisnosti od raspoložive količine određenog artikla i mogućnosti njegovog nabavljanja, a u skladu sa našom poslovnom politikom, očekivani rok isporuke proizvoda iz naše ponude je do 7 radnih dana."
        },
        {
            question: "Da li se uz proizvod dobija i uputstvo na srpskom jeziku?",
            answer: "Ukoliko Vam nedostaje uputstvo za bilo koji proizvod, upit možete poslati na domis-enterijeri@mts.rs."
        },
        {
            question: "Da li je moguće saznati trajanje neke akcije ili do kada važi cena na proizvodu?",
            answer: "Ukoliko nije naveden vremenski rok akcije, ona traje do isteka trenutnih zaliha, dok cena na proizvodu nije vremenski ograničena."
        },
        {
            question: "Da li je moguće odloženo plaćanje ako poručim online, ako nisam zaposlen za stalno ili putem administrativne zabrane?",
            answer: "Sve o načinima plaćanja u Domis enterijeri možete pročitati na linku Način plaćanja."
        },
        {
            question: 'Kako da pronađem proizvod koji mi treba?',
            answer: 'Do proizvoda koji tražite možete doći na dva načina:\n– Pregledom kategorija iz menija\n– Pretragom\nPregled kategorija podrazumeva listanje kategorija i podkategorija iz menija koji se nalazi sa leve strane. Na primer, ako želite da kupite Kaindlov Laminatni pod debljine 7mm, iz menija ćete odabrati kategoriju Podovi, a potom Laminatni pod i 7mm. Pretraga podrazumeva pronalaženja proizvoda na osnovu karakteristične ključne reči – najčešće naziv, model šifra, ili robna marka. Upišite ključne reči u predviđeno polje za pretragu i kliknite dugme za pretragu. Dobićete listu proizvoda koji odgovaraju zadatim ključnim rečima.'
        },
        {
            question: 'Šta je korpa?',
            answer: 'Korpa predstavlja Internet servis za kupovinu, koji Vam omogućava da kupujete po istom principu kao i sa ‘pravom korpom’ u prodavnici. Korpa služi da izaberete proizvode koje želite da kupite. Dok pregledate proizvode na sajtu možete ih ubaciti u korpu klikom na dugme ‘u korpu’. Na taj način izabrani proizvodi biće zapamćeni dok pregledate sajt. Ubacivanje proizvoda u korpu Vas ne obavezuje da taj proizvod kupite, a moguće je i izbacivanje proizvoda iz korpe. U svakom trenutku možete pregledati sadržaj svoje korpe klikom na link ‘vaša korpa’ koje se nalazi u zaglavlju sajta.'
        },
        {
            question: 'Kako da kupim nešto iz Internet prodavnice Domis enterijeri DOO?',
            answer: '1. Registrujte se\n2. Izaberite proizvode koje želite da kupite\n3. Popunite i pošaljite narudžbenicu\nDa biste mogli da naručujete proizvode morate biti registrovani korisnik. Registracija podrazumeva ispravno popunjavanje odgovarajućeg formulara. Izaberite proizvode koje želite da kupite, tako što ćete ih ubaciti u korpu. Proizvode ubacujete u korpu klikom na dugme ‘Kupi’ pored željenog proizvoda. Kada završite sa izborom proizvoda, otvorite stranu za pregled korpe „Vaša korpa“ i kliknite dugme ‘Plaćanje’. Popunite formular za naručivanje koji sadrži Detalje o porudžbini i nakon što se složite sa Uslovima korišćenja, kliknite dugme ‘Naručite’.'
        },
        {
            question: 'Šta je registracija?',
            answer: 'Registracijom kreirate svoj nalog za kupovinu u Domis enterijeri internet prodavnici. Registracija podrazumeva popunjevanje formulara sa osnovnim kontakt podacima koji služe za potvrdu i slanje narudžbe. Tom prilikom birate i lozinku koju ćete koristiti za pristup svom nalogu.'
        },
        {
            question: 'Šta da radim ako zaboravim lozinku?',
            answer: 'U formular za slanje lozinke upišite email koji koristite za pristup ovom sajtu i na njega ćemo Vam poslati lozinku ili obavestite administratora i biće vam kreirana nova lozinka.'
        },
        {
            question: 'Kako da promenim lozinku?',
            answer: 'Lozinku Vam može promeniti samo administrator. Ukoliko želite izmenu postojeće lozinke obratite se na domis.webshop@gmail.com.'
        },
        {
            question: 'Plaćanje poštarine',
            answer: 'Za svaku kupovinu preko 10.000,00 RSD poštarina se ne plaća, ukoliko je iznos kupovine niži od navedenog, ista se naplaćuje po težini paketa.'
        }
      // Add more FAQs here...
    ];
  
    let activeIndex: number | null = null; // Track the currently open dropdown
</script>
  
<section class="flex flex-col w-full items-center">
    <h1 class="mt-4 text-xl lg:text-2xl">Česta pitanja</h1>
    <div class="mt-12 grid grid-cols-1 lg:grid-cols-2 w-5/6 lg:w-2/3 gap-x-6 gap-y-6">
      {#each faqs as faq, index}
      <div class="w-full border-b border-gray-300 pb-4">
        <button class="font-bold text-left w-full" on:click={() => activeIndex = activeIndex === index ? null : index}>
          {faq.question}
        </button>
        {#if activeIndex === index}
        <div class="mt-4 text-sm">
          {faq.answer}
        </div>
        {/if}
      </div>
      {/each}
    </div>
  </section>
  
  <style>
  button {
    color: #7d0f05;
    background-color: transparent;
    border: none;
    cursor: pointer;
  }
  
  button:focus {
    outline: none;
  }
  
  div {
    transition: max-height 0.3s ease-out;
  }
  
  @media (min-width: 1024px) {
    .grid {
      display: grid;
      grid-template-columns: repeat(2, 1fr); /* Two columns on larger screens */
      gap: 1.5rem; /* Adjust gap between columns */
    }
  }
  </style>
  
  