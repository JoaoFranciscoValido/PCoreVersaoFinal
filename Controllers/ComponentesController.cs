using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PCore.Data;
using PCore.Models;


namespace PCore.Controllers
{

    public class ComponentesController : Controller
    {
        /// <summary>
        /// atributo que representa a base de dados do projeto
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// atributo que contém os dados da app web no servidor
        /// </summary>
        private readonly IWebHostEnvironment _caminho;
        
        /// <summary>
        /// variavel que recolhe os dados da pessoa que se autenticou
        /// </summary>
        private readonly UserManager<IdentityUser> _userManager;


        public ComponentesController(ApplicationDbContext context, IWebHostEnvironment caminho, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _caminho = caminho;
            _userManager = userManager;
        }

        // GET: Componentes
        public async Task<IActionResult> Index()
        {


            return View(await _context.Componentes.ToListAsync());
        }

        // GET: Componentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var componente = await _context.Componentes
                .Where(f => f.IdComponentes == id)
                .Include(f => f.ListaDeReviews)
                .ThenInclude(r => r.Utilizador)
                .OrderByDescending(f => f.Nome)
                .Include(fc => fc.ListaDeCategorias)
                .FirstOrDefaultAsync();
            if (componente == null)
            {
                return NotFound();
            }
            if (User.Identity.IsAuthenticated) {
                //recolher dados do utilizador
                var utilizador = _context.Utilizadores.Where(u => u.UserNameId == _userManager.GetUserId(User)).FirstOrDefault();

                var favorito = await _context.Carrinho.Where(f => f.ComponentesFK == id && f.UtilizadoresFK == utilizador.IdUtilizador).FirstOrDefaultAsync();

                if (favorito == null) {
                    ViewBag.Carrinho = false;
                } else {
                    ViewBag.Carrinho = true;
                }

            }



            return View(componente);
        }



        /// <summary>
        /// Metodo para apresentar os comentarios feitos pelos utilizadores
        /// </summary>
        /// <param name="IdComponentes"></param>
        /// <param name="comentario"></param>
        /// <param name="rating"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CreateComentario(int IdComponentes, string comentario, int rating) {
            //recolher dados do utilizador
            var utilizador = _context.Utilizadores.Where(u => u.UserNameId == _userManager.GetUserId(User)).FirstOrDefault();

            if (utilizador.ControlarReview == false) {
                //variavel que contem os dados da review, do utilizador e sobre qual componente foi feita review
                var comment = new Reviews {
                ComponentesFK = IdComponentes,
                Comentario = comentario.Replace("\r\n", "<br />"),
                Pontuacao = rating,
                Data = DateTime.Now,
                Visibilidade = true,
                Utilizador = utilizador
            };
                //adiciona a review à Base de Dados
                _context.Reviews.Add(comment);
                //o utilizador já fez a sua review
                utilizador.ControlarReview = true;
                //guardar a alteração na Base de Dados
                _context.Utilizadores.Update(utilizador);
                //Guarda as alterações na Base de Dados
                await _context.SaveChangesAsync();
                //redirecionar para a página dos details do componente
                return RedirectToAction(nameof(Details),new { id = IdComponentes});
            } else {
                return RedirectToAction(nameof(Details), new { id = IdComponentes });
            }
            
            
        }

        public async Task<IActionResult> AdicionarCarrinho(int IdComponentes) {
            //recolher dados do utilizador
            var utilizador = _context.Utilizadores.Where(u => u.UserNameId == _userManager.GetUserId(User)).FirstOrDefault();

            var carrinho = await _context.Carrinho.Where(f => f.ComponentesFK == IdComponentes && f.UtilizadoresFK == utilizador.IdUtilizador).FirstOrDefaultAsync();

            if (carrinho == null) {
                //variavel que contem dados do utilizador e do componente 
                var car = new Carrinho {
                    ComponentesFK = IdComponentes,
                    UtilizadoresFK = utilizador.IdUtilizador
                };
                //Adiciona o componente à Base de Dados
                _context.Carrinho.Add(car);
                //Guarda as alterações na Base de Dados
                await _context.SaveChangesAsync();
                //redirecionar para a página dos details do componente
                return RedirectToAction(nameof(Details), new { id = IdComponentes });
            } else {
                //remove da base de dados 
                _context.Carrinho.Remove(carrinho);
                //Guarda as alterações na Base de Dados
                await _context.SaveChangesAsync();
                //redirecionar para a página dos details do componente
                return RedirectToAction(nameof(Details), new { id = IdComponentes });
            }
        }

        // GET: Componentes/Create
        public IActionResult Create()
        {
            ViewBag.ListaDeCategorias = _context.ListaDeCategorias.OrderBy(c => c.IdCategorias).ToList();
            return View();
        }

        // POST: Componentes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdComponentes,Nome,Foto,Descricao,Categoria,Preco,Stock")] Componentes componentes,
            IFormFile imgFile, int[] CategoriaEscolhida)
        {

            // avalia se o array com a lista de categorias escolhidas associadas ao componente está vazio ou não
            if (CategoriaEscolhida.Length == 0) {
                //É gerada uma mensagem de erro
                ModelState.AddModelError("", "É necessário selecionar pelo menos uma categoria.");
                // gerar a lista Categorias que podem ser associadas ao componente
                ViewBag.ListaDeCategorias = _context.ListaDeCategorias.OrderBy(c => c.IdCategorias).ToList();
                // devolver controlo à View
                return View(componentes);
            }

            // criar uma lista com os objetos escolhidos das Categorias
            List<Categorias> listaDeCategoriasEscolhidas = new List<Categorias>();
            // Para cada objeto escolhido..
            foreach (int item in CategoriaEscolhida) {
                //procurar a categoria
                Categorias Categoria = _context.ListaDeCategorias.Find(item);
                // adicionar a Categoria à lista
                listaDeCategoriasEscolhidas.Add(Categoria);
            }

            // adicionar a lista ao objeto de "componente"
            componentes.ListaDeCategorias = listaDeCategoriasEscolhidas;





            componentes.Foto = imgFile.FileName;

            //_webhost.WebRootPath vai ter o path para a pasta wwwroot
            var saveimg = Path.Combine(_caminho.WebRootPath, "fotos", imgFile.FileName);

            var imgext = Path.GetExtension(imgFile.FileName);

            if (imgext == ".jpg" || imgext == ".png" || imgext == ".JPG" || imgext == ".PNG") {
                using (var uploadimg = new FileStream(saveimg, FileMode.Create)) {
                    await imgFile.CopyToAsync(uploadimg);

                }
            }

            if (ModelState.IsValid) {
                _context.Add(componentes);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            return View(componentes);

        }

        // GET: Componentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.ListaDeCategorias = _context.ListaDeCategorias.OrderBy(c => c.IdCategorias).ToList();
            var componentes = await _context.Componentes.FindAsync(id);
            if (componentes == null)
            {
                return NotFound();
            }
            return View(componentes);
        }

        // POST: Componentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdComponentes,Nome,Foto,Descricao,Categoria,Preco,Stock")] Componentes componentes,
            IFormFile imgFile, int[] CategoriaEscolhida)
        {
            if (id != componentes.IdComponentes) {
                return NotFound();
            }
            // avalia se o array com a lista de categorias escolhidas associadas ao Componente está vazio ou não
            if (CategoriaEscolhida.Length == 0) {
                //É gerada uma mensagem de erro
                ModelState.AddModelError("", "É necessário selecionar pelo menos uma categoria.");
                // gerar a lista Categorias que podem ser associadas ao Componente
                ViewBag.ListaDeCategorias = _context.ListaDeCategorias.OrderBy(c => c.IdCategorias).ToList();
                // devolver controlo à View
                return View(componentes);
            }

            // criar uma lista com os objetos escolhidos das Categorias
            List<Categorias> listaDeCategoriasEscolhidas = new List<Categorias>();
            // Para cada objeto escolhido..
            foreach (int item in CategoriaEscolhida) {
                //procurar a categoria
                Categorias Categoria = _context.ListaDeCategorias.Find(item);
                // adicionar a Categoria à lista
                listaDeCategoriasEscolhidas.Add(Categoria);
            }

            // adicionar a lista ao objeto de "Componente"
            componentes.ListaDeCategorias = listaDeCategoriasEscolhidas;




            /**************************************************/
            if (imgFile !=null) {
                componentes.Foto = imgFile.FileName;

                //_webhost.WebRootPath vai ter o path para a pasta wwwroot
                var saveimg = Path.Combine(_caminho.WebRootPath, "fotos", imgFile.FileName);

                var imgext = Path.GetExtension(imgFile.FileName);

                if (imgext == ".jpg" || imgext == ".png" || imgext == ".JPG" || imgext == ".PNG") {
                    using (var uploadimg = new FileStream(saveimg, FileMode.Create)) {
                        await imgFile.CopyToAsync(uploadimg);

                    }
                }
            } else {
                Componentes componentes1 = _context.Componentes.Find(componentes.IdComponentes);

                _context.Entry<Componentes>(componentes1).State = EntityState.Detached;


                componentes.Foto = componentes1.Foto;
            }
            
            /***************************************************/
            if (ModelState.IsValid) {
                _context.Update(componentes);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            return View(componentes);
        }

        // GET: Componentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmes = await _context.Componentes
                .FirstOrDefaultAsync(m => m.IdComponentes == id);
            if (filmes == null)
            {
                return NotFound();
            }

            return View(filmes);
        }

        // POST: Componentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var filmes = await _context.Componentes.FindAsync(id);
            _context.Componentes.Remove(filmes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComponentesExists(int id)
        {
            return _context.Componentes.Any(e => e.IdComponentes == id);
        }
    }
}
