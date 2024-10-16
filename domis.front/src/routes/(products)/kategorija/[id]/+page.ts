import { getCategoryProducts } from "../../../../services/category-service.js";

export async function load({ params }) {
  const categoryId = params.id;

  let categoryData = await getCategoryProducts(Number.parseInt(categoryId));

  return {
    props: {
      category: categoryData.category,
      products: categoryData.products,
    },
  };
}
