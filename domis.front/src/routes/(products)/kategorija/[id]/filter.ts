import { getCurrencyString } from "../../../../helpers/stringFormatter";

interface Filter {
  price: FilterData;
  width: FilterData;
  height: FilterData;
}

export const filters: Filter = {
  price: {
    displayName: "Cena",
    minValue: 0,
    maxValue: 5000,
    unit: getCurrencyString(),
  },
  width: {
    displayName: "Širina",
    minValue: 0,
    maxValue: 100,
    unit: "cm",
  },
  height: {
    displayName: "Visina",
    minValue: 0,
    maxValue: 100,
    unit: "cm",
  },
};
