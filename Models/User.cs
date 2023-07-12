using Microsoft.AspNetCore.Identity;

namespace QuantumCloud.Models
{
    public class User : IdentityUser
    {
        public string Nama { get; set; }
    }
}
