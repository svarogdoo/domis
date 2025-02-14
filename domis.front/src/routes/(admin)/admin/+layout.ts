// src/routes/admin/+layout.ts
import { redirect } from "@sveltejs/kit";
import { userStore } from "../../../stores/user";
import { getAdminOrders } from "../../../services/admin-service";

export const load = async ({ parent }) => {
  await parent();

  if (!userStore.isUserAdmin()) {
    throw redirect(303, "/zabranjen-pristup");
  }

  let orders = await getAdminOrders();

  return {
    props: {
      orders: orders,
    },
  };
};
