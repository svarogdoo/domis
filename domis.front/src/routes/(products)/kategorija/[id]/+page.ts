import { SortType } from "../../../../enums.js";
import {
  getCategoryProducts,
  setCategories,
} from "../../../../services/category-service.js";

export async function load({ params, url, parent }) {
  await parent();

  setCategories();

  let categoryId = params.id;
  const pageNumber = url.searchParams.get("strana") || "1";
  const pageSize = url.searchParams.get("velicina") || "18";

  if (categoryId === "akcija") categoryId = "sale";

  let categoryData = await getCategoryProducts(
    categoryId === "sale" ? categoryId : Number.parseInt(categoryId),
    Number.parseInt(pageNumber),
    Number.parseInt(pageSize),
    SortType.NameAsc
  );

  return {
    props: {
      category: categoryData.category,
      products: categoryData.products,
    },
  };
}
