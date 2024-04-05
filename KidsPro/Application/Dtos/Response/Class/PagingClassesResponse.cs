namespace Application.Dtos.Response;

public class PagingClassesResponse
{
    public int TotalPage { get; set; }
    public int TotalRecord { get; set; }
    public List<ClassesResponse> Classes { get; set; }= new List<ClassesResponse>();
}