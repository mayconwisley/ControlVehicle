namespace ControlVehicle.App.Pagination;

public sealed record PagedResult<T>(
	IReadOnlyList<T> Items,
	int Page,
	int Size,
	int TotalCount)
{
	public int TotalPages => (int)Math.Ceiling(TotalCount / (double)Size);
	public bool HasPrevious => Page > 1;
	public bool HasNext => Page < TotalPages;
}