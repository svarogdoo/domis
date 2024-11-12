<script>
  import notFoundImage from "$lib/assets/not-found.webp";
</script>

<div class="flex flex-col lg:flex-row gap-x-2 justify-center items-center">
  <img src={notFoundImage} alt="not found" class="w-60 h-60" />

  <!-- Optional 404 Message -->
  <div
    class="flex flex-col gap-y-4 mt-6 items-center text-center text-gray-500"
  >
    <h2 class="text-2xl font-semibold">Uups! Stranica nije pronađena.</h2>
    <p class="text-sm">Desila se greška ili tražena stranica ne postoji</p>
    <a class="px-4 py-3 w-48 font-light rounded bg-domis-secondary" href="/"
      >Nazad na početnu</a
    >
  </div>
</div>
