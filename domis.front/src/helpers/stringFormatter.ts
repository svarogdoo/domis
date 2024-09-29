export function getKutijaString(value: number) {
  if ([2, 3, 4].includes(value % 10) && Math.floor(value / 10) !== 1)
    return "kutije";
  else return "kutija";
}
