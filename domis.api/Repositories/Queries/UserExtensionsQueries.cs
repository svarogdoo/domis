namespace domis.api.Repositories.Queries;

public static class UserExtensionsQueries
{
    public const string UpsertAddress = @"
        INSERT INTO domis.address (UserId, Country, County, City, AddressLine, Apartment, PostalCode, 
                               ContactPerson, ContactPhone, AddressType)
        VALUES (@UserId, @Country, @County, @City, @AddressLine, @Apartment, @PostalCode, 
                @ContactPerson, @ContactPhone, @AddressType)
        ON CONFLICT (userid, addresstype)
        DO UPDATE SET 
            Country = EXCLUDED.Country,
            County = EXCLUDED.County,
            City = EXCLUDED.City,
            AddressLine = EXCLUDED.AddressLine,
            Apartment = EXCLUDED.Apartment,
            PostalCode = EXCLUDED.PostalCode,
            ContactPerson = EXCLUDED.ContactPerson,
            ContactPhone = EXCLUDED.ContactPhone,
            AddressType = EXCLUDED.AddressType
        RETURNING Id;
    ";
    
    public const string UpsertCompany = @"
        WITH upsert AS (
            INSERT INTO domis.company (userid, name, number, firstname, lastname) 
            VALUES (@userid, @name, @number, @firstname, @lastname)
            ON CONFLICT (userid) 
            DO UPDATE SET
                name = EXCLUDED.name,
                number = EXCLUDED.number,
                firstname = EXCLUDED.firstname,
                lastname = EXCLUDED.lastname
            RETURNING id
        )
        SELECT id FROM upsert LIMIT 1;
    ";
    
    public const string GetAddresses = @"
        SELECT 
            AddressLine,
            Apartment,
            City,
            PostalCode,
            Country,
            County,
            ContactPhone,
            ContactPerson,
            AddressType
        FROM domis.address
        WHERE UserId = @UserId";
    
    public const string GetCompany = @"
        SELECT 
            Name,
            Number,
            FirstName,
            LastName
        FROM domis.company
        WHERE UserId = @UserId";
}