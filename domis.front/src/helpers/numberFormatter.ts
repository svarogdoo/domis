const formatter = new Intl.NumberFormat("en-US", {
  minimumFractionDigits: 2,
  maximumFractionDigits: 2,
});

export function formatPrice(value: number) {
  return formatter.format(value);
}

export function formatToTwoDecimals(value: number) {
  return Number(value.toFixed(2));
}
