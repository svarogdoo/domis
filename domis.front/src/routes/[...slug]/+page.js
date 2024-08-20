export async function load({ params }) {
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