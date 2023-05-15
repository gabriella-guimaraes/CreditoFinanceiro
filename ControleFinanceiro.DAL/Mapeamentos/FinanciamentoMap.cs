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
    public class FinanciamentoMap : IEntityTypeConfiguration<Financiamento>
    {
        public void Configure(EntityTypeBuilder<Financiamento> builder)
        {
            builder.HasKey(f => f.FinanciamentoId);
            builder.Property(f => f.CPF).IsRequired().HasMaxLength(20);
            builder.HasIndex(f => f.CPF).IsUnique();
            builder.Property(f => f.TipoFinancimento).IsRequired();
            builder.Property(f => f.ValorTotal).IsRequired();
            builder.Property(f => f.ValorTotalComJuros).IsRequired();
            builder.Property(f => f.DataUltimoVencimento).IsRequired();

            builder.HasOne(f => f.Cliente).WithMany(f => f.Financiamentos).HasForeignKey(f => f.ClienteID).IsRequired().OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(f => f.Parcelas).WithOne(f => f.Financiamento);

            builder.ToTable("Financiamentos");
        }
    }
}
