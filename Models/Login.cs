using System.ComponentModel.DataAnnotations;

namespace Empresa.Models
{
    public class Login
    {
        [EmailAddress]
        public string? Username { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
