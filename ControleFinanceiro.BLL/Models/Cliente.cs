using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFinanceiro.BLL.Models
{
    public class Cliente : IdentityUser<string>
    {
        //public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string UF { get; set; }
        public int Celular { get; set; }
        public virtual ICollection<Financiamento> Financiamentos { get; set; }
        //public virtual ICollection<Parcela> Parcelas { get; set; }
    }
}
