using Microsoft.AspNetCore.Identity;

namespace LokatyWebApi.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
