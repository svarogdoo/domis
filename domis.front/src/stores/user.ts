// src/stores/userStore.ts
import { writable } from "svelte/store";

const { subscribe, set, update } = writable<User | null>(null);

export const loginUser = (userData: User) => {
  set(userData);
};

export const logoutUser = () => {
  set(null);
};

export const registerUser = async (userName: string, password: string) => {
  const response = await fetch("https://example.com/api/register", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ userName, password }),
  });

  if (response.ok) {
    const userData = await response.json();
    loginUser(userData);
  } else {
    throw new Error("Registration failed");
  }
};

export const requestPasswordReset = async (email: string) => {
  const response = await fetch(
    "https://example.com/api/request-password-reset",
    {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email }),
    }
  );

  if (!response.ok) {
    throw new Error("Password reset request failed");
  }
};

export const refreshUserSession = (newToken: string) => {
  update((user) => {
    if (user) {
      return { ...user, accessToken: newToken };
    }
    return user;
  });
};

export const userStore = {
  subscribe,
  loginUser,
  logoutUser,
  registerUser,
  requestPasswordReset,
  refreshUserSession,
};
