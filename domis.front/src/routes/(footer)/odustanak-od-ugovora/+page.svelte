<script>
    import { onMount } from 'svelte';
  
    let lastModified = '20.10.2020';
  
    onMount(() => {
      // You can add any necessary initialization logic here
    });
  </script>
  
  <section class="flex flex-col w-full items-center">
    <h1 class="mt-4 text-xl lg:text-2xl">Pravo na odustanak od ugovora</h1>
    <h2 class="font-light text-sm lg:text-md">
      Poslednje izmene na ovoj stranici bile su {lastModified}
    </h2>
    <div class="mt-12 w-5/6 lg:w-2/3">
      <p class="mb-4">
        Ukoliko se proizvodi kupuju na daljinu (npr. preko internet sajta Prodavca) i isporučuju na adresu kupca, kupac ima pravo da da odustane od ugovora zaključenog na daljinu, u roku od 14 dana, bez navođenja razloga i dodatnih troškova, osim troškova vraćanja robe, koje u ovom slučaju snosi Kupac. Kod ugovora o prodaji robe, rok od 14 dana računa se od trenutka kada roba dospe u državinu Kupca, odnosno trećeg lica koje je odredio Kupac, a koje nije prevoznik.
      </p>
  
      <p class="mb-4">
        Kada Kupac jednom porudžbenicom naruči više vrsta roba koje se isporučuju zasebno, rok od 14 dana počinje da teče kada poslednja vrsta naručene robe dospe u državinu Kupca, odnosno trećeg lica koje je odredio Kupac, a koje nije prevoznik. Kada se isporuka robe sastoji iz više pošiljki i delova, rok od 14 dana počinje da teče kada je poslednja pošiljka ili deo, dospeo u državinu Kupca, odnosno trećeg lica koje je odredio Kupac, a koje nije prevoznik. Rok iz ovog stava ističe protekom poslednjeg časa poslednjeg dana roka.
      </p>
  
      <p class="mb-4">
        Kupac ostvaruje pravo na odustanak od ugovora izjavom koju može dati na posebnom obrascu za odustanak od ugovora zaključenog na daljinu, odnosno izvan poslovnih prostorija ili na drugi nedvosmislen način (u daljem tekstu: obrazac za odustanak). Izjava o odustanku od ugovora kod ugovora na daljinu i ugovora koji se zaključuju izvan poslovnih prostorija smatra se blagovremenom ukoliko je poslata Prodavcu u roku iz stava 1. ovog člana. Izjava o odustanku od ugovora proizvodi pravno dejstvo od dana kada je poslata Prodavcu. Ako Prodavac omogući Kupcu da elektronski popuni i pošalje obrazac za odustanak, dužan je da ga o prijemu obrasca bez odlaganja obavesti u pisanoj formi ili na drugom trajnom nosaču zapisa.
      </p>
  
      <p class="mb-4"> <!--TODO:href-->
        Formular za raskid ugovora možete skinuti <a href="/" class="text-blue-600 hover:underline">OVDE</a>. Popunjeni obrazac potrebno je da pošaljete na sledecu mail adresu komercijala@domisenterijeri.com
      </p>
  
      <p class="mb-4">
        Početkom roka iz stava 1 ovog člana, prestaje pravo Kupca na odustanak od ugovora.
      </p>
  
      <p class="mb-4">
        Prodavac je u slučaju zakonitog odustanka od ugovora dužan da Kupcu bez odlaganja vrati iznos koji je Kupac platio po osnovu ugovora, a najkasnije u roku od 14 dana od dana kada je primio obrazac za odustanak. Prodavac može da odloži povraćaj sredstava dok ne dobije robu koja se vraća, ili dok Kupac ne dostavi dokaz da je poslao robu Prodavcu u zavisnosti od toga šta nastupa prvo. Kupac je dužan da vrati robu Prodavcu ili licu ovlašćenom od strane Prodavca, bez odlaganja, a najkasnije u roku od 14 dana od dana kada je poslao obrazac za odustanak. Kupac snosi isključivo direktne troškove vraćanja robe. Kupac je isključivo odgovoran za umanjenu vrednost robe koja nastane kao posledica rukovanja robom na način koji nije adekvatan, odnosno prevazilazi ono što je neophodno da bi se ustanovili priroda, karakteristike i funkcionalnost robe. Kupać će proizvod vratiti u neoštećenoj originalnoj ambalaži, sa svom pripadajućom dokumentacijom koja se nalazi u originalnom pakovanju i u istom stanju u kakvom je i isporučen, bez ikakvih mehaničkih ili bilo kakvih drugih oštećenja.
      </p>
  
      <p class="mb-4">Kupac nema pravo da odustane od ugovora u slučaju:</p>
  
      <ul class="list-disc pl-5 mb-4">
        <li>Isporuke robe ili pružanja usluga čija cena zavisi od promena na finansijskom tržištu na koje Prodavac ne može da utiče i koje mogu nastati u toku roka za odustanak</li>
        <li>Isporuke robe proizvedene prema posebnim zahtevima Kupca ili jasno personalizovane</li>
        <li>Isporuke zapečaćene robe, zapečaćenih audio, video zapisa ili računarskog softvera, koji su otpečaćeni nakon isporuke</li>
        <li>Isporuke robe koja se, nakon isporuke, zbog svoje prirode neodvojivo meša sa drugom robom</li>
        <li>Isporuke digitalnog sadržaja koji nije isporučen na trajnom nosaču zapisa ako je izvršenje započelo posle prethodne izričite saglasnosti Kupca i njegove potvrde da zna da na taj način gubi pravo na odustanak od ugovora</li>
      </ul>
  
      <p class="mb-4">
        Ovim Ugovorom Kupac daje izričitu saglasnost isporuke digitalnog sadržaja koji nije isporučen na trajnom nosaču zapisa i potvrđuje da zna da na taj način gubi pravo na odustanak od ugovora.
      </p>
  
      <p class="mb-4"> <!--TODO:href-->
        Formular za raskid ugovora možete skinuti <a href="/" class="text-blue-600 hover:underline">OVDE</a>.
      </p>
  
      <p class="mb-4">
        Popunjeni obrazac potrebno je da pošaljete na sledecu mail adresu komercijala@domisenterijeri.com
      </p>
  
      <p class="mb-4">
        Troskove povrata robe u slucaju raskida ugovora snosi kupac.
      </p>
    </div>
  </section>
  
  <style>
    /* Add any component-specific styles here */
  </style>