<script lang="ts">
  export let category = {
    id: 1,
    name: "",
    image: "",
  };
</script>

<a href="/{category.name.toLowerCase()}">
  <div class="w-full relative display flex justify-center items-center">
    <div class="absolute w-full h-96 bg-black"></div>
    <img
      class="w-full h-96 object-cover opacity-80 text-white bg-slate-500 hover:opacity-60"
      src={category.image}
      alt="CategoryCard"
    />
    <p class="absolute text-white tracking-widest text-xl">{category.name}</p>
  </div>
</a>
