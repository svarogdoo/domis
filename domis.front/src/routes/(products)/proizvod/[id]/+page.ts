import { getProduct } from "../../../../services/product-service.js";

export async function load({ params }) {
  const productId = params.id;

  const product = await getProduct(Number.parseInt(productId));

  return {
    props: {
      product,
    },
  };
}
