<script lang="ts">
  export let value: string;
  export let title: string;
  export let placeholder: string;
  export let error: string = "";
  export let isRequired: boolean = false;
  export let width: string;
  export let prefix: string = "";
</script>

<div class="flex flex-col">
  <div class="flex flex-col gap-y-2">
    {#if isRequired}
      <label for="input">{title} <span class="text-red-500">*</span></label>
    {:else}
      <label for="input">{title}</label>
    {/if}
    <div id="input" class="w-{width} relative rounded-md shadow-sm">
      {#if prefix}
        <p class="absolute inset-y-0 left-4 font-light flex items-center mr-2">
          {prefix}
        </p>
      {/if}
      <input
        type="text"
        name="value"
        id="value"
        class="block w-full font-light tracking-wider rounded-md border-0 py-1.5 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-300 focus:ring-inset focus:ring-blue-600 text-md leading-6 placeholder:font-light
              {prefix ? 'pl-16' : 'pl-3'}"
        {placeholder}
        bind:value
      />
    </div>
  </div>
  {#if error}
    <p class="mt-1 text-red-500 font-light text-sm">
      {error}
    </p>
  {/if}
</div>
