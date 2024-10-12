import fallbackImage from "$lib/assets/backup.jpg";

export function handleImageError(event: Event) {
  (event.target as HTMLImageElement).src = fallbackImage;
}
