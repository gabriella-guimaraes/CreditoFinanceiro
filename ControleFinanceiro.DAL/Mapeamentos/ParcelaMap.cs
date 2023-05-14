﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleFinanceiro.BLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiro.DAL.Mapeamentos
{
    public class ParcelaMap : IEntityTypeConfiguration<Parcela>
    {
        public void Configure(EntityTypeBuilder<Parcela> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.FinanciamentoId).IsRequired();
            builder.Property(p => p.NumeroParcela).IsRequired();
            builder.Property(p => p.ValorParcela).IsRequired();
            builder.Property(p => p.DataPagamento).IsRequired();

            builder.HasOne(p => p.Financiamento).WithMany(p => p.Parcelas).HasForeignKey(p => p.Id).IsRequired();

            builder.ToTable("Parcelas");
        }
    }
}
