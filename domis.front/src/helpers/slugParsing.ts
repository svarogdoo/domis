export function getLastSlug(slug: string) {
  const slugParts = slug?.split("/");
  if (!slugParts) {
    return;
  }
  return slugParts[slugParts.length - 1];
}
