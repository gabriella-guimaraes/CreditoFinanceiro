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

            if(financiamento == null)
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
                if(!FinanciamentoExists(id))
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

        //DELETE: api/Financiamento

        private bool FinanciamentoExists(int id)
        {
            return _context.Financiamentos.Any(e => e.FinanciamentoId == id);
        }
    }
}
