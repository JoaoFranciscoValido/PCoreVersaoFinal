using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCore.Data;
using PCore.Models;

namespace PCore.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentesAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _caminho;

        public ComponentesAPI(ApplicationDbContext context, IWebHostEnvironment caminho)
        {
            _context = context;
            _caminho = caminho;
        }

        // GET: api/ComponentesAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComponentesAPIViewModel>>> GetFilmes()
        {
            var listaComponentes = await _context.Componentes
                .Select(f => new ComponentesAPIViewModel
                {
                    IdComponentes = f.IdComponentes,
                    Nome = f.Nome,
                    Foto = f.Foto,
                    Descricao = f.Descricao,
                    Preco = f.Preco,
                    Stock = f.Stock
                })
                .OrderBy(f => f.IdComponentes)
                .ToListAsync();
            return listaComponentes;
        }

        // GET: api/ComponentesAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Componentes>> GetComponentes(int id)
        {
            var componentes = await _context.Componentes.FindAsync(id);

            if (componentes == null)
            {
                return NotFound();
            }

            return componentes;
        }

        // PUT: api/ComponentesAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComponentes(int id, Componentes componentes)
        {
            if (id != componentes.IdComponentes)
            {
                return BadRequest();
            }

            _context.Entry(componentes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComponentesExists(id))
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

        // POST: api/ComponentesAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Componentes>> PostFilmes([FromForm] Componentes componentes  , IFormFile UpFotografia)
        {

            componentes.Foto = "";
            string localizacao = _caminho.WebRootPath;
            var nomeFoto = Path.Combine(localizacao, "fotos", UpFotografia.FileName);
            var fotoUp = new FileStream(nomeFoto, FileMode.Create);
            await UpFotografia.CopyToAsync(fotoUp);
            componentes.Foto = UpFotografia.FileName;

            try
            {
                _context.Componentes.Add(componentes);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
            return CreatedAtAction("GetComponentes", new { id = componentes.IdComponentes }, componentes);
        }

        // DELETE: api/ComponentesAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComponentes(int id)
        {
            var componentes = await _context.Componentes.FindAsync(id);
            if (componentes == null)
            {
                return NotFound();
            }

            _context.Componentes.Remove(componentes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComponentesExists(int id)
        {
            return _context.Componentes.Any(e => e.IdComponentes == id);
        }
    }
}
