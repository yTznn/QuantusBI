using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuantusBI.Models;
using QuantusBI.Repositorio;
using QuantusBI.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging; // Importar para logging de erros

namespace QuantusBI.Controllers
{
    public class DocumentosContratuaisController : Controller
    {
        private readonly IDocumentoContratualRepositorio _documentoContratualRepositorio;
        private readonly IEntidadeRepositorio _entidadeRepositorio;
        private readonly ILogger<DocumentosContratuaisController> _logger; // Adicionado para logging

        public DocumentosContratuaisController(
            IDocumentoContratualRepositorio documentoContratualRepositorio,
            IEntidadeRepositorio entidadeRepositorio,
            ILogger<DocumentosContratuaisController> logger) // Injetar ILogger
        {
            _documentoContratualRepositorio = documentoContratualRepositorio;
            _entidadeRepositorio = entidadeRepositorio;
            _logger = logger; // Atribuir o logger
        }

        /// <summary>
        /// Exibe a lista de todos os documentos contratuais.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var documentos = await _documentoContratualRepositorio.ListarDocumentosContratuaisAsync();
                return View(documentos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar lista de documentos contratuais na Index.");
                TempData["MensagemErro"] = "Ocorreu um erro ao carregar os documentos contratuais. Tente novamente mais tarde.";
                return View(new List<DocumentoContratualViewModel>()); // Retorna uma lista vazia para evitar crash
            }
        }

        /// <summary>
        /// Exibe o formulário para criar ou editar um documento contratual.
        /// </summary>
        /// <param name="id">Id do documento contratual a ser editado (opcional).</param>
        [HttpGet]
        public async Task<IActionResult> CriarEditar(int? id)
        {
            var viewModel = new DocumentoContratualViewModel();

            // Popula a lista de entidades para o dropdown (sempre)
            await PopularEntidadesDropdown(viewModel);

            // Os TiposContrato já são preenchidos no construtor da ViewModel.

            if (id.HasValue && id.Value > 0)
            {
                var documento = await _documentoContratualRepositorio.ObterDocumentoContratualPorIdAsync(id.Value);
                if (documento == null)
                {
                    TempData["MensagemErro"] = "Documento contratual não encontrado para edição.";
                    return RedirectToAction(nameof(Index));
                }
                viewModel.DocumentoContratual = documento;
                ViewData["Title"] = "Atualizar Documento Contratual";
            }
            else
            {
                // Define valores padrão para novo documento
                viewModel.DocumentoContratual.Status = true; // Novo documento geralmente começa ativo
                viewModel.DocumentoContratual.DataInicio = DateTime.Today; // Data de início padrão
                viewModel.DocumentoContratual.DataFim = DateTime.Today.AddYears(1); // Data de fim padrão
                ViewData["Title"] = "Cadastrar Documento Contratual";
            }

            return View(viewModel);
        }

        /// <summary>
        /// Processa o envio do formulário de criação ou edição de documento contratual.
        /// </summary>
        /// <param name="viewModel">Dados do documento contratual submetidos.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarEditar(DocumentoContratualViewModel viewModel)
        {
            // Re-popula as listas para o dropdown em caso de erro de validação (sempre antes de retornar a view)
            await PopularEntidadesDropdown(viewModel);

            // Validações de modelo (DataAnnotations e model binding)
            if (!ModelState.IsValid)
            {
                // Se houver erros de validação, retorna a View com a ViewModel para exibir as mensagens
                ViewData["Title"] = viewModel.DocumentoContratual.Id == 0 ? "Cadastrar Documento Contratual" : "Atualizar Documento Contratual";
                return View(viewModel);
            }

            // Validação de regra de negócio: DataFim não pode ser anterior a DataInicio
            if (viewModel.DocumentoContratual.DataFim < viewModel.DocumentoContratual.DataInicio)
            {
                ModelState.AddModelError("DocumentoContratual.DataFim", "A Data de Fim não pode ser anterior à Data de Início.");
                // Também pode adicionar ao sumário geral:
                // ModelState.AddModelError("", "As datas de início e fim do contrato são inválidas.");
                ViewData["Title"] = viewModel.DocumentoContratual.Id == 0 ? "Cadastrar Documento Contratual" : "Atualizar Documento Contratual";
                return View(viewModel); // Retorna a View com o erro específico
            }

            try
            {
                if (viewModel.DocumentoContratual.Id == 0)
                {
                    // Cadastrar novo documento
                    await _documentoContratualRepositorio.CadastrarDocumentoContratualAsync(viewModel.DocumentoContratual);
                    TempData["MensagemSucesso"] = "Documento Contratual cadastrado com sucesso!";
                }
                else
                {
                    // Atualizar documento existente
                    await _documentoContratualRepositorio.AtualizarDocumentoContratualAsync(viewModel.DocumentoContratual);
                    TempData["MensagemSucesso"] = "Informações do Documento Contratual atualizadas com sucesso!";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log do erro completo para depuração (útil em ambiente de produção)
                _logger.LogError(ex, "Erro ao salvar Documento Contratual (ID: {DocumentoId})", viewModel.DocumentoContratual.Id);

                // Adiciona uma mensagem de erro genérica ao ModelState para exibição na View
                ModelState.AddModelError("", "Ocorreu um erro inesperado ao processar sua solicitação. Por favor, tente novamente.");
                ViewData["Title"] = viewModel.DocumentoContratual.Id == 0 ? "Cadastrar Documento Contratual" : "Atualizar Documento Contratual";
                return View(viewModel);
            }
        }

        /// <summary>
        /// Método auxiliar para popular o dropdown de Entidades.
        /// </summary>
        /// <param name="viewModel">A ViewModel a ser preenchida.</param>
        private async Task PopularEntidadesDropdown(DocumentoContratualViewModel viewModel)
        {
            var entidades = await _entidadeRepositorio.ListarEntidadesAsync();
            viewModel.Entidades = entidades.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = $"{e.Nome} ({e.Sigla})"
            }).ToList();
        }
    }
}