using Microsoft.AspNetCore.Mvc.Rendering;

namespace Empresa.Models
{
    public class AssignRoles
    {   
        public string? Username { get; set; }
        public string? RoleName { get; set; }

        public List<SelectListItem>? List { get; set; }
        public List<SelectListItem>? List1 { get; set; }

    }
}
