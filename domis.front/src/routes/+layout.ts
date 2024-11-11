import { userStore } from "../stores/user";

export const ssr = false;

export async function load() {
  userStore.initialize();
}
