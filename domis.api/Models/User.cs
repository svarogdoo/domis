﻿using Microsoft.AspNetCore.Identity;

namespace domis.api.Models;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AddressLine { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
}
