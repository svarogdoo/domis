<script lang="ts">
  import { createEventDispatcher, onMount } from "svelte";

  export let min: number = 0;
  export let max: number = 100;
  export let initMin: number = 0;
  export let initMax: number = 100;
  export let unit: string = "RSD";

  let fromSlider: HTMLInputElement;
  let toSlider: HTMLInputElement;
  let fromInput: HTMLInputElement;
  let toInput: HTMLInputElement;
  let start: number = 0;
  let end: number = 1000;

  const dispatch = createEventDispatcher<{ change: string }>();

  $: if (start > end) {
    start = end;
  }

  $: if (initMin || initMax) {
    start = initMin;
    end = initMax;
  }

  onMount(() => {
    start = initMin;
    end = initMax;

    fromSlider.value = start.toString();
    toSlider.value = end.toString();

    fillSlider(fromSlider, toSlider, "#C6C6C6", "#2F2F2F", toSlider);
    setToggleAccessible(toSlider);

    fromSlider.addEventListener("input", () =>
      controlFromSlider(fromSlider, toSlider, fromInput)
    );
    toSlider.addEventListener("input", () =>
      controlToSlider(fromSlider, toSlider, toInput)
    );
    fromInput.addEventListener("input", () =>
      controlFromInput(fromSlider, fromInput, toInput, toSlider)
    );
    toInput.addEventListener("input", () =>
      controlToInput(toSlider, fromInput, toInput, toSlider)
    );
  });

  function controlFromInput(
    fromSlider: HTMLInputElement,
    fromInput: HTMLInputElement,
    toInput: HTMLInputElement,
    controlSlider: HTMLInputElement
  ) {
    const [from, to] = getParsed(fromInput, toInput);
    fillSlider(fromInput, toInput, "#C6C6C6", "#2F2F2F", controlSlider);
    if (from > to) {
      fromSlider.value = to.toString();
      fromInput.value = to.toString();
    } else {
      fromSlider.value = from.toString();
    }

    dispatchStartAndEndValues(from, to);
  }

  function controlToInput(
    toSlider: HTMLInputElement,
    fromInput: HTMLInputElement,
    toInput: HTMLInputElement,
    controlSlider: HTMLInputElement
  ) {
    const [from, to] = getParsed(fromInput, toInput);
    fillSlider(fromInput, toInput, "#C6C6C6", "#2F2F2F", controlSlider);
    setToggleAccessible(toInput);
    if (from <= to) {
      toSlider.value = to.toString();
      toInput.value = to.toString();
    } else {
      toInput.value = from.toString();
    }
    dispatchStartAndEndValues(from, to);
  }

  function controlFromSlider(
    fromSlider: HTMLInputElement,
    toSlider: HTMLInputElement,
    fromInput: HTMLInputElement
  ) {
    const [from, to] = getParsed(fromSlider, toSlider);
    fillSlider(fromSlider, toSlider, "#C6C6C6", "#2F2F2F", toSlider);
    if (from > to) {
      fromSlider.value = to.toString();
      fromInput.value = to.toString();
    } else {
      fromInput.value = from.toString();
    }

    dispatchStartAndEndValues(from, to);
  }

  function controlToSlider(
    fromSlider: HTMLInputElement,
    toSlider: HTMLInputElement,
    toInput: HTMLInputElement
  ) {
    const [from, to] = getParsed(fromSlider, toSlider);
    fillSlider(fromSlider, toSlider, "#C6C6C6", "#2F2F2F", toSlider);
    setToggleAccessible(toSlider);
    if (from <= to) {
      toSlider.value = to.toString();
      toInput.value = to.toString();
    } else {
      toInput.value = from.toString();
      toSlider.value = from.toString();
    }

    dispatchStartAndEndValues(from, to);
  }

  function getParsed(
    currentFrom: HTMLInputElement,
    currentTo: HTMLInputElement
  ): [number, number] {
    const from = parseInt(currentFrom.value, 10);
    const to = parseInt(currentTo.value, 10);
    return [from, to];
  }

  function fillSlider(
    from: HTMLInputElement,
    to: HTMLInputElement,
    sliderColor: string,
    rangeColor: string,
    controlSlider: HTMLInputElement
  ) {
    // Ensure max and min are numbers
    const min = parseInt(to.min, 10);
    const max = parseInt(to.max, 10);
    const rangeDistance = max - min;

    // Ensure values are numbers
    const fromValue = parseInt(from.value, 10);
    const toValue = parseInt(to.value, 10);

    // Calculate positions as percentages of the range
    const fromPosition = ((fromValue - min) / rangeDistance) * 100;
    const toPosition = ((toValue - min) / rangeDistance) * 100;

    // Apply the gradient
    controlSlider.style.background = `linear-gradient(
    to right,
    ${sliderColor} 0%,
    ${sliderColor} ${fromPosition}%,
    ${rangeColor} ${fromPosition}%, 
    ${rangeColor} ${toPosition}%, 
    ${sliderColor} ${toPosition}%, 
    ${sliderColor} 100%)`;
  }

  function setToggleAccessible(currentTarget: HTMLInputElement) {
    const toSlider = document.querySelector("#toSlider") as HTMLInputElement;
    if (Number(currentTarget.value) <= 0) {
      toSlider.style.zIndex = "2";
    } else {
      toSlider.style.zIndex = "0";
    }
  }

  function dispatchStartAndEndValues(from: number, to: number) {
    start = from;
    end = to;
    dispatch("change", { start, end });
  }
</script>

<div class="range_container">
  <p class="font-light text-center text-sm mb-2">{start} - {end} {unit}</p>

  <div class="w-full">
    <div class="sliders_control">
      <input
        bind:this={fromSlider}
        id="fromSlider"
        type="range"
        {min}
        {max}
        value={start}
        step="10"
      />
      <input
        bind:this={toSlider}
        id="toSlider"
        type="range"
        {min}
        {max}
        step="10"
        value={end}
      />
    </div>
    <div class="form_control">
      <div class="form_control_container">
        <div class="form_control_container__time">Min</div>
        <input
          bind:this={fromInput}
          class="form_control_container__time__input"
          type="number"
          id="fromInput"
          {min}
          {max}
          value={start}
        />
      </div>
      <div class="form_control_container">
        <div class="form_control_container__time">Max</div>
        <input
          bind:this={toInput}
          class="form_control_container__time__input"
          type="number"
          id="toInput"
          {min}
          {max}
          value={end}
        />
      </div>
    </div>
  </div>
</div>

<style>
  .range_container {
    display: flex;
    flex-direction: column;
    width: 80%;
    justify-content: center;
  }

  .sliders_control {
    position: relative;
    min-height: 50px;
  }

  .form_control {
    position: relative;
    display: none;
    justify-content: space-between;
    font-size: 24px;
    color: #635a5a;
  }

  input[type="range"]::-webkit-slider-thumb {
    -webkit-appearance: none;
    pointer-events: all;
    width: 24px;
    height: 24px;
    background-color: #fff;
    border-radius: 50%;
    box-shadow: 0 0 0 1px #c6c6c6;
    cursor: pointer;
  }

  input[type="range"]::-moz-range-thumb {
    -webkit-appearance: none;
    pointer-events: all;
    width: 20px;
    height: 20px;
    background-color: #fff;
    border-radius: 50%;
    box-shadow: 0 0 0 1px #c6c6c6;
    cursor: pointer;
  }

  input[type="range"]::-webkit-slider-thumb:hover {
    background: #f7f7f7;
  }

  input[type="range"]::-webkit-slider-thumb:active {
    box-shadow:
      inset 0 0 3px #387bbe,
      0 0 9px #387bbe;
    -webkit-box-shadow:
      inset 0 0 3px #387bbe,
      0 0 9px #387bbe;
  }

  input[type="number"] {
    color: #8a8383;
    width: 50px;
    height: 30px;
    font-size: 20px;
    border: none;
  }

  input[type="number"]::-webkit-inner-spin-button,
  input[type="number"]::-webkit-outer-spin-button {
    opacity: 1;
  }

  input[type="range"] {
    -webkit-appearance: none;
    appearance: none;
    height: 2px;
    width: 100%;
    position: absolute;
    background-color: #c6c6c6;
    pointer-events: none;
  }

  #fromSlider {
    height: 0;
    z-index: 1;
  }
</style>
