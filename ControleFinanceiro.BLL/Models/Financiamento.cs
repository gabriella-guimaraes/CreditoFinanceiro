using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ControleFinanceiro.BLL.Models
{
    public class Financiamento
    {
        public int FinanciamentoId { get; set; }
        public int CPF { get; set; }
        public string TipoFinancimento { get; set; }
        public decimal ValorTotal { get; set; }
        public string ClienteID { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime DataUltimoVencimento { get; set; }
        public virtual ICollection<Parcela> Parcelas { get; set; }
        public decimal ValorTotalComJuros { get; set; }
    }
}
