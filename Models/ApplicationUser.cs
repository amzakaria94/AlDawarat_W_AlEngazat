using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AlDawarat_W_AlEngazat.Models {
    public class ApplicationUser : IdentityUser {
        [PersonalData]

        public string? Name { get; set; }

        public string? City { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
    }
}
