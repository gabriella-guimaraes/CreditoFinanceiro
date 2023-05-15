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
        public string ClienteId { get; set; } // Nova propriedade para ser usada como chave primária
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string UF { get; set; }
        public int Celular { get; set; }
        public virtual ICollection<Financiamento> Financiamentos { get; set; }
    }
}
