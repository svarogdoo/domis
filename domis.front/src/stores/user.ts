import { writable } from "svelte/store";
import { userService } from "../services/user-service";
import { setAuthToken } from "../helpers/fetch";
import { cart } from "./cart";
import { goto } from "$app/navigation";

const createUserStore = () => {
  const userInitialState = {
    isAuthenticated: false,
    user: null,
    token: null,
  };
  const { subscribe, set, update } = writable<UserState>(userInitialState);

  const setUser = (user: UserProfileResponse, token: string) => {
    const userState: UserState = {
      isAuthenticated: true,
      user,
      token,
    };
    set(userState);
    localStorage.setItem("user", JSON.stringify(userState));
  };

  return {
    subscribe, //out-of-the-box
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

      setUser(userProfile, token);
      cart.loginUser();

      return loginResponse;
    },

    async registerUser(request: UserRegisterRequest) {
      await userService.register(request);
      //log in user right after registering
      return this.loginUser(request.email, request.password);
    },

    async logoutUser() {
      set(userInitialState);
      localStorage.setItem("user", JSON.stringify(userInitialState));
      await cart.logoutUser();
      await goto("/");
      window.location.reload();
    },

    async getProfile() {
      return await userService.getProfile();
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

    //TODO: actually implement
    refreshUserSession(newToken: string) {
      update((state) => ({
        ...state,
        token: newToken,
      }));
    },
  };
};

export const userStore = createUserStore();
