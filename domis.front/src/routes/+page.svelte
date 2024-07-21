<script>
  import { onMount } from "svelte";

  let url = "https://localhost:7210/WeatherForecast";
  let test = [];

  onMount(async () => {
    const res = await fetch(url);
    test = await res.json();
    console.info(test);
  });
</script>

<h1>Welcome to SvelteKit</h1>
<p>
  Visit <a href="https://kit.svelte.dev">kit.svelte.dev</a> to read the documentation
</p>
