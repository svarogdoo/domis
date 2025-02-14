import { get, writable, type Writable } from "svelte/store";

export const categories: Writable<Array<Category>> = writable([]);

export function getFlattenedCategories(): Array<FlattenedCategory> {
  return flattenCategories(get(categories));
}

function flattenCategories(
  categories: Array<Category>,
  level: number = 0
): Array<FlattenedCategory> {
  return categories.flatMap((category) => {
    const { id, name, subcategories } = category;

    // Add the current category with the given level
    const current = { id, name, level };

    // Recursively flatten subcategories, if any
    const subcategoriesFlattened = subcategories
      ? flattenCategories(subcategories, level + 1)
      : [];

    // Combine the current category with its flattened subcategories
    return [current, ...subcategoriesFlattened];
  });
}

export function getParentCategories(categoryId: string) {
  let allCategories = get(categories);

  return getParentCategoryNames(allCategories, categoryId);
  // ako je id isti kao id na kom sam -> true
  // ako nema subcategory -> false
  // ako ima subcategory -> idi na nizi nivo
}

function getParentCategoryNames(
  categories: Category[],
  categoryId: string,
  path: string[] = []
): string {
  for (const category of categories) {
    if (category.id === categoryId) {
      return path.length > 0 ? path.join(" / ") : "/";
    }

    if (category.subcategories) {
      const result = getParentCategoryNames(
        category.subcategories,
        categoryId,
        [...path, category.name]
      );
      if (result !== "/") {
        return result;
      }
    }
  }

  return "/";
}
