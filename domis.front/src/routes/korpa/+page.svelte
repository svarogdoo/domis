<script lang="ts">
  const tableList = [
    {
      image: "test",
      name: "test",
      price: "test",
      amount: "test",
      total: "test",
    },
    {
      image: "test",
      name: "test",
      price: "test",
      amount: "test",
      total: "test",
    },
    {
      image: "test",
      name: "test",
      price: "test",
      amount: "test",
      total: "test",
    },
    {
      image: "test",
      name: "test",
      price: "test",
      amount: "test",
      total: "test",
    },
  ];
</script>

<section class="w-full flex flex-col">
  <h1>Korpa</h1>
  <div class="w-full flex justify-around gap-x-4">
    <table class="w-2/4 table table-hover">
      <thead class="w-full bg-black text-white">
        <th>Slika</th>
        <th>Opis</th>
        <th>Cena</th>
        <th>Količina</th>
        <th>Ukupno</th>
      </thead>
      <tbody>
        {#each tableList as row}
          <tr>
            <td>{row.image}</td>
            <td>{row.name}</td>
            <td>{row.price}</td>
            <td>{row.amount}</td>
            <td>{row.total}</td>
          </tr>
        {/each}
      </tbody>
    </table>

    <div class="order-card w-1/4 flex flex-col border-l-2 px-8 py-4">
      <h2 class="text-2xl font-bold tracking-widest mt-2 text-center">
        Porudžbina
      </h2>
      <div class="flex justify-between mt-20 font-light">
        <p>Ukupno</p>
        <p>1150,00 RSD</p>
      </div>
      <div class="w-full h-0.5 bg-black my-4"></div>
      <div class="flex justify-between text-lg">
        <p>Ukupno</p>
        <p>1150,00 RSD</p>
      </div>
      <button class="text-light bg-black text-white mt-20 py-2 px-4 rounded-lg"
        >PLATI</button
      >
    </div>
  </div>
</section>

<style>
  table {
    box-shadow: 0px 4px 18px 2.25px rgba(0, 0, 0, 0.18);
  }
  th {
    padding: 8px 0px;
  }
  td {
    padding: 6px 0px;
  }

  .order-card {
    box-sizing: border-box;

    border: 1px solid #d9d9d9;
    box-shadow: 0px 4px 18px 2.25px rgba(0, 0, 0, 0.18);
    border-radius: 22px;
  }

  button {
    letter-spacing: 0.24em;
  }
</style>
