using Microsoft.AspNetCore.Identity;


namespace Library.Shared.Model
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
