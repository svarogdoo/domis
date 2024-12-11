<script lang="ts">
  import DatePicker from "../../../../components/DatePicker.svelte";
  import Toggle from "../../../../components/Toggle.svelte";
  import Input from "./specifikacija/Input.svelte";

  export let saleInfo: SaleInfo | null;

  let isOnSale: boolean = false;
  let saleStartDate: Date = new Date();
  let hasSaleEndDate: boolean = true;
  let saleEndDate: Date;
  let salePrice: number;

  let errors = {
    salePrice: "",
  };
</script>

<div>
  <Toggle
    bind:isActive={isOnSale}
    activeText="Proizvod je na popustu"
    inActiveText="Proizvod nije na popustu"
  />
  <!-- Popust -->
  {#if isOnSale}
    <div class="mt-1 mb-2 flex flex-col gap-y-6 border rounded-lg p-4">
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

      <div class="flex items-center gap-x-6">
        <DatePicker bind:date={saleStartDate} title="Početak akcije" />
        <Toggle
          bind:isActive={hasSaleEndDate}
          activeText="Akcija ima kraj"
          inActiveText="Trajno na akciji"
        />
        {#if hasSaleEndDate}
          <DatePicker bind:date={saleEndDate} title="Kraj akcije" />
        {/if}
      </div>
    </div>
  {/if}
</div>

<!-- 

    let saleInfo = {
      salePrice: salePrice,
      startDate: saleStartDate,
      endDate: hasSaleEndDate ? saleEndDate : null,
    };


if (
      isOnSale &&
      (salePrice !== selectedProduct?.saleInfo?.salePrice ||
        saleStartDate !== new Date(selectedProduct.saleInfo.startDate) ||
        saleEndDate !== new Date(selectedProduct.saleInfo.endDate))
    ) {
      let saleRes = await postProductOnSale({
        productIds: [selectedProduct?.id],
        salePrice: salePrice,
        startDate: saleStartDate,
        endDate: saleEndDate,
      });
    } -->
