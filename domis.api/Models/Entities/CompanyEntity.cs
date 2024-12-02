namespace domis.api.Models;

public class CompanyEntity
{
    public required int Id { get; set; }
    public string? UserId { get; set; }
    public string? Name { get; set; }
    public long? Number { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}