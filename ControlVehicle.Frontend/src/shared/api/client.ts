import type { PaginatedResponse } from "../types/api";

const API_BASE_URL = import.meta.env.VITE_API_URL ?? "https://localhost:7096/api/v1";

type QueryParams = Record<string, string | number | undefined>;
type HttpMethod = "POST" | "PUT" | "DELETE";

const normalizeText = (value: string) => value.normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLowerCase();

const mapKnownErrorMessage = (title?: string, detail?: string) => {
  const normalizedTitle = title ? normalizeText(title) : "";
  const normalizedDetail = detail ? normalizeText(detail) : "";

  if (
    normalizedTitle.includes("validacao de cnh") ||
    normalizedDetail.includes("cnh do motorista esta vencida")
  ) {
    return "Nao foi possivel registrar o controle. A CNH do motorista esta vencida na data atual. Renove a CNH ou atualize o cadastro do motorista com a nova validade.";
  }

  if (normalizedDetail.includes("data de validade da cnh esta vencida")) {
    return "Nao foi possivel salvar o motorista. A data de validade da CNH esta vencida. Informe uma validade atualizada.";
  }

  return null;
};

const buildUrl = (path: string, query?: QueryParams) => {
  const url = new URL(`${API_BASE_URL}/${path}`);

  if (query) {
    Object.entries(query).forEach(([key, value]) => {
      if (value !== undefined) {
        url.searchParams.set(key, String(value));
      }
    });
  }

  return url.toString();
};

const extractItems = <TItem>(payload: unknown): TItem[] => {
  if (!payload || typeof payload !== "object") {
    return [];
  }

  const possibleListKeys = ["driverList", "vehicleList", "controlList"] as const;

  for (const key of possibleListKeys) {
    const value = (payload as Record<string, unknown>)[key];
    if (Array.isArray(value)) {
      return value as TItem[];
    }
  }

  return [];
};

export const getResourcePage = async <TItem>(
  resource: string,
  page: number,
  size: number,
  search: string
): Promise<PaginatedResponse<TItem>> => {
  const response = await fetch(buildUrl(resource, { page, size, search }), {
    headers: {
      Accept: "application/json"
    }
  });

  if (response.status === 404) {
    return {
      totalData: 0,
      page,
      totalPage: 1,
      size,
      items: []
    };
  }

  if (!response.ok) {
    throw new Error(`Falha ao consultar ${resource}.`);
  }

  const payload = (await response.json()) as Record<string, unknown>;

  return {
    totalData: Number(payload.totalData ?? 0),
    page: Number(payload.page ?? page),
    totalPage: Number(payload.totalPage ?? 1),
    size: Number(payload.size ?? size),
    items: extractItems<TItem>(payload)
  };
};

const parseErrorMessage = async (response: Response) => {
  try {
    const payload = (await response.json()) as Record<string, unknown>;
    const title = typeof payload.title === "string" ? payload.title : undefined;
    const detail = typeof payload.detail === "string" ? payload.detail : undefined;
    const knownMessage = mapKnownErrorMessage(title, detail);
    if (knownMessage) {
      return knownMessage;
    }

    if (typeof detail === "string" && detail.length > 0) {
      return detail;
    }
  } catch {
    // Ignore parse errors and fallback to status text.
  }

  return response.statusText || "Requisicao falhou.";
};

const send = async <TResponse>(
  method: HttpMethod,
  path: string,
  body?: Record<string, unknown>
): Promise<TResponse | null> => {
  const response = await fetch(buildUrl(path), {
    method,
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json"
    },
    body: body ? JSON.stringify(body) : undefined
  });

  if (!response.ok) {
    const errorMessage = await parseErrorMessage(response);
    throw new Error(errorMessage);
  }

  if (response.status === 204) {
    return null;
  }

  return (await response.json()) as TResponse;
};

export const createResource = async <TResponse>(
  resource: string,
  payload: Record<string, unknown>
) => send<TResponse>("POST", resource, payload);

export const updateResource = async <TResponse>(
  resource: string,
  id: string,
  payload: Record<string, unknown>
) => send<TResponse>("PUT", `${resource}/${id}`, payload);

export const deleteResource = async (
  resource: string,
  pathValue: string
) => send("DELETE", `${resource}/${pathValue}`);
