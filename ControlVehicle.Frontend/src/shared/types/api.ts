export type PaginatedResponse<TItem> = {
  totalData: number;
  page: number;
  totalPage: number;
  size: number;
  items: TItem[];
};

export type ResourceSummary = {
  key: string;
  title: string;
  total: number;
};
