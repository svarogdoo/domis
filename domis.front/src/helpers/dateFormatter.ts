export function formatDate(date: Date): string {
  return date.toLocaleString("default", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit",
    hour12: false, // Ensures 24-hour format (removes AM/PM)
  });
}
