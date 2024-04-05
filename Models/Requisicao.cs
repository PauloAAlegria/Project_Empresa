using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Empresa.Models
{
    public enum Status
    {
        Aprovado, Reprovado
    }

    public enum Pedido
    {
        Urgente, Normal
    }
    public class Requisicao
    {
        public int RequisicaoId { get; set; }
        public string? UserId { get; set; }
        public int? ProdutoId { get; set; }
        public int? Quantidade { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Data do Pedido")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]    
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime? Data { get; set; } = DateTime.Now; 
        
        public Pedido? Pedido { get; set; }
        public Status? Status { get; set; }


        public Produto? Produto { get; set; }  
        
        public virtual IdentityUser? User { get; set; }
    }
}
