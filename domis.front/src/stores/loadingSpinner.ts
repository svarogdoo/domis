import { writable } from "svelte/store";

function createLoadingSpinnerStore() {
  const { subscribe, set } = writable({ show: false });

  function showSpinner() {
    set({ show: true });
  }

  function hideSpinner() {
    set({ show: false });
  }

  return {
    subscribe,
    showSpinner,
    hideSpinner,
  };
}

export const loadingSpinnerStore = createLoadingSpinnerStore();
