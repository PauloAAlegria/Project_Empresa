using System.ComponentModel.DataAnnotations;

namespace Empresa.Models
{
    public class Register
    {
        public string? Username { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
