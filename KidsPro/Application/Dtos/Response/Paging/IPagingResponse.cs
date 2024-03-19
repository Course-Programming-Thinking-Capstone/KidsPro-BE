namespace Application.Dtos.Response.Paging;

public interface IPagingResponse<out T>
{
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public IEnumerable<T> Results { get; }
}