import { userStore } from "../stores/user";

export function getPaketString(value: number) {
  if (1 === value % 10 && Math.floor(value / 10) !== 1) return "paket";
  else return "paketa";
}

export function shortenString(str: string, maxLength: number): string {
  if (str.length <= maxLength) {
    return str;
  }
  return str.slice(0, maxLength) + "...";
}

export function stringDateToString(dateString: string): string {
  const date = new Date(dateString);
  const day = String(date.getDate()).padStart(2, "0");
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const year = date.getFullYear();

  return `${day}.${month}.${year}.`;
}

export function getCurrencyString(): string {
  if (userStore.isUserVP()) return "EUR";
  return "RSD";
}
