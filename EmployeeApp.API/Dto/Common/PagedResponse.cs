namespace EmployeeApp.API.Dto.Common;

public class PagedResponse<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public IEnumerable<T> Records { get; set; } = default!;
}