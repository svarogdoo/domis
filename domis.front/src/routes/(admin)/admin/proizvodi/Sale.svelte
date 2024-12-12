<script lang="ts">
  import { createEventDispatcher } from "svelte";
  import DatePicker from "../../../../components/DatePicker.svelte";
  import Snackbar from "../../../../components/Snackbar.svelte";
  import Toggle from "../../../../components/Toggle.svelte";
  import { postProductOnSale } from "../../../../services/product-service";
  import SaleHistoryItem from "./SaleHistoryItem.svelte";
  import Input from "./specifikacija/Input.svelte";
  import DateDropdownPicker from "../../../../components/DateDropdownPicker.svelte";
  import DatePickerTwo from "../../../../components/DatePickerTwo.svelte";

  export let saleHistory: Array<SaleInfo> | null;
  export let initialPrice: number | undefined;
  export let productId: number;

  const dispatch = createEventDispatcher();

  let snackbarMessage: string;
  let isSnackbarSuccess: boolean;
  let showSnackbar = false;

  let salePrice: number = initialPrice ?? 0;
  let startDate: Date = new Date();
  let hasSaleEndDate: boolean = true;
  let endDate: Date = new Date();

  let errors = {
    salePrice: "",
  };

  async function submitSale() {
    let saleInfo = {
      productIds: [productId],
      salePrice: salePrice,
      startDate: startDate.toISOString(),
      endDate: hasSaleEndDate ? endDate.toISOString() : null,
    };

    let res = await postProductOnSale(saleInfo);

    if (res) {
      snackbarMessage = "Uspešno aktiviran popust!";
      isSnackbarSuccess = true;
      dispatch("save");
    } else {
      snackbarMessage = "Greška pri aktiviranju popusta!";
      isSnackbarSuccess = false;
    }
    handleShowSnackbar();
  }

  async function handleDeactivatedSale(event: any) {
    if (event.detail.success) {
      snackbarMessage = "Uspešno deaktiviran popust!";
      isSnackbarSuccess = true;
      dispatch("save");
    } else {
      snackbarMessage = "Greška pri deaktiviranju popusta!";
      isSnackbarSuccess = false;
    }
    handleShowSnackbar();
  }

  function handleShowSnackbar() {
    showSnackbar = true;
    setTimeout(function () {
      showSnackbar = false;
    }, 3000); // Close after 3s
  }

  const handleStartDateChanged = (event: CustomEvent<string>) => {
    startDate = new Date(event.detail);
    if (endDate.getTime() < startDate.getTime()) {
      endDate = new Date(startDate.getTime());
    }

    console.info(
      "start",
      startDate,
      startDate.toISOString(),
      endDate.toLocaleString()
    );
  };
  const handleEndDateChanged = (event: CustomEvent<string>) => {
    endDate = new Date(event.detail);
    console.info(
      "end",
      endDate,
      endDate.toISOString(),
      endDate.toLocaleString()
    );
  };
</script>

<div class="flex flex-col gap-y-6 px-8">
  <div
    id="sale-form"
    class="mt-1 mb-2 flex flex-col gap-y-6 p-2 rounded-lg bg-grey-50"
  >
    <div class="flex flex-wrap gap-y-3 items-center gap-x-6">
      <DatePickerTwo
        on:change={handleStartDateChanged}
        dateString={startDate.toLocaleString()}
        title="Početak akcije"
      />
      <Toggle
        bind:isActive={hasSaleEndDate}
        activeText="Akcija ima kraj"
        inActiveText="Trajno na akciji"
      />
      {#if hasSaleEndDate}
        <DatePickerTwo
          on:change={handleEndDateChanged}
          dateString={endDate.toLocaleString()}
          min={new Date(startDate).toLocaleString().split("T")[0]}
          title="Kraj akcije"
        />
      {/if}
    </div>
    <div class="flex gap-x-6 items-center">
      <Input
        bind:value={salePrice}
        type="number"
        title="Iznos na popustu"
        suffix="RSD"
        placeholder="0.00"
        width="32"
        gap="32"
        error={errors?.salePrice}
      />
      <button
        on:click={submitSale}
        class="text-white py-2 px-4 rounded tracking-widest bg-green-600"
      >
        Postavi novu akciju
      </button>
    </div>
  </div>

  {#if saleHistory && saleHistory.length > 0}
    <table class="table table-hover">
      <thead class="w-full bg-domis-dark text-white">
        <th class="text-start w-32">Cena</th>
        <th class="text-center w-32">Datum početka</th>
        <th class="text-center w-32">Datum kraja</th>
        <th class="text-center w-32">Aktivan</th>
      </thead>
      <tbody class="divide-y divide-gray-200">
        {#each saleHistory as sale, index (index)}
          <SaleHistoryItem on:save={handleDeactivatedSale} {sale} />
        {/each}
      </tbody>
    </table>
  {/if}

  <Snackbar
    message={snackbarMessage}
    isSuccess={isSnackbarSuccess}
    {showSnackbar}
  />
</div>

<style>
  table {
    box-shadow: 0px 4px 18px 2.25px rgba(0, 0, 0, 0.18);
  }
  th {
    padding: 8px 12px;
  }
</style>
