using System.Runtime.Serialization;

namespace Empresa.Models
{
    public enum Localizacao
    {
        Piso_0, Piso_1, Piso_2, Piso_3, Piso_4, Piso_5, Piso_6, Piso_7
    }

    public class Departamento
    {
        public int DepartamentoId { get; set; }
        public string? NomeDepartamento { get; set; }
        public Localizacao? Localizacao { get; set; }
    }
}
