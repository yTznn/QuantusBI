using Microsoft.AspNetCore.Mvc;
using QuantusBI.Models;
using QuantusBI.Models.ViewModels;
using QuantusBI.Repositorio;

namespace QuantusBI.Controllers
{
    /// <summary>
    /// Controlador responsável pela gestão das faixas de cumprimento de metas.
    /// Permite visualizar, adicionar e remover faixas associadas a uma meta específica.
    /// </summary>
    public class MetaFaixaCumprimentosController : Controller
    {
        private readonly IMetaRepositorio _metaRepositorio;
        private readonly IMetaFaixaCumprimentoRepositorio _faixaRepositorio;

        /// <summary>
        /// Construtor da controller com injeção de dependência dos repositórios.
        /// </summary>
        public MetaFaixaCumprimentosController(
            IMetaRepositorio metaRepositorio,
            IMetaFaixaCumprimentoRepositorio faixaRepositorio)
        {
            _metaRepositorio = metaRepositorio;
            _faixaRepositorio = faixaRepositorio;
        }

        /// <summary>
        /// Exibe a tela de faixas de cumprimento para uma meta específica.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Gerenciar(int metaId)
        {
            var meta = await _metaRepositorio.ObterPorIdAsync(metaId);
            if (meta == null) return NotFound();

            ViewBag.MetaNome = meta.Nome;
            ViewBag.MetaId = meta.Id;

            var faixas = await _faixaRepositorio.ObterPorMetaIdAsync(metaId);
            return View("Gerenciar", faixas); // Aponta explicitamente para a view Gerenciar.cshtml
        }

        /// <summary>
        /// Exibe o formulário para adicionar uma nova faixa.
        /// </summary>
        [HttpGet]
        public IActionResult Nova(int metaId)
        {
            return View(new MetaFaixaCumprimentoViewModel { MetaId = metaId });
        }

        /// <summary>
        /// Processa o envio do formulário para cadastrar uma nova faixa.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Nova(MetaFaixaCumprimentoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var faixa = new MetaFaixaCumprimento
            {
                MetaId = model.MetaId!.Value,
                PercentualMinimo = model.PercentualMinimo,
                PercentualMaximo = model.PercentualMaximo,
                PercentualPagamento = model.PercentualPagamento
            };

            await _faixaRepositorio.InserirAsync(faixa);
            return RedirectToAction("Gerenciar", new { metaId = model.MetaId });
        }

        /// <summary>
        /// Remove todas as faixas de uma meta (caso necessário para reconfiguração).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RemoverTodas(int metaId)
        {
            await _faixaRepositorio.DeletarPorMetaIdAsync(metaId);
            return RedirectToAction("Gerenciar", new { metaId });
        }
    }
}