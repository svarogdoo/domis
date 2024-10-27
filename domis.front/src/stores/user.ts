import { get, writable } from "svelte/store";
import { userService } from "../services/user-service";
import { setAuthToken } from "../helpers/fetch";
import { cart } from "./cart";
import { goto } from "$app/navigation";

const createUserStore = () => {
  const userInitialState = {
    isAuthenticated: false,
    user: null,
    token: null,
    refreshToken: null,
  };
  const { subscribe, set, update } = writable<UserState>(userInitialState);

  const setUser = (
    user: UserProfileResponse,
    token: string,
    refreshToken: string
  ) => {
    const userState: UserState = {
      isAuthenticated: true,
      user,
      token,
      refreshToken,
    };
    set(userState);
    localStorage.setItem("user", JSON.stringify(userState));
  };

  return {
    subscribe,
    set,
    update,

    async initialize() {
      const savedUser = localStorage.getItem("user");
      if (savedUser) {
        let userState = JSON.parse(savedUser);
        set(userState);
        setAuthToken(userState.token);
      } else {
        set(userInitialState);
        localStorage.setItem("user", JSON.stringify(userInitialState));
      }
    },

    async loginUser(email: string, password: string) {
      const loginResponse = await userService.login(email, password);
      const token = loginResponse.accessToken;
      const tokenType = loginResponse.tokenType;
      const expiresIn = loginResponse.expiresIn;
      const refreshToken = loginResponse.refreshToken;

      const userProfile = await this.getProfile();

      setUser(userProfile, token, refreshToken);
      cart.loginUser();

      return loginResponse;
    },

    async registerUser(request: UserRegisterRequest) {
      await userService.register(request);
      return this.loginUser(request.email, request.password);
    },

    async logoutUser() {
      set(userInitialState);
      localStorage.setItem("user", JSON.stringify(userInitialState));
      await cart.logoutUser();
      await goto("/");
    },

    async getProfile() {
      var user = await userService.getProfile();
      console.info(user);
      return user;
    },

    async updateProfile(request: UserProfileUpdateRequest) {
      return await userService.updateProfile(request);
    },

    async forgotPassword(email: string) {
      return await userService.forgotPassword(email);
    },

    async resetPassword(email: string, resetCode: string, newPassword: string) {
      return await userService.resetPassword(email, resetCode, newPassword);
    },

    refreshUserSession(newToken: UserLoginResponse) {
      update((state) => ({
        ...state,
        token: newToken.accessToken,
        refreshToken: newToken.refreshToken,
      }));
      const currentUser = get(userStore);
      localStorage.setItem("user", JSON.stringify(currentUser));
    },
  };
};

export const userStore = createUserStore();
