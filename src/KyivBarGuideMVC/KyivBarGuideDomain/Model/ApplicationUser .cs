using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KyivBarGuideDomain.Model;
public class ApplicationUser : IdentityUser
{

    public Admin? AdminProfile { get; set; }
    public Client? ClientProfile { get; set; }
}
