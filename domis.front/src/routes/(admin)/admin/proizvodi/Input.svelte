<script lang="ts">
  export let value: string | number;
  export let title: string | null = null;
  export let placeholder: string;
  export let error: string;
  export let width: string;
  export let gap: string;
  export let suffix: string = "";
  export let type = "text";
</script>

<div class="flex flex-col">
  <div class="flex gap-x-2 lg:gap-x-6 items-center">
    {#if title}
      <p class="w-{gap} text-sm lg:text-md">{title}:</p>
    {/if}
    <div class="w-{width} relative rounded-md shadow-sm">
      {#if type === "text"}
        <input
          type="text"
          name="value"
          id="value"
          class="block w-full rounded-md border-0 py-1.5 pl-3 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-blue-600 text-sm lg:text-md leading-6"
          {placeholder}
          bind:value
        />
      {:else}
        <input
          type="number"
          name="value"
          id="value"
          class="block w-full rounded-md border-0 py-1.5 pl-3 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-blue-600 text-sm lg:text-md leading-6"
          {placeholder}
          bind:value
        />
      {/if}
      {#if suffix}
        <p class="absolute inset-y-0 right-0 flex items-center mr-2 font-light">
          {suffix}
        </p>
      {/if}
    </div>
  </div>
  {#if error}
    <p class="mt-1 text-red-500 font-light text-xs lg:text-sm pl-24">
      {error}
    </p>
  {/if}
</div>
