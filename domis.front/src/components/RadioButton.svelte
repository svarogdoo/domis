<script lang="ts">
  export let options;
  export let legend;
  export let userSelected = options[0].value;

  const uniqueID = Math.floor(Math.random() * 100);

  const slugify = (str = "") =>
    str.toLowerCase().replace(/ /g, "-").replace(/\./g, "");
</script>

<div
  class="flex flex-col mt-2"
  role="radiogroup"
  aria-labelledby={`label-${uniqueID}`}
  id={`group-${uniqueID}`}
>
  <div class="text-lg mb-2" id={`label-${uniqueID}`}>{legend}</div>
  <div class="flex gap-x-8">
    {#each options as { value, label }}
      <div>
        <input
          type="radio"
          id={slugify(label)}
          bind:group={userSelected}
          {value}
        />
        <label for={slugify(label)}> {label} </label>
      </div>
    {/each}
  </div>
</div>
