using Microsoft.AspNetCore.Mvc;
using QuantusBI.Repositorio;
using QuantusBI.ViewModels; // Garanta este using
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System;
using System.Globalization; // Adicione este using para CultureInfo
using QuantusBI.Models;

namespace QuantusBI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IMetaRepositorio _metaRepositorio;
        private readonly IOrgaoRepositorio _orgaoRepositorio;
        private readonly IEntidadeRepositorio _entidadeRepositorio;
        private readonly IDocumentoContratualRepositorio _documentoContratualRepositorio;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IMetaRepositorio metaRepositorio,
            IOrgaoRepositorio orgaoRepositorio,
            IEntidadeRepositorio entidadeRepositorio,
            IDocumentoContratualRepositorio documentoContratualRepositorio,
            ILogger<DashboardController> logger)
        {
            _metaRepositorio = metaRepositorio;
            _orgaoRepositorio = orgaoRepositorio;
            _entidadeRepositorio = entidadeRepositorio;
            _documentoContratualRepositorio = documentoContratualRepositorio;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? orgaoId, int? entidadeId, int? documentoContratualId, int? selectedMonth, int? selectedYear) // Adicionado parâmetros de mês e ano
        {
            var dashboardViewModel = new DashboardFilterViewModel // Instanciando a ViewModel de filtro
            {
                SelectedOrgaoId = orgaoId,
                SelectedEntidadeId = entidadeId,
                SelectedDocumentoContratualId = documentoContratualId,
                SelectedMonth = selectedMonth ?? DateTime.Today.Month, // Padrão para o mês atual
                SelectedYear = selectedYear ?? DateTime.Today.Year // Padrão para o ano atual
            };

            IEnumerable<MetaDashboardViewModel> metasDashboard = new List<MetaDashboardViewModel>();

            try
            {
                // Popula dropdowns para filtro
                await PopularOrgaosDropdown(dashboardViewModel);
                await PopularEntidadesDropdown(dashboardViewModel, orgaoId);
                await PopularDocumentosContratuaisDropdown(dashboardViewModel, entidadeId);
                PopularMonthsAndYearsDropdown(dashboardViewModel); // NOVO MÉTODO: Popular meses e anos

                // Aplicar filtros, passando o mês e ano selecionados para o repositório
                metasDashboard = await _metaRepositorio.ListarMetasParaDashboardAsync(
                    orgaoId,
                    entidadeId,
                    documentoContratualId,
                    dashboardViewModel.SelectedMonth, // Passando o mês
                    dashboardViewModel.SelectedYear // Passando o ano
                );

                dashboardViewModel.Metas = metasDashboard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar o dashboard.");
                TempData["MensagemErro"] = "Ocorreu um erro ao carregar o dashboard. Tente novamente mais tarde.";
                dashboardViewModel.Metas = new List<MetaDashboardViewModel>(); // Garante lista vazia em caso de erro
            }

            ViewData["Title"] = "Dashboard de Metas BI";
            return View(dashboardViewModel);
        }

        // NOVO MÉTODO: Popular dropdowns de Mês e Ano
        private void PopularMonthsAndYearsDropdown(DashboardFilterViewModel viewModel)
        {
            // Meses (1 a 12)
            viewModel.AvailableMonths = Enumerable.Range(1, 12)
                .Select(m => new SelectListItem
                {
                    Value = m.ToString(),
                    Text = new DateTime(2000, m, 1).ToString("MMMM", CultureInfo.CurrentCulture) // Ex: "Janeiro", "Fevereiro"
                }).ToList();

            // Anos (ex: últimos 5 anos e próximos 2)
            var currentYear = DateTime.Today.Year;
            viewModel.AvailableYears = Enumerable.Range(currentYear - 5, 8) // Gera anos de (Ano Atual - 5) a (Ano Atual + 2)
                .Select(y => new SelectListItem
                {
                    Value = y.ToString(),
                    Text = y.ToString()
                }).ToList();
        }

        // Método auxiliar para popular dropdown de Órgãos
        private async Task PopularOrgaosDropdown(DashboardFilterViewModel viewModel)
        {
            var orgaos = await _orgaoRepositorio.ListarOrgaosAsync();
            viewModel.Orgaos = orgaos.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = $"{o.Nome} ({o.Sigla})"
            }).ToList();
        }

        // Método auxiliar para popular dropdown de Entidades (filtra por OrgaoId)
        private async Task PopularEntidadesDropdown(DashboardFilterViewModel viewModel, int? orgaoId)
        {
            IEnumerable<Entidade> entidades;
            if (orgaoId.HasValue)
            {
                // Idealmente, este filtro seria no repositório para eficiência de DB
                var todasEntidades = await _entidadeRepositorio.ListarEntidadesAsync();
                entidades = todasEntidades.Where(e => e.OrgaoId == orgaoId.Value);
            }
            else
            {
                entidades = await _entidadeRepositorio.ListarEntidadesAsync();
            }

            viewModel.Entidades = entidades.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = $"{e.Nome} ({e.Sigla})"
            }).ToList();
        }

        // Método auxiliar para popular dropdown de Documentos Contratuais (filtra por EntidadeId)
        private async Task PopularDocumentosContratuaisDropdown(DashboardFilterViewModel viewModel, int? entidadeId)
        {
            IEnumerable<DocumentoContratualViewModel> documentos;
            if (entidadeId.HasValue)
            {
                // Idealmente, este filtro seria no repositório para eficiência de DB
                var todosDocumentos = await _documentoContratualRepositorio.ListarDocumentosContratuaisAsync();
                documentos = todosDocumentos.Where(d => d.DocumentoContratual.EntidadeId == entidadeId.Value);
            }
            else
            {
                documentos = await _documentoContratualRepositorio.ListarDocumentosContratuaisAsync();
            }

            viewModel.DocumentosContratuais = documentos.Select(d => new SelectListItem
            {
                Value = d.DocumentoContratual.Id.ToString(),
                Text = $"Contrato: {d.DocumentoContratual.NumeroContrato} ({d.EntidadeSigla})"
            }).ToList();
        }
    }
}