using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuantusBI.Models;
using QuantusBI.Repositorio;
using QuantusBI.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace QuantusBI.Controllers
{
    public class MetasController : Controller
    {
        private readonly IMetaRepositorio _metaRepositorio;
        private readonly IDocumentoContratualRepositorio _documentoContratualRepositorio;
        private readonly ILogger<MetasController> _logger;

        public MetasController(
            IMetaRepositorio metaRepositorio,
            IDocumentoContratualRepositorio documentoContratualRepositorio,
            ILogger<MetasController> logger)
        {
            _metaRepositorio = metaRepositorio;
            _documentoContratualRepositorio = documentoContratualRepositorio;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Aqui você pode listar todas as metas ou filtrar por um documento contratual
                // Para a dashboard, usaremos o método ListarMetasParaDashboardAsync mais tarde.
                var metas = await _metaRepositorio.ListarTodasMetasAsync();
                var documentos = await _documentoContratualRepositorio.ListarDocumentosContratuaisAsync(); // Para obter o número do contrato, entidade, etc.

                var viewModelList = metas.Select(m =>
                {
                    var doc = documentos.FirstOrDefault(d => d.DocumentoContratual.Id == m.DocumentoContratualId);
                    return new MetaViewModel
                    {
                        Meta = m,
                        DocumentoContratualNumero = doc?.DocumentoContratual.NumeroContrato,
                        EntidadeNome = doc?.EntidadeNome,
                        OrgaoSigla = doc?.OrgaoSigla // Se DocumentoContratualViewModel tivesse OrgaoSigla
                    };
                }).ToList();

                return View(viewModelList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar lista de metas na Index.");
                TempData["MensagemErro"] = "Ocorreu um erro ao carregar as metas. Tente novamente mais tarde.";
                return View(new List<MetaViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> CriarEditar(int? id)
        {
            var viewModel = new MetaViewModel();
            await PopularDocumentosContratuaisDropdown(viewModel);

            if (id.HasValue && id.Value > 0)
            {
                var meta = await _metaRepositorio.ObterMetaPorIdAsync(id.Value);
                if (meta == null)
                {
                    TempData["MensagemErro"] = "Meta não encontrada para edição.";
                    return RedirectToAction(nameof(Index));
                }
                viewModel.Meta = meta;
                ViewData["Title"] = "Atualizar Meta";
            }
            else
            {
                viewModel.Meta.Ativa = true; // Meta nova ativa por padrão
                ViewData["Title"] = "Cadastrar Nova Meta";
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarEditar(MetaViewModel viewModel)
        {
            await PopularDocumentosContratuaisDropdown(viewModel);

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = viewModel.Meta.Id == 0 ? "Cadastrar Nova Meta" : "Atualizar Meta";
                return View(viewModel);
            }

            try
            {
                if (viewModel.Meta.Id == 0)
                {
                    await _metaRepositorio.CadastrarMetaAsync(viewModel.Meta);
                    TempData["MensagemSucesso"] = "Meta cadastrada com sucesso!";
                }
                else
                {
                    await _metaRepositorio.AtualizarMetaAsync(viewModel.Meta);
                    TempData["MensagemSucesso"] = "Meta atualizada com sucesso!";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar Meta (ID: {MetaId})", viewModel.Meta.Id);
                ModelState.AddModelError("", "Ocorreu um erro inesperado ao salvar a meta. Por favor, tente novamente.");
                ViewData["Title"] = viewModel.Meta.Id == 0 ? "Cadastrar Nova Meta" : "Atualizar Meta";
                return View(viewModel);
            }
        }

        private async Task PopularDocumentosContratuaisDropdown(MetaViewModel viewModel)
        {
            var documentos = await _documentoContratualRepositorio.ListarDocumentosContratuaisAsync();
            viewModel.DocumentosContratuaisDisponiveis = documentos
                .Select(d => new SelectListItem
                {
                    Value = d.DocumentoContratual.Id.ToString(),
                    Text = $"Contrato: {d.DocumentoContratual.NumeroContrato} ({d.EntidadeSigla})" // Melhor identificação
                }).ToList();
        }
    }
}