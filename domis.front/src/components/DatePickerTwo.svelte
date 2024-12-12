<script lang="ts">
  import { createEventDispatcher, onMount } from "svelte";

  export let dateString: string;
  export let title: string;
  export let min: string = new Date().toLocaleString().split("T")[0];
  let timeString: string;

  const dispatch = createEventDispatcher<{ change: string }>();

  onMount(() => {
    setInitialValue();
  });

  function setInitialValue() {
    let currentDate = new Date(dateString);
    let currentHour = currentDate.getHours();
    let currentMinute = currentDate.getMinutes();

    // Round current minute to nearest 30-minute increment
    if (currentMinute >= 30) {
      currentHour++;
      currentMinute = 0;
    } else {
      currentMinute = 30;
    }

    timeString = `${currentHour}:${currentMinute < 10 ? "00" : currentMinute}`;
    currentDate.setMinutes(currentMinute);
    currentDate.setHours(currentHour);
    currentDate.setSeconds(0);
    dateString = currentDate.toLocaleString();
    dispatch("change", dateString);
  }

  // Handle the change events for date and time
  const handleDateChange = (e: Event) => {
    const target = e.target as HTMLInputElement;
    const newDate = new Date(target.value);

    // Ensure that the updated date keeps the selected time
    const [hours, minutes] = timeString
      .split(":")
      .map((num) => parseInt(num, 10));
    newDate.setHours(hours, minutes, 0);

    dateString = newDate.toLocaleString();
    dispatch("change", dateString);
  };

  const handleTimeChange = (e: Event) => {
    const target = e.target as HTMLSelectElement;
    const [hours, minutes] = target.value
      .split(":")
      .map((num) => parseInt(num, 10));
    const newDate = new Date(dateString);
    newDate.setHours(hours, minutes, 0);
    dateString = newDate.toLocaleString();
    dispatch("change", dateString);
  };
</script>

<div>
  <label for="date">{title}</label>
  <div class="flex gap-x-4">
    <input
      type="date"
      id="date"
      value={new Date(dateString).toISOString().split("T")[0]}
      {min}
      on:change={handleDateChange}
    />

    <select id="time" on:change={handleTimeChange} bind:value={timeString}>
      {#each Array(48) as _, index}
        <option
          value={`${Math.floor(index / 2)}:${index % 2 === 0 ? "00" : "30"}`}
        >
          {`${Math.floor(index / 2)}:${index % 2 === 0 ? "00" : "30"}`}
        </option>
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
