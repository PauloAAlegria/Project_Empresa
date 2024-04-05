using Empresa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Empresa.Data
{
    public class EmpresaContext: IdentityDbContext<IdentityUser>
    {
        public EmpresaContext(DbContextOptions<EmpresaContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


        public DbSet<Colaborador> Colaborador { get; set; }
        public DbSet<Departamento> Departamento { get; set; }
        public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<Funcao> Funcao { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Requisicao> Requisicao { get; set; }
            
    }
}
