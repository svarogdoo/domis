import { userStore } from "../../../../stores/user";

export async function load({ parent }) {
  await parent();

  let user = userStore.getUserData();

  return {
    props: {
      user: user,
    },
  };
}
