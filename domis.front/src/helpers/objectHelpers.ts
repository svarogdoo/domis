export function getUpdatedFields<T extends Record<string, any>>(
  original: T | null,
  updated: T
): Partial<T> {
  const changedFields: Partial<T> = {};

  // Treat original as an empty object if it's null
  const baseOriginal = original || ({} as T);

  for (const key in updated) {
    if (
      Object.prototype.hasOwnProperty.call(updated, key) &&
      baseOriginal[key] !== updated[key]
    ) {
      changedFields[key] = updated[key];
    }
  }

  return changedFields;
}

export function hasAnyFieldSet(
  obj: Record<string, any>,
  ignoreKeys: string[] = []
): boolean {
  return Object.entries(obj).some(
    ([key, value]) =>
      !ignoreKeys.includes(key) &&
      value !== null &&
      value !== undefined &&
      value !== ""
  );
}
