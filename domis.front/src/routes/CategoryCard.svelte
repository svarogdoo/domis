<script lang="ts">
  import backup from "$lib/assets/backup.jpg";

  export let category: Category;
</script>

<a href="kategorija/{category?.id}">
  <div class="w-full relative display flex justify-center items-center">
    <div class="absolute w-full h-48 lg:h-96 rounded-lg bg-domis-dark"></div>
    <img
      class="w-full h-48 lg:h-96 rounded-lg object-cover opacity-60 text-white bg-slate-500 hover:opacity-60"
      src={backup}
      alt={category.name}
    />
    <p
      class="absolute text-white tracking-widest lg:text-xl px-10 text-center font-medium"
    >
      {category.name}
    </p>
  </div>
</a>
