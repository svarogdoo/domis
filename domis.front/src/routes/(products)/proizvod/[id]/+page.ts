import { getProduct } from "../../../../services/product-service.js";
import { userStore } from "../../../../stores/user.js";

export async function load({ params, parent }) {
  await parent(); // wait for root and user initialize

  const productId = params.id;
  let productPrice: ProductPricing;

  const product = await getProduct(Number.parseInt(productId));

  if (userStore.isUserVP()) productPrice = product.vpPrice;
  else productPrice = product.price;

  if (product.size === null)
    product.size = {
      box: 1,
      pallet: null,
    };

  return {
    props: {
      product,
      productPrice,
    },
  };
}
