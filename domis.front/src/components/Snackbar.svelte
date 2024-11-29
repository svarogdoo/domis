<script>
  export let message;
  export let isSuccess;
  function closeSnackbar() {
    var snackbar = document.getElementById("snackbar");
    if (snackbar) snackbar.style.display = "none";
  }
</script>

<div
  id="snackbar"
  class="{isSuccess
    ? 'bg-green-600'
    : 'bg-red-600'} text-white py-4 px-8 rounded-md fixed bottom-4 right-4 left-4 lg:left-auto flex justify-between items-center"
  style="display: none;"
>
  <p>{message}</p>
  <button class="text-white pl-4" on:click={() => closeSnackbar()}>
    <svg
      xmlns="http://www.w3.org/2000/svg"
      fill="none"
      viewBox="0 0 24 24"
      stroke="currentColor"
      class="w-4 h-4"
    >
      <path
        stroke-linecap="round"
        stroke-linejoin="round"
        stroke-width="2"
        d="M6 18L18 6M6 6l12 12"
      />
    </svg>
  </button>
</div>
