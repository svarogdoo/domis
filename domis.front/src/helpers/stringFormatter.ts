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
