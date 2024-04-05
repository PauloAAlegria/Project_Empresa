namespace Empresa.Models
{
    public enum Grade
    {
        Nivel_1,
        Nivel_2,
        Nivel_3
    }


    public class Produto
    {
        public int ProdutoId { get; set; }
        public Grade? Grade { get; set; }
        public string? Nome { get; set; }
        public int? Quantidade { get; set; }
        public int? FornecedorId { get; set; }

        public Fornecedor? Fornecedor { get; set; }
    }
}
