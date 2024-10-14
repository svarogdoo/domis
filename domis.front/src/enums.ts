import { text } from "@sveltejs/kit";

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
  quantityType: QuantityType,
  intValue?: number
): string {
  switch (quantityType) {
    case QuantityType.MeterSquared:
    case QuantityType.Meter:
      return "pak";
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

export enum PaymentType {
  Takeover = 1,
  BankPayment = 2,
  Online = 3,
}
export const paymentOptions = [
  {
    value: 1,
    label: "Plaćanje pouzećem",
  },
  {
    value: 2,
    label: "Uplata na račun",
    text: "Izvršite uplatu direktno na naš račun. Kao poziv na broj navedite broj svoje porudžbine, u suprotnom vaša porudžbina neće biti obrađena i isporučena.",
  },
];
