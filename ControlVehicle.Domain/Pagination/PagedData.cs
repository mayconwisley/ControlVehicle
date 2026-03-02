namespace ControlVehicle.Domain.Pagination;

public sealed record PagedData<T>(IReadOnlyList<T> Items, int TotalCount);