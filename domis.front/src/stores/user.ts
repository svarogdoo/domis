import { writable } from "svelte/store";
import { userService } from "../services/user-service";

const createUserStore = () => {
  const { subscribe, set, update } = writable<UserState>({
    isAuthenticated: false,
    user: null,
    token: null,
  });

  const setUser = (user: UserProfileResponse, token: string) => {
    set({
      isAuthenticated: true,
      user,
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

      const userProfile = await this.getProfile();

      setUser(userProfile, token);

      return loginResponse;
    },

    async registerUser(request: UserRegisterRequest) {
      await userService.register(request);
      //log in user right after registering
      return this.loginUser(request.email, request.password);
    },

    async logoutUser() {
      set({
        isAuthenticated: false,
        user: null,
        token: null,
      });
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

    async resetPassword(email: string, resetCode: string, newPassword: string){
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
