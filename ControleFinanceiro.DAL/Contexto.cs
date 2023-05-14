using ControleFinanceiro.BLL.Models;
using ControleFinanceiro.DAL.Mapeamentos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControleFinanceiro.DAL
{
    public class Contexto : IdentityDbContext<Cliente, Funcao, string>
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Financiamento> Financiamentos { get; set; }
        public DbSet<Parcela> Parcelas { get; set; }

        public Contexto(DbContextOptions<Contexto> opcoes) : base(opcoes) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ClienteMap());
            builder.ApplyConfiguration(new FinanciamentoMap());
            builder.ApplyConfiguration(new ParcelaMap());
        }
    }
}