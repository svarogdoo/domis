import { writable } from "svelte/store";
import { userService } from "../services/user-service";
import { setAuthToken } from "../helpers/fetch";

const createUserStore = () => {
  const userInitialState = {
    isAuthenticated: false,
    user: null,
    token: null,
    refreshToken: null
  };
  const { subscribe, set, update } = writable<UserState>(userInitialState);

  const setUser = (user: UserProfileResponse, token: string, refreshToken: string) => {
    const userState: UserState = {
      isAuthenticated: true,
      user,
      token,
      refreshToken
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

        console.info(userState);
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

    // async refreshAccessToken(refreshToken: string){
    //   const loginResponse =  await userService.refreshAccessToken(refreshToken);
    //   const userProfile = await this.getProfile();
    //   setUser(userProfile, loginResponse.accessToken, loginResponse.refreshToken);
    // },

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
