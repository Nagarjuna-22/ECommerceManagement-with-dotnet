using Microsoft.AspNetCore.Identity;

namespace ECommerceOrderManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? CustomerId { get; set; }

        public Customer? Customer { get; set; }
    }
}