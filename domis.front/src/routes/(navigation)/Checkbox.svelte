<script lang="ts">
  export let show: boolean;
  export let title: string;
</script>

<button
  on:click={() => (show = !show)}
  class="relative flex mt-4 gap-x-2 items-center"
>
  <input
    type="checkbox"
    class="appearance-none h-5 w-5 cursor-pointer rounded-md border border-green-700 checked:bg-green-700"
    checked={show}
  />
  <span class="absolute text-white left-0.5">
    <svg
      xmlns="http://www.w3.org/2000/svg"
      class="h-3.5 w-3.5"
      viewBox="0 0 20 20"
      fill="currentColor"
      stroke="currentColor"
      stroke-width="1"
    >
      <path
        fill-rule="evenodd"
        d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
        clip-rule="evenodd"
      ></path>
    </svg>
  </span>
  <p class="text-green-700 pt-1 text-sm font-extralight tracking-wider">
    {title}
  </p>
</button>
