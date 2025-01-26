<script lang="ts">
  import { createEventDispatcher } from "svelte";
  import SaleHistoryItem from "./SaleHistoryItem.svelte";
  import { postProductOnSale } from "../../../../../services/product-service";
  import DatePicker from "../../../../../components/DatePicker.svelte";
  import Toggle from "../../../../../components/Toggle.svelte";
  import Input from "../Input.svelte";
  import SaleHistoryMobileItem from "./SaleHistoryMobileItem.svelte";
  import { snackbarStore } from "../../../../../stores/snackbar";

  export let saleHistory: Array<SaleInfo> | null;
  export let initialPrice: number | undefined;
  export let productId: number;

  const dispatch = createEventDispatcher();

  let salePrice: number = initialPrice ?? 0;
  let salePercentage: number;
  let usePercentage: boolean = true;
  let startDate: Date = new Date();
  let hasSaleEndDate: boolean = true;
  let endDate: Date = new Date();

  let errors = {
    salePrice: "",
    salePercentage: "",
  };

  async function submitSale() {
    let saleInfo = {
      productIds: [productId],
      salePrice: usePercentage ? null : salePrice,
      salePercentage: usePercentage ? salePercentage : null,
      startDate: startDate.toISOString(),
      endDate: hasSaleEndDate ? endDate.toISOString() : null,
    };

    let res = await postProductOnSale(saleInfo);

    if (res) {
      snackbarStore.showSnackbar("Uspešno aktiviran popust!", true);
      dispatch("save");
    } else {
      snackbarStore.showSnackbar("Greška pri aktiviranju popusta!", false);
    }
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

  const handleStartDateChanged = (event: CustomEvent<string>) => {
    startDate = new Date(event.detail);
    if (endDate.getTime() < startDate.getTime()) {
      endDate = new Date(startDate.getTime());
    }
  };
  const handleEndDateChanged = (event: CustomEvent<string>) => {
    endDate = new Date(event.detail);
  };
</script>

<div class="flex flex-col gap-y-2 lg:gap-y-6 lg:px-8">
  <div
    id="sale-form"
    class="mt-1 mb-2 flex flex-col gap-y-2 lg:gap-y-6 p-2 rounded-lg bg-grey-50"
  >
    <div class="flex flex-wrap gap-y-3 items-center gap-x-6">
      <DatePicker
        on:change={handleStartDateChanged}
        dateString={startDate.toLocaleString()}
        title="Početak akcije"
      />
      <div class="mt-6">
        <Toggle
          bind:isActive={hasSaleEndDate}
          activeText="Akcija ima kraj"
          inActiveText="Trajno na akciji"
        />
      </div>
      {#if hasSaleEndDate}
        <DatePicker
          on:change={handleEndDateChanged}
          dateString={endDate.toLocaleString()}
          min={new Date(startDate).toLocaleString().split("T")[0]}
          title="Kraj akcije"
        />
      {/if}
    </div>
    <div class="flex flex-wrap gap-y-3 gap-x-2 lg:gap-x-6 items-center">
      {#if usePercentage}
        <Input
          bind:value={salePercentage}
          type="number"
          suffix="%"
          placeholder="0"
          width="16"
          gap="32"
          error={errors?.salePercentage}
        />
      {:else}
        <Input
          bind:value={salePrice}
          type="number"
          suffix="RSD"
          placeholder="0.00"
          width="28"
          gap="32"
          error={errors?.salePrice}
        />
      {/if}
      <Toggle
        bind:isActive={usePercentage}
        activeText="Koristi procente"
        inActiveText="Koristi fiksnu cenu"
      />
      <button
        on:click={submitSale}
        class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm max-w-96 px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
      >
        Postavi novu akciju
      </button>
    </div>
  </div>

  {#if saleHistory && saleHistory.length > 0}
    <table class="hidden lg:table table-hover">
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
    <table class="lg:hidden table table-hover">
      <thead class="w-full bg-domis-dark text-white">
        <th class="text-start w-32">Istorija popusta</th>
      </thead>
      <tbody class="divide-y divide-gray-200">
        {#each saleHistory as sale, index (index)}
          <SaleHistoryMobileItem on:save={handleDeactivatedSale} {sale} />
        {/each}
      </tbody>
    </table>
  {/if}
</div>

<style>
  table {
    box-shadow: 0px 4px 18px 2.25px rgba(0, 0, 0, 0.18);
  }
  th {
    padding: 8px 12px;
  }
</style>
