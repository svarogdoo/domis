export enum QuantityType {
  None = 1,
  MeterSquared = 2,
  Meter = 3,
  Piece = 4,
}
export function mapQuantityTypeToString(quantityType: QuantityType): string {
  switch (quantityType) {
    case QuantityType.MeterSquared:
      return "m²";
    case QuantityType.Meter:
      return "m";
    case QuantityType.Piece:
      return "kom";
    default:
      return "kom";
  }
}

export function mapQuantityTypeToCartString(
  quantityType: QuantityType
): string {
  switch (quantityType) {
    case QuantityType.MeterSquared:
    case QuantityType.Meter:
      return "kutija";
    case QuantityType.Piece:
      return "kom";
    default:
      return "kom";
  }
}

export const quantityTypeOptions = [
  {
    value: 4,
    label: "Komad",
  },
  {
    value: 3,
    label: "m",
  },
  {
    value: 2,
    label: "m²",
  },
];
