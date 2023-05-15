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

            builder.HasData(
                new Financiamento
                {
                    FinanciamentoId = 1,
                    ClienteID = "1",
                    CPF = "12345678956",
                    TipoFinancimento = "Pessoa Fisica",
                    ValorTotal = 100000,
                    ValorTotalComJuros = 1200000,
                    DataUltimoVencimento = new DateTime(2023, 10, 3)
                },
                new Financiamento
                {
                    FinanciamentoId = 2,
                    ClienteID = "2",
                    CPF = "90345678900",
                    TipoFinancimento = "Pessoa Juridica",
                    ValorTotal = 100000,
                    ValorTotalComJuros = 1200000,
                    DataUltimoVencimento = new DateTime(2023, 11, 13)
                });

            builder.ToTable("Financiamentos");
        }
    }
}
