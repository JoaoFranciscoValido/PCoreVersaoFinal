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
                .FirstOrDefaultAsync(m => m.IdComponentes == id);

            if (componente == null)
            {
                return NotFound();
            }
            if (User.Identity.IsAuthenticated) {
                //recolher dados do utilizador
                var utilizador = _context.Utilizadores.Where(u => u.UserNameId == _userManager.GetUserId(User)).FirstOrDefault();

                var favorito = await _context.Carrinho.Where(f => f.ComponentesFK == id && f.UtilizadoresFK == utilizador.IdUtilizador).FirstOrDefaultAsync();

                ViewBag.ListaDeCategorias = _context.ListaDeCategorias.OrderBy(c => c.Nome).ToList();

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

            //ViewBag.ListaDeCategorias = _context.ListaDeCategorias.OrderBy(c => c.IdCategorias).ToList();
            
            var componentes = await _context.Componentes
                                            .Include(l => l.ListaDeCategorias)
                                            .FirstOrDefaultAsync(m => m.IdComponentes == id);


            if (componentes == null)
            {
                return NotFound();
            }

            ViewBag.ListaDeCategorias = _context.ListaDeCategorias.OrderBy(c => c.Nome).ToList();

            return View(componentes);
        }


        // POST: Componentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        /// <summary>
        /// Edição dos dados de uma Lesson
        /// </summary>
        /// <param name="id">Id da Componente</param>
        /// <param name="newComponentes">novos dados a associar à Lesson</param>
        /// <param name="CategoriaEscolhida">Lista de Categorias a que a Lesson deve estar associada</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdComponentes,Nome,Foto,Descricao,Categoria,Preco,Stock")] Componentes newComponentes,
            IFormFile imgFile, int[] CategoriaEscolhida)
        {
            if (id != newComponentes.IdComponentes) {
                return NotFound();
            }


            // dados anteriormente guardados do componente
            var componentes = await _context.Componentes
                                       .Where(l => l.IdComponentes == id)
                                       .Include(l => l.ListaDeCategorias)
                                       .FirstOrDefaultAsync();

            // obter a lista dos IDs das Categorias associadas ao componente, antes da edição
            var oldListaCategorias = componentes.ListaDeCategorias
                                           .Select(c => c.IdCategorias)
                                           .ToList();

            // avaliar se o utilizador alterou alguma categoria associada ao componente
            // adicionadas -> lista de categorias adicionadas
            // retiradas   -> lista de categorias retiradas
            var adicionadas = CategoriaEscolhida.Except(oldListaCategorias);
            var retiradas = oldListaCategorias.Except(CategoriaEscolhida.ToList());

            // se alguma Category foi adicionada ou retirada
            // é necessário alterar a lista de categorias 
            // associada à Lesson
            if (adicionadas.Any() || retiradas.Any())
            {

                if (retiradas.Any())
                {
                    // retirar a Category 
                    foreach (int oldCategory in retiradas)
                    {
                        var categoryToRemove = componentes.ListaDeCategorias.FirstOrDefault(c => c.IdCategorias == oldCategory);
                        componentes.ListaDeCategorias.Remove(categoryToRemove);
                    }
                }
                if (adicionadas.Any())
                {
                    // adicionar a Categoria 
                    foreach (int newCategory in adicionadas)
                    {
                        var categoryToAdd = await _context.ListaDeCategorias.FirstOrDefaultAsync(c => c.IdCategorias == newCategory);
                        componentes.ListaDeCategorias.Add(categoryToAdd);
                    }
                }
            }

                // avalia se o array com a lista de categorias escolhidas associadas ao Componente está vazio ou não
                if (CategoriaEscolhida.Length == 0) {
                    //É gerada uma mensagem de erro
                    ModelState.AddModelError("", "É necessário selecionar pelo menos uma categoria.");
                    // gerar a lista Categorias que podem ser associadas ao Componente
                    ViewBag.ListaDeCategorias = _context.ListaDeCategorias.OrderBy(c => c.IdCategorias).ToList();
                    // devolver controlo à View
                    return View(newComponentes);
                }

            // avalia se o array com a lista de categorias escolhidas associadas ao Componente está vazio ou não
            if (CategoriaEscolhida.Length < 1)
            {
                //É gerada uma mensagem de erro
                ModelState.AddModelError("", "Selecione apenas uma categoria.");
                // gerar a lista Categorias que podem ser associadas ao Componente
                ViewBag.ListaDeCategorias = _context.ListaDeCategorias.OrderBy(c => c.IdCategorias).ToList();
                // devolver controlo à View
                return View(newComponentes);
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
                newComponentes.ListaDeCategorias = listaDeCategoriasEscolhidas;
            

            /**************************************************/
            if (imgFile !=null) {
                newComponentes.Foto = imgFile.FileName;

                //_webhost.WebRootPath vai ter o path para a pasta wwwroot
                var saveimg = Path.Combine(_caminho.WebRootPath, "fotos", imgFile.FileName);

                var imgext = Path.GetExtension(imgFile.FileName);

                if (imgext == ".jpg" || imgext == ".png" || imgext == ".JPG" || imgext == ".PNG") {
                    using (var uploadimg = new FileStream(saveimg, FileMode.Create)) {
                        await imgFile.CopyToAsync(uploadimg);

                    }
                }
            } else {
                Componentes componentes1 = _context.Componentes.Find(newComponentes.IdComponentes);

                _context.Entry<Componentes>(componentes1).State = EntityState.Detached;


                newComponentes.Foto = componentes1.Foto;
            }
            
            /***************************************************/
            if (ModelState.IsValid) {
                try { 
                    /* a EF só permite a manipulação de um único objeto de um mesmo tipo
                     *  por esse motivo, como estamos a usar o objeto 'componente'
                     *  temos de o atualizar com os dados que vêm da View
                     */
                    
                    componentes.Nome = newComponentes.Nome;
                    componentes.Descricao = newComponentes.Descricao;
                    componentes.Preco = newComponentes.Preco;
                    componentes.Stock = newComponentes.Stock;
                    componentes.Foto = newComponentes.Foto;


                    // adição do objeto 'componente' para atualização
                    _context.Update(componentes);
                     // 'commit' da atualização
                     await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComponentesExists(componentes.IdComponentes))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
           // _context.Update(newComponentes);
            //    await _context.SaveChangesAsync();

                
                return RedirectToAction(nameof(Index));    
            }
            return View(newComponentes);
        }

        // GET: Componentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var componente = await _context.Componentes
                                   .Include(l => l.ListaDeCategorias)
                                   .FirstOrDefaultAsync(m => m.IdComponentes == id);


            var componentes = await _context.Componentes
                .FirstOrDefaultAsync(m => m.IdComponentes == id);
            if (componentes == null)
            {
                return NotFound();
            }

            ViewBag.ListaDeCategorias = _context.ListaDeCategorias.OrderBy(c => c.Nome).ToList();

            return View(componentes);
        }

        // POST: Componentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var componentes = await _context.Componentes.FindAsync(id);
            _context.Componentes.Remove(componentes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComponentesExists(int id)
        {
            return _context.Componentes.Any(e => e.IdComponentes == id);
        }
    }
}
