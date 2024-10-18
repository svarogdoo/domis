import { userStore } from "../stores/user";
import { goto } from "$app/navigation";

export const requireAuth = async () => {
  let isAuthenticated = false;
  const unsubscribe = userStore.subscribe((state) => {
    isAuthenticated = state.isAuthenticated;
  });

  if (!isAuthenticated) {
    goto("/login"); // Adjust this path as needed
  }

  unsubscribe();
};
