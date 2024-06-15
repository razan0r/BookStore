using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BookStore.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Address { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedOn { get; set; }= DateTime.Now;
        public DateTime UpdateOn { get; set; }= DateTime.Now;


    }
}
