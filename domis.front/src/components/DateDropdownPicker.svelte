<script lang="ts">
  import { onMount } from "svelte";

  export let value: string = ""; // The bound date in local time string (ISO format)

  let date: Date = new Date(value); // Initialize with the bound date
  let months: string[] = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December",
  ];
  let years: number[] = [];
  let minutesList: number[] = [0, 30]; // Minutes will go by 30

  // Intermediate values for binding
  let selectedDay: number = date.getDate();
  let selectedMonth: number = date.getMonth();
  let selectedYear: number = date.getFullYear();
  let selectedHour: number = date.getHours();
  let selectedMinute: number = date.getMinutes();

  // Create a list of years from 2000 to the current year
  onMount(() => {
    const currentYear = new Date().getFullYear();
    for (let i = currentYear - 100; i <= currentYear; i++) {
      years.push(i);
    }
  });

  // Update the value when any of the dropdowns changes
  function updateDate() {
    date.setDate(selectedDay);
    date.setMonth(selectedMonth);
    date.setFullYear(selectedYear);
    date.setHours(selectedHour);
    date.setMinutes(selectedMinute);

    value = date.toISOString(); // Store the updated value as an ISO string
    dispatch("input", value); // Emit the updated value for binding
  }

  // Dispatch function to update parent component with the new value
  import { createEventDispatcher } from "svelte";
  const dispatch = createEventDispatcher();
</script>

<div class="space-y-2 bg-white p-4 rounded-md">
  <!-- Date Field (Day/Month/Year) -->
  <div class="flex items-center gap-2">
    <input
      type="number"
      bind:value={selectedDay}
      on:input={updateDate}
      class="w-16 text-center border-none p-1 bg-white"
      placeholder="DD"
    />
    <span>/</span>
    <select
      bind:value={selectedMonth}
      on:change={updateDate}
      class="w-32 text-center border-none p-1 bg-white"
    >
      {#each months as month, index}
        <option value={index}>{month}</option>
      {/each}
    </select>
    <span>/</span>
    <select
      bind:value={selectedYear}
      on:change={updateDate}
      class="w-32 text-center border-none p-1 bg-white"
    >
      {#each years as year}
        <option value={year}>{year}</option>
      {/each}
    </select>
  </div>

  <!-- Time Dropdown (Hour:Minute) -->
  <div class="flex items-center gap-2">
    <select
      bind:value={selectedHour}
      on:change={updateDate}
      class="w-16 text-center border-none p-1 bg-white"
    >
      {#each Array.from({ length: 24 }, (_, i) => i) as hour}
        <option value={hour}>{hour.toString().padStart(2, "0")}</option>
      {/each}
    </select>
    :
    <select
      bind:value={selectedMinute}
      on:change={updateDate}
      class="w-16 text-center border-none p-1 bg-white"
    >
      {#each minutesList as minute}
        <option value={minute}>{minute.toString().padStart(2, "0")}</option>
      {/each}
    </select>
  </div>
</div>

<style>
  select,
  input {
    text-align: center;
  }
</style>
