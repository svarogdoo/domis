import { userService } from "../../../../services/user-service";

export async function load({ parent }) {
  await parent();

  let orders = await userService.getUserOrders();

  return {
    props: {
      orders: orders,
    },
  };
}
