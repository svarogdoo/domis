// src/routes/admin/+layout.ts
import { redirect } from "@sveltejs/kit";
import { userStore } from "../../../stores/user";

export const load = async ({ parent }) => {
  await parent();

  if (!userStore.isUserAdmin()) {
    throw redirect(303, "/zabranjen-pristup");
  }

  return {};
};
