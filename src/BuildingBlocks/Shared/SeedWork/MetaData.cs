namespace Shared.SeedWork;

public class MetaData
{
    public int CurrentPage { get; set; } = default!;

    public int TotalPages { get; set; } = default!;

    public int PageSize { get; set; } = default!;

    public long TotalItems { get; set; } = default!;

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNext => CurrentPage < TotalPages;

    public int FirstRowOnPage => TotalItems > 0 ? (CurrentPage - 1) * PageSize + 1 : 0;

    public int LastRowOnPage => (int)Math.Min(CurrentPage * PageSize, TotalItems);
}