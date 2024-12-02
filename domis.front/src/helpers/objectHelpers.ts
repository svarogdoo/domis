export function getUpdatedFields<T extends Record<string, any>>(
  original: T,
  updated: T
): Partial<T> {
  const changedFields: Partial<T> = {};

  for (const key in updated) {
    if (
      Object.prototype.hasOwnProperty.call(updated, key) &&
      original[key] !== updated[key]
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
