export function getKutijaString(value: number) {
  if ([2, 3, 4].includes(value % 10) && Math.floor(value / 10) !== 1)
    return "kutije";
  else return "kutija";
}

export function shortenString(str: string, maxLength: number): string {
  if (str.length <= maxLength) {
    return str;
  }
  return str.slice(0, maxLength) + "...";
}
