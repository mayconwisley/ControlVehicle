import type { ReactNode } from "react";
import {
  Badge as BadgeIcon,
  Build as BuildIcon,
  DirectionsCar as DirectionsCarIcon,
  Gavel as GavelIcon,
  LocalGasStation as LocalGasStationIcon,
  Route as RouteIcon
} from "@mui/icons-material";

export type ResourceColumn = {
  field: string;
  headerName: string;
  render?: (value: unknown, row: Record<string, unknown>) => ReactNode;
};

export type ResourceFormFieldType = "text" | "number" | "date" | "datetime" | "checkbox" | "select" | "lookup";

export type ResourceLookupConfig = {
  endpoint: "Driver" | "Vehicle";
};

export type ResourceFormField = {
  name: string;
  label: string;
  type: ResourceFormFieldType;
  required?: boolean;
  options?: string[];
  lookup?: ResourceLookupConfig;
};

export type ResourceDefinition = {
  key: string;
  slug: string;
  title: string;
  route: string;
  endpoint: string;
  icon: ReactNode;
  deleteByField: string;
  columns: ResourceColumn[];
  formFields: ResourceFormField[];
};

const fuelOptions = ["Gasoline", "Diesel", "Ethanol", "Flex", "Hybrid", "Electric", "Hydrogen", "Cng", "Lpg"];
const colorOptions = ["White", "Black", "Gray", "Silver", "Blue", "Red", "Brown", "Green", "Beige"];
const categoryCnhOptions = ["A", "B", "C", "D", "E", "AB", "AC", "AD", "AE"];

const dateCell = (value: unknown) => {
  if (typeof value !== "string") {
    return "-";
  }

  const dateOnlyMatch = /^(\d{4})-(\d{2})-(\d{2})$/.exec(value.trim());
  if (dateOnlyMatch) {
    const [, year, month, day] = dateOnlyMatch;
    return `${day}/${month}/${year}`;
  }

  const parsed = new Date(value);
  if (Number.isNaN(parsed.getTime())) {
    return "-";
  }

  return parsed.toLocaleDateString("pt-BR");
};

const dateTimeCell = (value: unknown) => {
  if (typeof value !== "string") {
    return "-";
  }

  return new Date(value).toLocaleString("pt-BR");
};

const moneyCell = (value: unknown) => {
  if (typeof value !== "number") {
    return "-";
  }

  return value.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
};

const boolCell = (value: unknown) => (value ? "Ativo" : "Inativo");

export const resourceDefinitions: ResourceDefinition[] = [
  {
    key: "Driver",
    slug: "drivers",
    title: "Motoristas",
    route: "/drivers",
    endpoint: "Driver",
    icon: <BadgeIcon fontSize="small" />,
    deleteByField: "cnh",
    columns: [
      { field: "name", headerName: "Nome" },
      { field: "cnh", headerName: "CNH" },
      { field: "categoryCnh", headerName: "Categoria" },
      { field: "dateExpiration", headerName: "Validade CNH", render: dateCell },
      { field: "active", headerName: "Status", render: boolCell }
    ],
    formFields: [
      { name: "name", label: "Nome", type: "text", required: true },
      { name: "cnh", label: "CNH", type: "text", required: true },
      { name: "categoryCnh", label: "Categoria CNH", type: "select", options: categoryCnhOptions, required: true },
      { name: "dateExpiration", label: "Validade da CNH", type: "date", required: true },
      { name: "active", label: "Ativo", type: "checkbox" }
    ]
  },
  {
    key: "Vehicle",
    slug: "vehicles",
    title: "Veículos",
    route: "/vehicles",
    endpoint: "Vehicle",
    icon: <DirectionsCarIcon fontSize="small" />,
    deleteByField: "renavam",
    columns: [
      { field: "model", headerName: "Modelo" },
      { field: "licensePlate", headerName: "Placa" },
      { field: "renavam", headerName: "Renavam" },
      { field: "fuel", headerName: "Combustível" },
      { field: "active", headerName: "Status", render: boolCell }
    ],
    formFields: [
      { name: "model", label: "Modelo", type: "text", required: true },
      { name: "licensePlate", label: "Placa", type: "text", required: true },
      { name: "renavam", label: "Renavam", type: "text", required: true },
      { name: "fuel", label: "Combustível", type: "select", options: fuelOptions, required: true },
      { name: "chassi", label: "Chassi", type: "text" },
      { name: "vehicleColor", label: "Cor", type: "select", options: colorOptions, required: true },
      { name: "active", label: "Ativo", type: "checkbox" }
    ]
  },
  {
    key: "VehicleControl",
    slug: "vehicle-control",
    title: "Controle de uso",
    route: "/vehicle-control",
    endpoint: "VehicleControl",
    icon: <RouteIcon fontSize="small" />,
    deleteByField: "id",
    columns: [
      { field: "departureDate", headerName: "Saída", render: dateTimeCell },
      { field: "arrivalDate", headerName: "Retorno", render: dateTimeCell },
      { field: "initialKm", headerName: "KM inicial" },
      { field: "finalKm", headerName: "KM final" },
      { field: "description", headerName: "Descrição" }
    ],
    formFields: [
      { name: "vehicleId", label: "Veículo", type: "lookup", lookup: { endpoint: "Vehicle" }, required: true },
      { name: "driverId", label: "Motorista", type: "lookup", lookup: { endpoint: "Driver" }, required: true },
      { name: "departureDate", label: "Data de saída", type: "datetime", required: true },
      { name: "arrivalDate", label: "Data de retorno", type: "datetime", required: true },
      { name: "initialKm", label: "KM inicial", type: "number", required: true },
      { name: "finalKm", label: "KM final", type: "number", required: true },
      { name: "description", label: "Descrição", type: "text", required: true }
    ]
  },
  {
    key: "FuelControl",
    slug: "fuel-control",
    title: "Abastecimentos",
    route: "/fuel-control",
    endpoint: "FuelControl",
    icon: <LocalGasStationIcon fontSize="small" />,
    deleteByField: "id",
    columns: [
      { field: "date", headerName: "Data", render: dateTimeCell },
      { field: "liters", headerName: "Litros" },
      { field: "value", headerName: "Valor", render: moneyCell },
      { field: "initialKm", headerName: "KM" },
      { field: "description", headerName: "Descrição" }
    ],
    formFields: [
      { name: "vehicleId", label: "Veículo", type: "lookup", lookup: { endpoint: "Vehicle" }, required: true },
      { name: "driverId", label: "Motorista", type: "lookup", lookup: { endpoint: "Driver" }, required: true },
      { name: "date", label: "Data", type: "datetime", required: true },
      { name: "initialKm", label: "KM inicial", type: "number", required: true },
      { name: "liters", label: "Litros", type: "number", required: true },
      { name: "value", label: "Valor", type: "number", required: true },
      { name: "description", label: "Descrição", type: "text" }
    ]
  },
  {
    key: "TrafficFineControl",
    slug: "traffic-fine-control",
    title: "Multas",
    route: "/traffic-fine-control",
    endpoint: "TrafficFineControl",
    icon: <GavelIcon fontSize="small" />,
    deleteByField: "id",
    columns: [
      { field: "date", headerName: "Data", render: dateTimeCell },
      { field: "points", headerName: "Pontos" },
      { field: "value", headerName: "Valor", render: moneyCell },
      { field: "description", headerName: "Descrição" }
    ],
    formFields: [
      { name: "vehicleId", label: "Veículo", type: "lookup", lookup: { endpoint: "Vehicle" }, required: true },
      { name: "driverId", label: "Motorista", type: "lookup", lookup: { endpoint: "Driver" }, required: true },
      { name: "date", label: "Data", type: "datetime", required: true },
      { name: "points", label: "Pontos", type: "number", required: true },
      { name: "value", label: "Valor", type: "number", required: true },
      { name: "description", label: "Descrição", type: "text" }
    ]
  },
  {
    key: "MaintenanceControl",
    slug: "maintenance-control",
    title: "Manutenções",
    route: "/maintenance-control",
    endpoint: "MaintenanceControl",
    icon: <BuildIcon fontSize="small" />,
    deleteByField: "id",
    columns: [
      { field: "date", headerName: "Data", render: dateTimeCell },
      { field: "value", headerName: "Valor", render: moneyCell },
      { field: "description", headerName: "Descrição" }
    ],
    formFields: [
      { name: "vehicleId", label: "Veículo", type: "lookup", lookup: { endpoint: "Vehicle" }, required: true },
      { name: "date", label: "Data", type: "datetime", required: true },
      { name: "value", label: "Valor", type: "number", required: true },
      { name: "description", label: "Descrição", type: "text" }
    ]
  }
];
