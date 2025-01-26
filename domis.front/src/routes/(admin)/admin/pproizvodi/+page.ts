import { getCategoryProductsBasicInfo } from "../../../../services/product-service";

export async function load({ parent }) {
  await parent();

  // get all products
  let products = await getCategoryProductsBasicInfo("701");

  return {
    props: {
      products: products,
    },
  };
}
