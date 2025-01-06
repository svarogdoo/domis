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

  const minPrice = url.searchParams.get("minPrice") || undefined;
  const maxPrice = url.searchParams.get("maxPrice") || undefined;
  const minWidth = url.searchParams.get("minWidth") || undefined;
  const maxWidth = url.searchParams.get("maxWidth") || undefined;
  const minHeight = url.searchParams.get("minHeight") || undefined;
  const maxHeight = url.searchParams.get("maxHeight") || undefined;

  if (categoryId === "akcija") categoryId = "sale";

  let categoryData = await getCategoryProducts(
    categoryId === "sale" ? categoryId : Number.parseInt(categoryId),
    Number.parseInt(pageNumber),
    Number.parseInt(pageSize),
    SortType.NameAsc,
    minPrice,
    maxPrice,
    minWidth,
    maxWidth,
    minHeight,
    maxHeight
  );

  return {
    props: {
      category: categoryData.category,
      products: categoryData.products,
      sort: SortType.NameAsc,
    },
  };
}
