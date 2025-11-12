using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ProductManagement.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
