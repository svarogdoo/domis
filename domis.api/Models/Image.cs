namespace domis.api.Models;

public class Image
{
    public required int Id { get; set; }
    public string? Name { get; set; }
    public string? Url { get; set; }
    public string? BlobUrl { get; set; }
}