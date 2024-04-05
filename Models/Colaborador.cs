using System.ComponentModel.DataAnnotations;

namespace Empresa.Models
{
    public enum Genero
    {
        Masculino, Feminino
    }


    public class Colaborador
    {
        public int ColaboradorId { get; set; }
        public string? Nome { get; set; }
        public Genero? Genero { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DataNascimento { get; set; }
        public string? DocumentoIdentificacao { get; set; }        
        public string? Email { get; set; }
        public string? Telefone { get; set; }        

       
        public int? DepartamentoId { get; set; }
        public int? FuncaoId { get; set; }  
        
        


        
        public Departamento? Departamento { get; set; }
        public Funcao? Funcao { get; set; }  
        
    }
}
