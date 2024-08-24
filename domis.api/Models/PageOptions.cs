namespace domis.api.Models;

//public record PageOptions(int PageNumber = 1, int PageSize = 200); //TO-DO: Change PageSize to 20

public class PageOptions
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 200; //TO-DO: Change PageSize to 20
}