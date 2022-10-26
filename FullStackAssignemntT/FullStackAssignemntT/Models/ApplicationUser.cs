using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FullStackAssignemntT.Models
{
    //26.10 Tatiana - Applicaiton user
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }

    }
}
