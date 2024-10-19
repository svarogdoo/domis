import { userStore } from "../stores/user";
import { goto } from "$app/navigation";

export const requireAuth = async () => {
  let isAuthenticatedStore = false;

  const unsubscribe = userStore.subscribe((state) => {
    isAuthenticatedStore = state.isAuthenticated;
  });

  const savedUser = localStorage.getItem("user");
  let isAuthenticatedStorage = false;
  if (savedUser)
    isAuthenticatedStorage = JSON.parse(savedUser)?.isAuthenticated;

  if (!isAuthenticatedStorage && !isAuthenticatedStore) {
    goto("/login");
  }

  unsubscribe();
};
