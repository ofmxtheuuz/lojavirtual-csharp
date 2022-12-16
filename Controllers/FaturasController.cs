using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LojaVirtual.Data;
using LojaVirtual.Models;
using Microsoft.AspNetCore.Authorization;

namespace LojaVirtual.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FaturasController : Controller
    {
        private readonly AppDbContext _context;

        public FaturasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Faturas
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Faturas.Include(f => f.Pedido);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Faturas/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Faturas == null)
            {
                return NotFound();
            }

            var fatura = await _context.Faturas
                .Include(f => f.Pedido)
                .FirstOrDefaultAsync(m => m.FaturaId == id);
            if (fatura == null)
            {
                return NotFound();
            }

            return View(fatura);
        }

        // GET: Faturas/Create
        public IActionResult Create()
        {
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "Cep");
            return View();
        }

        // POST: Faturas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FaturaId,PedidoId,FaturaStatus,ExternalReference,IPAddress,PreferenceMPId,Preco,MetodoDePagamento,CreatedDate,LastUpdatedDate")] Fatura fatura)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fatura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "Cep", fatura.PedidoId);
            return View(fatura);
        }

        // GET: Faturas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Faturas == null)
            {
                return NotFound();
            }

            var fatura = await _context.Faturas.FindAsync(id);
            if (fatura == null)
            {
                return NotFound();
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "Cep", fatura.PedidoId);
            return View(fatura);
        }

        // POST: Faturas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FaturaId,PedidoId,FaturaStatus,ExternalReference,IPAddress,PreferenceMPId,Preco,MetodoDePagamento,CreatedDate,LastUpdatedDate")] Fatura fatura)
        {
            if (id != fatura.FaturaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fatura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaturaExists(fatura.FaturaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "Cep", fatura.PedidoId);
            return View(fatura);
        }

        // GET: Faturas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Faturas == null)
            {
                return NotFound();
            }

            var fatura = await _context.Faturas
                .Include(f => f.Pedido)
                .FirstOrDefaultAsync(m => m.FaturaId == id);
            if (fatura == null)
            {
                return NotFound();
            }

            return View(fatura);
        }

        // POST: Faturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Faturas == null)
            {
                return Problem("Entity set 'AppDbContext.Faturas'  is null.");
            }
            var fatura = await _context.Faturas.FindAsync(id);
            if (fatura != null)
            {
                _context.Faturas.Remove(fatura);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FaturaExists(string id)
        {
          return (_context.Faturas?.Any(e => e.FaturaId == id)).GetValueOrDefault();
        }
    }
}
