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
    public class MetaPeriodoValoresController : Controller
    {
        private readonly IMetaPeriodoValorRepositorio _metaPeriodoValorRepositorio;
        private readonly IMetaRepositorio _metaRepositorio;
        private readonly ILogger<MetaPeriodoValoresController> _logger;

        public MetaPeriodoValoresController(
            IMetaPeriodoValorRepositorio metaPeriodoValorRepositorio,
            IMetaRepositorio metaRepositorio,
            ILogger<MetaPeriodoValoresController> logger)
        {
            _metaPeriodoValorRepositorio = metaPeriodoValorRepositorio;
            _metaRepositorio = metaRepositorio;
            _logger = logger;
        }

        // Você pode ter uma Index que liste todos os lançamentos ou lançamentos por Meta.
        // Por enquanto, vamos criar uma Index que liste por Meta.
        public async Task<IActionResult> Index(int? metaId)
        {
            try
            {
                IEnumerable<MetaPeriodoValor> valores;
                string metaNome = "Todos os Valores de Metas por Período";

                if (metaId.HasValue && metaId.Value > 0)
                {
                    valores = await _metaPeriodoValorRepositorio.ListarMetaPeriodoValoresPorMetaAsync(metaId.Value);
                    var meta = await _metaRepositorio.ObterMetaPorIdAsync(metaId.Value);
                    if (meta != null)
                    {
                        metaNome = $"Valores para a Meta: {meta.Nome}";
                    }
                }
                else
                {
                    // Se não for passado um metaId, redireciona ou mostra todas de alguma forma
                    // Para simplicidade, vou listar todas as metas e pedir para o usuário escolher
                    TempData["MensagemInfo"] = "Selecione uma meta para ver seus valores por período.";
                    return RedirectToAction("Index", "Metas"); // Redireciona para a listagem de metas
                }

                ViewData["Title"] = metaNome;
                return View(valores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar lista de valores por período.");
                TempData["MensagemErro"] = "Ocorreu um erro ao carregar os valores por período. Tente novamente mais tarde.";
                return View(new List<MetaPeriodoValor>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> CriarEditar(int? id, int? metaId)
        {
            var viewModel = new MetaPeriodoValorViewModel();
            await PopularMetasDropdown(viewModel);

            if (id.HasValue && id.Value > 0)
            {
                var periodoValor = await _metaPeriodoValorRepositorio.ObterMetaPeriodoValorPorIdAsync(id.Value);
                if (periodoValor == null)
                {
                    TempData["MensagemErro"] = "Lançamento de valor por período não encontrado para edição.";
                    return RedirectToAction(nameof(Index));
                }
                viewModel.MetaPeriodoValor = periodoValor;
                ViewData["Title"] = "Atualizar Lançamento de Meta";
            }
            else
            {
                if (metaId.HasValue && metaId.Value > 0)
                {
                    viewModel.MetaPeriodoValor.MetaId = metaId.Value;
                }
                viewModel.MetaPeriodoValor.DataInicioPeriodo = DateTime.Today.AddDays(-7); // Sugere última semana
                viewModel.MetaPeriodoValor.DataFimPeriodo = DateTime.Today;
                ViewData["Title"] = "Cadastrar Novo Lançamento de Meta";
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarEditar(MetaPeriodoValorViewModel viewModel)
        {
            await PopularMetasDropdown(viewModel);

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = viewModel.MetaPeriodoValor.Id == 0 ? "Cadastrar Novo Lançamento de Meta" : "Atualizar Lançamento de Meta";
                return View(viewModel);
            }

            // Validação de datas para MetaPeriodoValor
            if (viewModel.MetaPeriodoValor.DataFimPeriodo < viewModel.MetaPeriodoValor.DataInicioPeriodo)
            {
                ModelState.AddModelError("MetaPeriodoValor.DataFimPeriodo", "A Data de Fim do Período não pode ser anterior à Data de Início.");
                ViewData["Title"] = viewModel.MetaPeriodoValor.Id == 0 ? "Cadastrar Novo Lançamento de Meta" : "Atualizar Lançamento de Meta";
                return View(viewModel);
            }

            try
            {
                if (viewModel.MetaPeriodoValor.Id == 0)
                {
                    await _metaPeriodoValorRepositorio.CadastrarMetaPeriodoValorAsync(viewModel.MetaPeriodoValor);
                    TempData["MensagemSucesso"] = "Lançamento de valor por período cadastrado com sucesso!";
                }
                else
                {
                    await _metaPeriodoValorRepositorio.AtualizarMetaPeriodoValorAsync(viewModel.MetaPeriodoValor);
                    TempData["MensagemSucesso"] = "Lançamento de valor por período atualizado com sucesso!";
                }
                return RedirectToAction(nameof(Index), new { metaId = viewModel.MetaPeriodoValor.MetaId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar Lançamento de Meta (ID: {PeriodoValorId})", viewModel.MetaPeriodoValor.Id);
                ModelState.AddModelError("", "Ocorreu um erro inesperado ao salvar o lançamento. Por favor, tente novamente.");
                ViewData["Title"] = viewModel.MetaPeriodoValor.Id == 0 ? "Cadastrar Novo Lançamento de Meta" : "Atualizar Lançamento de Meta";
                return View(viewModel);
            }
        }

        private async Task PopularMetasDropdown(MetaPeriodoValorViewModel viewModel)
        {
            var metas = await _metaRepositorio.ListarTodasMetasAsync();
            viewModel.MetasDisponiveis = metas.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = $"{m.Nome} (ID Contrato: {m.DocumentoContratualId})" // Pode ser melhorado para mostrar número do contrato
            }).ToList();
        }
    }
}