namespace Amsberg_digital_universe.Domain;

public class PagedResponse<T>
{
    public List<T> Items { get; set; } = [];
    public int Total { get; set; }

    public int Page { get; set; }
    public int PageSize { get; set; }
    public bool HasNext { get; set; }
    public bool HasPrevious { get; set; }
}