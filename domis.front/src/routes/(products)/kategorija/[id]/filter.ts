import { getCurrencyString } from "../../../../helpers/stringFormatter";

export const filters: Array<FilterData> = [
  {
    name: "price",
    displayName: "Cena",
    minValue: 100,
    maxValue: 1000,
    unit: getCurrencyString(),
  },
  {
    name: "width",
    displayName: "Širina",
    minValue: 100,
    maxValue: 1000,
    unit: "cm",
  },
  {
    name: "height",
    displayName: "Visina",
    minValue: 100,
    maxValue: 1000,
    unit: "cm",
  },
];
