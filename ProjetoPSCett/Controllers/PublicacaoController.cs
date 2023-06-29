using Microsoft.AspNetCore.Mvc;
using ProjetoPSCett.Models;

namespace ProjetoPSCett.Controllers
{
    public class PublicacaoController : Controller
    {
        private readonly RepositorioPublicacao repositorio = new();
        public IActionResult Index(string stringPesquisa)
        {
            var publicacaoVM = new PublicacaoViewModel
            {
                Publicacoes = repositorio.GetAll().ToList()
            };

            if (!string.IsNullOrEmpty(stringPesquisa))
            {
                publicacaoVM.Publicacoes = repositorio.GetByTitulo(stringPesquisa).ToList();

                if(publicacaoVM.Publicacoes.Count != 0)
                {
                    return View(publicacaoVM);
                }
                else
                {
                    publicacaoVM.Publicacoes = repositorio.GetByAutor(stringPesquisa).ToList();
                }
            }
            return View(publicacaoVM);
        }

        public IActionResult Basquete()
        {
            var publicacaoVM = new PublicacaoViewModel
            {
                Publicacoes = repositorio.GetAll().ToList()
            };

            return View(publicacaoVM);
        }

        public IActionResult Volei()
        {
            var publicacaoVM = new PublicacaoViewModel
            {
                Publicacoes = repositorio.GetAll().ToList()
            };

            return View(publicacaoVM);
        }

        public IActionResult Futebol()
        {
            var publicacaoVM = new PublicacaoViewModel
            {
                Publicacoes = repositorio.GetAll().ToList()
            };

            return View(publicacaoVM);
        }

        public IActionResult Adicionar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Adicionar(int id, string autor, string titulo, string conteudo, EnumeradorEsporte esporte, DateTime data)
        {
            id = repositorio.GetAll().Count();
            id += 1;

            data = DateTime.Now;

            Publicacao novaPublicacao = new(id, autor, titulo, conteudo, data, esporte);

            repositorio.Add(novaPublicacao);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Editar(int id)
        {
            var aluno = repositorio.GetById(id);
            return View(aluno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, string autor, string titulo, string conteudo, EnumeradorEsporte esporte, DateTime data)
        {
            data = DateTime.Now;

            Publicacao publicacaoEditado = new(id, autor, titulo, conteudo, data, esporte);

            repositorio.Update(publicacaoEditado);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remover(int id)
        {
            var publicacao = repositorio.GetById(id);
            return View(publicacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remover(int id, bool notUsed)
        {
            var publicacao = repositorio.GetById(id);
            repositorio.Remove(publicacao);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AdicionarComentario(int id, string comentario)
        {
            repositorio.AddComentario(id, comentario);
            return RedirectToAction(nameof(Index));
        }
    }
}
