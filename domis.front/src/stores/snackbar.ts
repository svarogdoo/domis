import { writable } from "svelte/store";

function createSnackbarStore() {
  const { subscribe, set, update } = writable({
    message: "",
    isSuccess: false,
    show: false,
  });

  function showSnackbar(message: string, isSuccess: boolean) {
    console.log("Snackbar triggered:", message, isSuccess);
    set({ message, isSuccess, show: true });
    setTimeout(() => {
      update((snackbar) => ({ ...snackbar, show: false }));
    }, 3000); // Close after 3s
  }

  return {
    subscribe,
    showSnackbar,
  };
}

export const snackbarStore = createSnackbarStore();
