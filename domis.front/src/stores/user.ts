// src/stores/userStore.ts
import { writable } from "svelte/store";
import { login } from "../services/user-service";

const { subscribe, set, update } = writable<User | null>(null);

// export const loginUser = async (userName: string, password: string) => {
//   const response = await login(userName, password);
//   const { tokenType, accessToken, expiresIn, refreshToken  } = response;
//   const userData: User = {
//     userName,
//     tokenType,
//     accessToken,
//     expiresIn,
//     refreshToken
//   };

//   set(userData);
// }


export async function loginUser(email: string, password: string) {
    const loginResponse = await login(email, password);
    const token = loginResponse.accessToken;
    const tokenType = loginResponse.tokenType;
    const expiresIn = loginResponse.expiresIn;
    const refreshToken = loginResponse.refreshToken;

    //TODO: should send another call to API to get user details

    setToken(token); //TODO: call setUser when user details ready

    return loginResponse;
}

export const logoutUser = () => {
  userStore.set({
    isAuthenticated: false,
    user: null,
    token: null
  });
};

//TODO
export const registerUser = async (userName: string, password: string) => {
  const response = await fetch("https://example.com/api/register", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ userName, password }),
  });

  if (response.ok) {
    const userData = await response.json();
    login(userData.userName, userData.password);
  } else {
    throw new Error("Registration failed");
  }
};

//TODO
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

//TODO
export const refreshUserSession = (newToken: string) => {
  update((user) => {
    if (user) {
      return { ...user, accessToken: newToken };
    }
    return user;
  });
};

// export const userStore = {
//   subscribe,
//   loginUser: login,
//   logoutUser,
//   registerUser,
//   requestPasswordReset,
//   refreshUserSession,
// };

export const userStore = writable<UserState>();

export function setUser(user: User, token: string) {
  userStore.set({
      isAuthenticated: true,
      user,
      token
  });
}

//TODO: remove when setUser is ready
//TODO: also, decide what to set in the userStore and how does userStore actually look
export function setToken(token: string) {
  userStore.set({
      isAuthenticated: true,
      user: null,
      token
  });
}