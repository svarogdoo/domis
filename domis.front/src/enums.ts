// Quantity Type
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
    value: 1,
    label: "Neodabrano",
  },
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

// Payment Vendor
export enum PaymentVendorType {
  OnDelivery = 1,
  BankPayment = 2,
  Card = 3,
}
export function mapPaymentVendorTypeToString(
  paymentVendorType: PaymentVendorType
): string {
  switch (paymentVendorType) {
    case PaymentVendorType.OnDelivery:
      return "Pouzećem";
    case PaymentVendorType.BankPayment:
      return "Uplata na račun";
    case PaymentVendorType.Card:
      return "Kartično";
  }
}
export const paymentVendorOptions = [
  {
    value: 1,
    label: "Plaćanje pouzećem",
  },
  // {
  //   value: 2,
  //   label: "Uplata na račun",
  //   text: "Izvršite uplatu direktno na naš račun. Kao poziv na broj navedite broj svoje porudžbine, u suprotnom vaša porudžbina neće biti obrađena i isporučena.",
  // },
];

// Order Status
export enum OrderStatus {
  New = 1,
  InProgress = 2,
  Sent = 3,
  Completed = 4,
  Canceled = 5,
}
export function mapOrderStatusToString(orderStatus: OrderStatus): string {
  switch (orderStatus) {
    case OrderStatus.New:
      return "U obradi";
    case OrderStatus.InProgress:
      return "Prihvaćena";
    case OrderStatus.Sent:
      return "Poslato";
    case OrderStatus.Completed:
      return "Završena";
    case OrderStatus.Canceled:
      return "Poništena";
  }
}
export function getOrderStatusColor(orderStatus: OrderStatus): string {
  switch (orderStatus) {
    case OrderStatus.New:
      return "bg-blue-200";
    case OrderStatus.InProgress:
      return "bg-blue-200";
    case OrderStatus.Sent:
      return "bg-blue-200";
    case OrderStatus.Completed:
      return "bg-green-200";
    case OrderStatus.Canceled:
      return "bg-red-200";
  }
}

// Sort types
export enum SortType {
  None = 0,
  PriceAsc = 1,
  PriceDesc = 2,
  NameAsc = 3,
  NameDesc = 4,
}
export function mapSortTypeToString(sortType: SortType): string {
  switch (sortType) {
    case SortType.PriceAsc:
      return "Cena rastuće";
    case SortType.PriceDesc:
      return "Cena opadajuće";
    case SortType.NameAsc:
      return "Naziv rastuće";
    case SortType.NameDesc:
      return "Naziv opadajuće";
    case SortType.None:
      return "Bez sortiranja";
  }
}

export enum AddressType {
  None,
  Invoice,
  Delivery,
}
