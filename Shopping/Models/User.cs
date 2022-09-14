using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Shopping.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string? Name { get; set; }

        public string? Password { get; set; }

        public string? Address { get; set; }

        public string? AdminID { get; set; }

    }
}
