<script lang="ts">
  export let value: string;
  export let title: string;
  export let placeholder: string;
  export let error: string;
</script>

<div class="flex flex-col">
  <div class="flex gap-x-6 items-center">
    <p class="w-16">{title}:</p>
    <div
      class="{title === 'Naslov'
        ? 'w-80'
        : 'w-64'} relative rounded-md shadow-sm"
    >
      <input
        type="text"
        name="value"
        id="value"
        class="block w-full rounded-md border-0 py-1.5 pl-3 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-blue-600 text-md leading-6"
        {placeholder}
        bind:value
      />
      {#if title !== "Naslov" && title !== "Težina"}
        <p class="absolute inset-y-0 right-0 flex items-center mr-2">cm</p>
      {/if}
      {#if title === "Težina"}
        <p class="absolute inset-y-0 right-0 flex items-center mr-2">g</p>
      {/if}
    </div>
  </div>
  {#if error}
    <p class="mt-1 text-red-500 font-light text-sm pl-24">
      {error}
    </p>
  {/if}
</div>
