import { redirect } from "@sveltejs/kit";
import { userStore } from "../../../../stores/user";

export async function load({ parent }) {
  await parent();

  let user = userStore.getUserData();

  if (user === null) throw redirect(302, "/login");

  return {
    props: {
      user: user as NonNullable<typeof user>,
      userRole: userStore.isUserVP() ? userStore.getUserRole() : null,
    },
  };
}
