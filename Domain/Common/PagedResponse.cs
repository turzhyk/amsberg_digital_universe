namespace Amsberg_digital_universe.Domain;

public class PagedResponse<T>
{
    public List<T> Content;
    public int Total;
    public int Page;
    public int PageSize;
    public bool HasNext;
    public bool HasPrevious;
}