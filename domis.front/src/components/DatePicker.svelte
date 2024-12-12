<script lang="ts">
  import { onMount } from "svelte";

  export let date: Date;
  export let title: string;

  $: if (date) {
    selectedDate = date.toISOString().split("T")[0];
    selectedTime = date.toTimeString().slice(0, 5);
  }

  let selectedDate: string;
  let selectedTime: string;
  let timeOptions: string[] = [];

  const generateTimeOptions = (): string[] => {
    const times: string[] = [];
    for (let h = 0; h < 24; h++) {
      for (let m = 0; m < 60; m += 30) {
        const hour = h.toString().padStart(2, "0");
        const minute = m.toString().padStart(2, "0");
        times.push(`${hour}:${minute}`);
      }
    }
    return times;
  };

  onMount(() => {
    timeOptions = generateTimeOptions();

    // Set default to now, rounded to the nearest 30 minutes
    const now = new Date();
    now.setSeconds(0, 0);
    const roundedMinutes = Math.ceil(now.getMinutes() / 30) * 30;
    if (roundedMinutes === 60) {
      now.setHours(now.getHours() + 1, 0, 0, 0);
    } else {
      now.setMinutes(roundedMinutes);
    }
    selectedDate = now.toISOString().split("T")[0];
    selectedTime = now.toTimeString().slice(0, 5);
    setDateTime();
  });

  const validateDate = (event: Event) => {
    const input = event.target as HTMLInputElement;
    const chosenDate = new Date(input.value);
    const now = new Date();
    now.setHours(0, 0, 0, 0); // Compare only the date part
    if (chosenDate < now) {
      input.value = selectedDate; // Reset if invalid
    } else {
      selectedDate = input.value;
    }

    setDateTime();
  };

  const validateTime = (event: Event) => {
    const input = event.target as HTMLSelectElement;
    const now = new Date();
    const currentDate = new Date(selectedDate);

    if (
      currentDate.toISOString().split("T")[0] ===
        now.toISOString().split("T")[0] &&
      input.value < now.toTimeString().slice(0, 5)
    ) {
      input.value = selectedTime; // Reset if invalid
    } else {
      selectedTime = input.value;
    }

    setDateTime();
  };

  function setDateTime() {
    date = new Date(`${selectedDate}T${selectedTime}:00.000Z`);
  }
</script>

<div class="flex flex-col gap-y-2">
  <p>{title}</p>
  <div class="flex gap-x-4">
    <input
      type="date"
      bind:value={selectedDate}
      min={new Date().toISOString().split("T")[0]}
      on:change={validateDate}
    />
    <select bind:value={selectedTime} on:change={validateTime}>
      {#each timeOptions as time}
        <option value={time}>{time}</option>
      {/each}
    </select>
  </div>
</div>

<style>
  select {
    padding: 0.5rem;
    border: 1px solid #ccc;
    border-radius: 4px;
    font-size: 1rem;
    background-color: white;
  }

  input {
    padding: 0.5rem;
    border: 1px solid #ccc;
    border-radius: 4px;
    font-size: 1rem;
  }
</style>
