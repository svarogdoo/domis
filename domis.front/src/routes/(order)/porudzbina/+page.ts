import { userStore } from "../../../stores/user";

export async function load({ parent }) {
  await parent();

  let user = userStore.getUserData();

  let initAddress: Address = {
    country: "Srbija",
    county: "",
    city: "",
    addressLine: "",
    apartment: "",
    postalCode: "",
    contactPerson: "",
    contactPhone: "",
  };

  if (user === null)
    user = {
      firstName: "",
      lastName: "",
      email: "",
      phoneNumber: "",
      addressInvoice: initAddress,
      addressDelivery: initAddress,
      useSameAddress: true,
    };

  if (user.companyInfo === null)
    user.companyInfo = {
      name: "",
      number: "",
      firstName: "",
      lastName: "",
    };

  if (user.addressInvoice === null) user.addressInvoice = initAddress;
  if (user.addressDelivery === null) {
    user.addressDelivery = initAddress;
    user.useSameAddress = true;
  }

  return {
    props: {
      user: user,
    },
  };
}
