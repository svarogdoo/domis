import { initFetch } from "../helpers/fetch";
import { setCategories } from "../services/category-service";
import { userStore } from "../stores/user";

export const ssr = false;

export async function load({ fetch }) {
  initFetch(fetch);
  await userStore.initialize();
  setCategories();
}
