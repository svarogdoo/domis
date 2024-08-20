import { writable, type Writable } from "svelte/store";

export const categories: Writable<Array<Category>> = writable([]);
