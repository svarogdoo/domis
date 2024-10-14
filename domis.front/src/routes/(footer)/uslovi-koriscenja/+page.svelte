<script>
    const lastUpdated = "18.4.2018.";
  
    const companyInfo = {
      name: "Domis enterijeri Shop",
      website: "http://www.domisenterijeri.com",
      address: "1. novembra 10",
      city: "22221 Laćarak",
      phone: "+381 (22) 679 125",
      fax: "+381 (22) 670 125",
      email: "office@domisenterijeri.com"
    };
  </script>
  
  <section class="flex flex-col w-full items-center">
    <h1 class="mt-4 text-xl lg:text-2xl">Uslovi korišćenja</h1>
    <p class="mt-12 w-5/6 lg:w-2/3 text-center">
      Poslednje izmene na ovoj stranici bile su {lastUpdated}.
    </p>
  
    <div class="mt-12 w-5/6 lg:w-2/3">
      <p>
        Ovom politikom privatnosti uređuju se način na koji {companyInfo.name} prikuplja, koristi, održava i objavljuje informacije prikupljene od korisnika (u daljem tekstu, “Korisnik”) preko <a href="{companyInfo.website}" target="_blank">{companyInfo.website}</a> sajta ili odredišnih strana u našem vlasništvu (u daljem tekstu “Sajt”).
      </p>
  
      <h2 class="mt-8 font-bold">Kako koristimo prikupljene informacije</h2>
      <p>
        {companyInfo.name} može prikupljati i koristiti lične podatke Korisnika u sledeće svrhe:
      </p>
      <ul class="list-disc ml-6">
        <li><strong>Da poboljša svoje usluge:</strong> Informacije koje prikupljamo nam pomažu da efikasnije odgovorimo na vaše zahteve korisničkoj službi i podršci.</li>
        <li><strong>Za personalizaciju iskustva korisnika:</strong> Možemo koristiti informacije da shvatimo kako naši Korisnici (kao grupa) koriste usluge i resurse koji se nalaze na našem Sajtu.</li>
        <li><strong>Da unapredimo naš Sajt:</strong> Možemo koristiti povratne informacije koje navedete za poboljšanje naših proizvoda i usluga.</li>
        <li><strong>Da bismo pokrenuli promocije, takmičenja, istraživanja ili druge funkcije Sajta:</strong> Slanje Korisnicima informacija koje su pristali da prime o temama njihovog interesovanja.</li>
        <li><strong>Da bismo poslali periodične e-poruke:</strong> Možemo koristiti e-mail za slanje informacija o ispravkama i novinama na Sajtu. Takođe se može koristiti kao kanal komunikacije da odgovorimo na Korisnikove molbe, pitanja i/ili druge zahteve. Ako Korisnici odluče da se opredele za našu mailing listu, oni će dobiti e-poruke koje mogu da uključuju vesti kompanije, informacije o novim ponudama, proizvodima ili uslugama, itd. Ako u bilo kom trenutku Korisnik želi da se odjavi od prijema buduće e-poruke, uključili smo detaljnu odjavu na dno svakog e-maila.</li>
      </ul>
  
      <h2 class="mt-8 font-bold">Lični identifikacioni podaci</h2>
      <p>
        {companyInfo.name} prikuplja lične, identifikacione podatke od Korisnika na različite načine, uključujući, ali ne ograničavajući se, kada Korisnici posećuju naš Sajt, registruju na Sajtu, prijavljuju se na newsletter, vebinar, popunjavaju formular, a u vezi sa drugim aktivnostima, uslugama, funkcijama ili resursima koji su dostupni na našem Sajtu.
      </p>
  
      <h2 class="mt-8 font-bold">Identifikacioni podaci koji nisu lične prirode</h2>
      <p>
        {companyInfo.name} ima pravo prikupljanja identifikacionih podataka o Korisniku koji nisu lične prirode u svakom trenutku njegove interakcije sa Sajtom.
      </p>
  
      <h2 class="mt-8 font-bold">Gde čuvamo Vaše podatke</h2>
      <p>
        Podatke o ličnosti prikupljamo i čuvamo u okviru email liste (baze podataka).
      </p>
  
      <h2 class="mt-8 font-bold">Kako štitimo informacije</h2>
      <p>
        {companyInfo.name} usvaja prikupljanje podataka, skladištenje i obradu i sigurnosne mere za zaštitu od neovlašćenog pristupa.
      </p>
  
      <h2 class="mt-8 font-bold">Deljenje ličnih podataka</h2>
      <p>
        {companyInfo.name} ne prodaje, trguje ili iznajmljuje Korisničke lične identifikacione podatke drugima.
      </p>
  
      <h2 class="mt-8 font-bold">Prava Korisnika</h2>
      <ul class="list-disc ml-6">
        <li>Da se u svakom trenutku odjavi sa email liste</li>
        <li>Zatraži da pogleda koji su njegovi podaci sačuvani u našoj bazi podataka</li>
        <li>Zatraži da bude zaboravljen (da se izbrišu svi njegovi podaci iz naše baze podataka)</li>
      </ul>
  
      <h2 class="mt-8 font-bold">Promene u politici privatnosti</h2>
      <p>
        {companyInfo.name} ima diskreciono pravo da ažurira ovu politiku privatnosti u bilo koje vreme. Preporučujemo Korisnicima da provere ovu stranicu s vremena na vreme za eventualne promene.
      </p>
  
      <h2 class="mt-8 font-bold">Važna napomena:</h2>
      <p>
        Izgled proizvoda na internet stranici može se razlikovati zbog kvaliteta fotografije.
      </p>
  
      <h2 class="mt-8 font-bold">Vaše prihvatanje ovih uslova</h2>
      <p>
        Korišćenjem ovog Sajta, prihvatate ovu politiku.
      </p>
  
      <h2 class="mt-8 font-bold">Kontaktirajte nas</h2>
      <p>
        Ukoliko imate bilo kakvo pitanje u vezi sa ovom politikom privatnosti, kontaktirajte nas na:
      </p>
      <address class="mt-4">
        <strong>{companyInfo.name}</strong><br>
        <a class="text-blue-500 underline" href={companyInfo.website} target="_blank">{companyInfo.website}</a><br>
        {companyInfo.address}<br>
        {companyInfo.city}<br>
        Tel: <a class="text-blue-500 underline" href={`tel:${companyInfo.phone.replace(/\s|\(|\)/g, '')}`}>{companyInfo.phone}</a><br>
        Fax: {companyInfo.fax}<br>
        e: <a class="text-blue-500 underline" href={`mailto:${companyInfo.email}`}>{companyInfo.email}</a>
      </address>
    </div>
  </section>
  