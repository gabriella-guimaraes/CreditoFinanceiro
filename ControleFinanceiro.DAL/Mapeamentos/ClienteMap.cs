using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleFinanceiro.BLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiro.DAL.Mapeamentos
{
    public class ClienteMap : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(c => c.ClienteId); // Definindo a propriedade ClienteId como chave primária
            builder.Property(c => c.ClienteId).ValueGeneratedOnAdd(); // Gerando valor automaticamente para ClienteId
            builder.Property(c => c.Nome).IsRequired();
            builder.Property(c => c.CPF).IsRequired().HasMaxLength(20);
            builder.HasIndex(c => c.CPF).IsUnique();

            builder.HasMany(c => c.Financiamentos).WithOne(f => f.Cliente).HasForeignKey(f => f.ClienteID).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.HasData(
                new Cliente
                {
                    ClienteId = "1",
                    Nome = "Administrador",
                    CPF = "12345678956",
                    UF = "SP",
                    Celular = 999999999

                },
                new Cliente
                {
                    ClienteId = "2",
                    Nome = "Usuário",
                    CPF = "90345678900",
                    UF = "RS",
                    Celular = 999922993

                });

            builder.ToTable("Clientes");
        }
    }
}
