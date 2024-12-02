namespace domis.api.Repositories.Queries;

public static class UserExtensionsQueries
{
    public const string UpsertAddress = @"
        INSERT INTO domis.address (UserId, Country, County, City, AddressLine, Apartment, PostalCode, 
                                    ContactPerson, ContactPhone, AddressType)
        VALUES (@UserId, @Country, @County, @City, @AddressLine, @Apartment, @PostalCode, 
                @ContactPerson, @ContactPhone, @AddressType)
        ON CONFLICT (UserId, AddressType)
        DO UPDATE SET 
            Country = COALESCE(EXCLUDED.Country, domis.address.Country),
            County = COALESCE(EXCLUDED.County, domis.address.County),
            City = COALESCE(EXCLUDED.City, domis.address.City),
            AddressLine = COALESCE(EXCLUDED.AddressLine, domis.address.AddressLine),
            Apartment = COALESCE(EXCLUDED.Apartment, domis.address.Apartment),
            PostalCode = COALESCE(EXCLUDED.PostalCode, domis.address.PostalCode),
            ContactPerson = COALESCE(EXCLUDED.ContactPerson, domis.address.ContactPerson),
            ContactPhone = COALESCE(EXCLUDED.ContactPhone, domis.address.ContactPhone),
            AddressType = domis.address.AddressType
        RETURNING Id;
    ";
    
    public const string UpsertCompany = @"
        WITH upsert AS (
            INSERT INTO domis.company (userid, name, number, firstname, lastname) 
            VALUES (@userid, @name, @number, @firstname, @lastname)
            ON CONFLICT (userid) 
            DO UPDATE SET
                name = COALESCE(EXCLUDED.name, domis.company.name),
                number = COALESCE(EXCLUDED.number, domis.company.number),
                firstname = COALESCE(EXCLUDED.firstname, domis.company.firstname),
                lastname = COALESCE(EXCLUDED.lastname, domis.company.lastname)
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