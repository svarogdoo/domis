import { writable } from "svelte/store";
import { login } from "../services/user-service";

interface UserState {
  isAuthenticated: boolean;
  user: User | null;
  token: string | null;
}

const createUserStore = () => {
  const { subscribe, set, update } = writable<UserState>({
    isAuthenticated: false,
    user: null,
    token: null
  });

  return {
    //out-of-box
    subscribe,
    set,
    update,

    async loginUser(email: string, password: string) {
      const loginResponse = await login(email, password);
      const token = loginResponse.accessToken;
      const tokenType = loginResponse.tokenType;
      const expiresIn = loginResponse.expiresIn;
      const refreshToken = loginResponse.refreshToken;

      // TODO: Fetch user details after login
      // const userDetails = await fetchUserDetails();

      set({
        isAuthenticated: true,
        user: null, // replace with actual user when details are ready
        token
      });

      return loginResponse;
    },

    logoutUser() {
      set({
        isAuthenticated: false,
        user: null,
        token: null
      });
    },

    async registerUser(userName: string, password: string) {
      const response = await fetch("https://example.com/api/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ userName, password }),
      });

      if (response.ok) {
        const userData = await response.json();
        return this.loginUser(userData.userName, userData.password);
      } else {
        throw new Error("Registration failed");
      }
    },

    async requestPasswordReset(email: string) {
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
    },

    refreshUserSession(newToken: string) {
      update((state) => ({
        ...state,
        token: newToken
      }));
    },

    setUser(user: User, token: string) {
      set({
        isAuthenticated: true,
        user,
        token
      });
    },

    setToken(token: string) {
      set({
        isAuthenticated: true,
        user: null,
        token
      });
    }
  };
};

export const userStore = createUserStore();