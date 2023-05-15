using Microsoft.AspNetCore.Mvc;
using ControleFinanceiro.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ControleFinanceiro.BLL.Models;

namespace ControleFinanceiro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanciamentoController : Controller
    {
        private readonly Contexto _context;
        public FinanciamentoController(Contexto context)
        {
            _context = context;
        }

        //GET: api/Financiamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Financiamento>>> GetFinanciamentos()
        {
            return await _context.Financiamentos.ToListAsync();
        }

        //GET: api/Financiamentos
        [HttpGet("{id}")]
        public async Task<ActionResult<Financiamento>> GetFinanciamento(int id)
        {
            var financiamento = await _context.Financiamentos.FindAsync(id);

            if (financiamento == null)
            {
                return NotFound();
            }

            return financiamento;
        }

        //PUT: api/Financiamento/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFinanciamento(int id, Financiamento financiamento)
        {
            if (id != financiamento.FinanciamentoId)
            {
                return BadRequest();
            }

            _context.Entry(financiamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinanciamentoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //POST: api/Financiamento
        [HttpPost]
        public async Task<ActionResult<Financiamento>> PostFinanciamento([FromBody]Financiamento financiamento)
        {
            // Verifica se as entradas do usuário são válidas
            if (!ValidarEntradas(financiamento))
            {
                return BadRequest();
            }

            // Verifica se o crédito é aprovado
            if (!AprovarCredito(financiamento))
            {
                return BadRequest();
            }

            // Calcula o valor total com juros
            financiamento.ValorTotalComJuros = ValorTotalComJuros(financiamento);

            // Adiciona o financiamento ao banco de dados
            _context.Financiamentos.Add(financiamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFinanciamento", new { id = financiamento.FinanciamentoId }, financiamento);
        }

        //DELETE: api/Financiamento

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinanciamento(int id)
        {
            var financiamento = await _context.Financiamentos.FindAsync(id);
            if (financiamento == null)
            {
                return NotFound();
            }

            _context.Financiamentos.Remove(financiamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FinanciamentoExists(int id)
        {
            return _context.Financiamentos.Any(e => e.FinanciamentoId == id);
        }

        private bool ValidarEntradas(Financiamento financiamento)
        {
            // Verifica se o valor solicitado é menor ou igual a R$ 1.000.000,00
            if (financiamento.ValorTotal > 1000000)
            {
                return false;
            }

            // Verifica se o número de parcelas está dentro do intervalo de 5 a 72
            if (financiamento.Parcelas.Count < 5 || financiamento.Parcelas.Count > 72)
            {
                return false;
            }

            // Verifica se o valor mínimo para pessoa jurídica é atendido
            if (financiamento.TipoFinancimento == "Pessoa Juridica" && financiamento.ValorTotal < 15000)
            {
                return false;
            }

            // Verifica se a data do primeiro vencimento está dentro do intervalo de 15 a 40 dias a partir da data atual
            DateTime dataMinima = DateTime.Now.AddDays(15);
            DateTime dataMaxima = DateTime.Now.AddDays(40);
            if (financiamento.DataUltimoVencimento < dataMinima || financiamento.DataUltimoVencimento > dataMaxima)
            {
                return false;
            }

            return true;
        }

        //Aprovação de crédito

        private bool AprovarCredito(Financiamento financiamento)
        {
            if (financiamento.TipoFinancimento == "Pessoa Juridica")
            {
                if (financiamento.ValorTotal < 15000)
                {
                    return false;
                }
            }

            DateTime dataAtual = DateTime.Now.Date;
            DateTime dataPrimeiroVencimento = financiamento.Parcelas.OrderBy(p => p.DataVencimento).First().DataVencimento.Date;

            int diasParaPrimeiroVencimento = (dataPrimeiroVencimento - dataAtual).Days;

            if (diasParaPrimeiroVencimento < 15 || diasParaPrimeiroVencimento > 40)
            {
                return false;
            }

            if (financiamento.TipoFinancimento == "Credito Direto")
            {
                if (financiamento.ValorTotal * Convert.ToDecimal(1.02) > 1000000)
                {
                    return false;
                }
            }
            else if (financiamento.TipoFinancimento == "Credito Consignado")
            {
                if (financiamento.ValorTotal * Convert.ToDecimal(1.01) > 1000000)
                {
                    return false;
                }
            }
            else if (financiamento.TipoFinancimento == "Pessoa Juridica")
            {
                if (financiamento.ValorTotal * Convert.ToDecimal(1.05) > 1000000)
                {
                    return false;
                }
            }
            else if (financiamento.TipoFinancimento == "Pessoa Fisica")
            {
                if (financiamento.ValorTotal * Convert.ToDecimal(1.03) > 1000000)
                {
                    return false;
                }
            }
            else if (financiamento.TipoFinancimento == "Credito Imobiliario")
            {
                if (financiamento.ValorTotal * Convert.ToDecimal(1.09) > 1000000)
                {
                    return false;
                }
            }

            // se chegou até aqui, o crédito é aprovado
            return true;

        }

        //cálculo do valor total + juros
        private decimal ValorTotalComJuros(Financiamento financiamento)
        {
            decimal taxaDeJuros = 0;

            if (financiamento.TipoFinancimento == "Credito Direto")
            {
                taxaDeJuros = 0.02M;
            }
            else if (financiamento.TipoFinancimento == "Credito Consignado")
            {
                taxaDeJuros = 0.01M;
            }
            else if (financiamento.TipoFinancimento == "Pessoa Juridica")
            {
                taxaDeJuros = 0.05M;
            }
            else if (financiamento.TipoFinancimento == "Credito Fisica")
            {
                taxaDeJuros = 0.03M;
            }
            else if (financiamento.TipoFinancimento == "Credito Imobiliario")
            {
                taxaDeJuros = 0.09M;
            }

            decimal valorTotalComJuros = financiamento.ValorTotal;

            foreach (Parcela parcela in financiamento.Parcelas)
            {
                valorTotalComJuros += valorTotalComJuros * taxaDeJuros;
            }

            return valorTotalComJuros;
        }
    }
}
