using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PCore.Data;
using PCore.Models;

namespace PCore.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarrinhoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Favoritos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Carrinho.Include(f => f.Componente).Include(f => f.Utilizador);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Favoritos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrinho = await _context.Carrinho
                .Include(f => f.Componente)
                .Include(f => f.Utilizador)
                .FirstOrDefaultAsync(m => m.IdCarrinho == id);
            if (carrinho == null)
            {
                return NotFound();
            }

            return View(carrinho);
        }

        // GET: Carrinho/Create
        public IActionResult Create()
        {
            ViewData["ComponentesFK"] = new SelectList(_context.Componentes, "IdComponentes", "Foto");
            ViewData["UtilizadoresFK"] = new SelectList(_context.Utilizadores, "IdUtilizador", "Email");
            return View();
        }

        // POST: Carrinho/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCarrinho,UtilizadoresFK,ComponentesFK")] Carrinho carrinho)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carrinho);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ComponentesFK"] = new SelectList(_context.Componentes, "IdComponentes", "Foto", carrinho.ComponentesFK);
            ViewData["UtilizadoresFK"] = new SelectList(_context.Utilizadores, "IdUtilizador", "Email", carrinho.UtilizadoresFK);
            return View(carrinho);
        }

        // GET: Carrinho/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrinho = await _context.Carrinho.FindAsync(id);
            if (carrinho == null)
            {
                return NotFound();
            }
            ViewData["ComponentesFK"] = new SelectList(_context.Componentes, "IdComponentes", "Foto", carrinho.ComponentesFK);
            ViewData["UtilizadoresFK"] = new SelectList(_context.Utilizadores, "IdUtilizador", "Email", carrinho.UtilizadoresFK);
            return View(carrinho);
        }

        // POST: Carrinho/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCarrinho,UtilizadoresFK,ComponentesFK")] Carrinho carrinho)
        {
            if (id != carrinho.IdCarrinho)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carrinho);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarrinhoExists(carrinho.IdCarrinho))
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
            ViewData["ComponentesFK"] = new SelectList(_context.Componentes, "IdComponentes", "Foto", carrinho.ComponentesFK);
            ViewData["UtilizadoresFK"] = new SelectList(_context.Utilizadores, "IdUtilizador", "Email", carrinho.UtilizadoresFK);
            return View(carrinho);
        }

        // GET: Carrinho/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrinho = await _context.Carrinho
                .Include(f => f.Componente)
                .Include(f => f.Utilizador)
                .FirstOrDefaultAsync(m => m.IdCarrinho == id);
            if (carrinho == null)
            {
                return NotFound();
            }

            return View(carrinho);
        }

        // POST: Carrinho/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carrinho = await _context.Carrinho.FindAsync(id);
            _context.Carrinho.Remove(carrinho);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarrinhoExists(int id)
        {
            return _context.Carrinho.Any(e => e.IdCarrinho == id);
        }
    }
}
