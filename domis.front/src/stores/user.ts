import { writable } from "svelte/store";
import { userService } from "../services/user-service";

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

  const setUser = (user: User, token: string) => {
    set({
      isAuthenticated: true,
      user,
      token,
    });
  };

  const setToken = (token: string) => {
    set({
      isAuthenticated: true,
      user: null,
      token,
    });
  };

  return {
    subscribe, //out-of-the-box
    set,
    update,

    async loginUser(email: string, password: string) {
      const loginResponse = await userService.login(email, password);
      const token = loginResponse.accessToken;
      const tokenType = loginResponse.tokenType;
      const expiresIn = loginResponse.expiresIn;
      const refreshToken = loginResponse.refreshToken;

      // TODO: Fetch user details after login
      // const userDetails = await fetchUserDetails();

      setToken(token);

      return loginResponse;
    },

    async registerUser(email: string, password: string) {
      await userService.register(email, password);   
      //log in user right after registering  
      return this.loginUser(email, password);
    },

    logoutUser() {
      set({
        isAuthenticated: false,
        user: null,
        token: null
      });
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
    }

  };
};

export const userStore = createUserStore();
