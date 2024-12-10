namespace domis.api.Models.Entities;

public class CompanyEntity
{
    public required int Id { get; set; }
    public string? UserId { get; set; }
    public string? Name { get; set; }
    public string? Number { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}