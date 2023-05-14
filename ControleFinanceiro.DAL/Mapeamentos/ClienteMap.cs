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
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Nome).IsRequired();
            builder.Property(c => c.CPF).IsRequired().HasMaxLength(20);
            builder.HasIndex(c => c.CPF).IsUnique();

            builder.HasMany(c => c.Financiamentos).WithOne(c => c.Cliente).OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("Clientes");
        }
    }
}
