import { setCategories } from "../../services/category-service.js";

export async function load({ params }) {
  setCategories();

  const slugParts = params?.slug?.split("/");
  if (!slugParts) {
    return;
  }
  const lastSlug = slugParts[slugParts.length - 1];

  if (lastSlug === "product") {
    return {
      type: "product",
    };
  } else {
    return {
      type: "category",
    };
  }
}
