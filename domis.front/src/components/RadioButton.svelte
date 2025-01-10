<script lang="ts">
  export let options;
  export let legend;
  export let userSelected = options[0].value;
  export let isColumn = false;

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
  <div class="mb-2 cursor-default" id={`label-${uniqueID}`}>{legend}</div>
  <div
    class="flex flex-wrap gap-x-4 lg:gap-x-8 {isColumn
      ? 'flex-col'
      : ''} gap-y-2"
  >
    {#each options as { value, label, text }}
      <div>
        <label class="flex cursor-pointer" for={slugify(label)}>
          <input
            type="radio"
            class="hidden peer"
            id={slugify(label)}
            bind:group={userSelected}
            {value}
          />
          <div
            class="relative w-5 h-5 border-2 border-gray-300 rounded-full flex items-center justify-center mr-1 peer-checked:border-gray-700 peer-checked:bg-gray-700"
          >
            <span class="absolute text-white left-0.5">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-3 w-3"
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
          </div>
          <span class="ml-1"> {label} </span>
        </label>
        {#if text && userSelected == value}<p
            class="font-light my-2 text-wrap cursor-default"
          >
            {text}
          </p>
        {/if}
      </div>
    {/each}
  </div>
</div>
