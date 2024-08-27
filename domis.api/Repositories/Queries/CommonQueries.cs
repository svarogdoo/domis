namespace domis.api.Repositories.Queries;

public static class CommonQueries
{
    public const string GetCountries = @"
            SELECT 
                id AS Id, 
                country_name AS CountryName 
            FROM 
                domis.country;";
}