import { SortType } from "../../../../enums.js";
import { getCategoryProducts } from "../../../../services/category-service.js";

export async function load({ params, url, parent }) {
  await parent();

  const categoryId = params.id;
  const pageNumber = url.searchParams.get("strana") || "1";
  const pageSize = url.searchParams.get("velicina") || "18";

  let categoryData = await getCategoryProducts(
    Number.parseInt(categoryId),
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
