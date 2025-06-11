using Microsoft.AspNetCore.Mvc;
using QuantusBI.Models;
using QuantusBI.Models.ViewModels;
using QuantusBI.Repositorio;

namespace QuantusBI.Controllers
{
    /// <summary>
    /// Controlador responsável pela gestão das configurações de meta.
    /// Permite o cadastro, edição e listagem das regras de comportamento que podem ser atribuídas a uma meta.
    /// </summary>
    public class MetaConfiguracoesController : Controller
    {
        private readonly IMetaConfiguracaoRepositorio _configuracaoRepositorio;

        /// <summary>
        /// Construtor com injeção de dependência do repositório de configurações.
        /// </summary>
        public MetaConfiguracoesController(IMetaConfiguracaoRepositorio configuracaoRepositorio)
        {
            _configuracaoRepositorio = configuracaoRepositorio;
        }

        /// <summary>
        /// Lista todas as configurações de meta cadastradas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var configuracoes = await _configuracaoRepositorio.ListarTodasAsync();
            return View(configuracoes);
        }

        /// <summary>
        /// Exibe o formulário de cadastro de nova configuração.
        /// </summary>
        [HttpGet]
        public IActionResult Nova()
        {
            return View(new MetaConfiguracaoViewModel());
        }

        /// <summary>
        /// Processa o envio do formulário de nova configuração.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Nova(MetaConfiguracaoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var configuracao = new MetaConfiguracao
            {
                Nome = model.Nome,
                Descricao = model.Descricao,
                ExpressaoCondicional = model.ExpressaoCondicional,
                TipoComparacao = model.TipoComparacao,
                Ativa = model.Ativa
            };

            await _configuracaoRepositorio.InserirAsync(configuracao);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Exibe o formulário de edição de uma configuração existente.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var config = await _configuracaoRepositorio.ObterPorIdAsync(id);
            if (config == null) return NotFound();

            var model = new MetaConfiguracaoViewModel
            {
                Id = config.Id,
                Nome = config.Nome,
                Descricao = config.Descricao,
                ExpressaoCondicional = config.ExpressaoCondicional,
                TipoComparacao = config.TipoComparacao,
                Ativa = config.Ativa
            };

            return View(model);
        }

        /// <summary>
        /// Processa o envio do formulário de edição da configuração.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Editar(MetaConfiguracaoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var config = new MetaConfiguracao
            {
                Id = model.Id,
                Nome = model.Nome,
                Descricao = model.Descricao,
                ExpressaoCondicional = model.ExpressaoCondicional,
                TipoComparacao = model.TipoComparacao,
                Ativa = model.Ativa
            };

            await _configuracaoRepositorio.AtualizarAsync(config);
            return RedirectToAction("Index");
        }
    }
}