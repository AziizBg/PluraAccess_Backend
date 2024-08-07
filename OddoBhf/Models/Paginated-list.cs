public class PaginatedList<T>
{
    public List<T> Items { get; }
    public int PageIndex { get; }
    public int Length { get; }
    public int TotalPages { get; }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public PaginatedList(List<T> items, int pageIndex, int totalPages, int length)
    {
        Items = items;
        PageIndex = pageIndex;
        TotalPages = totalPages;
        Length = length;
    }
}