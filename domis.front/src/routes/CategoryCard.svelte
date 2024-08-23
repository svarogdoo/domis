<script lang="ts">
  import backup from "$lib/assets/backup.jpg";

  export let category: Category;
</script>

<a href="kategorija/{category?.id}">
  <div class="w-full relative display flex justify-center items-center">
    <div class="absolute w-full h-96 bg-black"></div>
    <img
      class="w-full h-96 object-cover opacity-60 text-white bg-slate-500 hover:opacity-60"
      src={backup}
      alt={category.name}
    />
    <p
      class="absolute text-white tracking-widest text-xl text-center font-medium"
    >
      {category.name}
    </p>
  </div>
</a>
