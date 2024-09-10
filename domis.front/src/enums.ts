export enum QuantityType {
  MeterSquared,
  Meter,
  Piece,
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
