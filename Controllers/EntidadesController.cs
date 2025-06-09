using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuantusBI.Models;
using QuantusBI.Repositorio;
using QuantusBI.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace QuantusBI.Controllers
{
    public class EntidadesController : Controller
    {
        private readonly IEntidadeRepositorio _entidadeRepositorio;
        private readonly IOrgaoRepositorio _orgaoRepositorio;

        public EntidadesController(IEntidadeRepositorio entidadeRepositorio,
                                   IOrgaoRepositorio orgaoRepositorio)
        {
            _entidadeRepositorio = entidadeRepositorio;
            _orgaoRepositorio = orgaoRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var entidades = await _entidadeRepositorio.ListarEntidadesAsync();
            var orgaos = await _orgaoRepositorio.ListarOrgaosAsync();

            var entidadesViewModel = entidades.Select(entidade =>
            {
                var orgao = orgaos.FirstOrDefault(o => o.Id == entidade.OrgaoId);
                return new EntidadeViewModel
                {
                    Id = entidade.Id,
                    Nome = entidade.Nome,
                    Sigla = entidade.Sigla,
                    Endereco = entidade.Endereco,
                    Tipo = entidade.Tipo,
                    CNPJ = entidade.CNPJ,
                    Ativa = entidade.Ativa,
                    OrgaoId = entidade.OrgaoId,
                    OrgaoSigla = orgao?.Sigla ?? "(não encontrado)"
                };
            }).ToList();

            return View(entidadesViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CriarEditar(int? id)
        {
            var orgaos = await _orgaoRepositorio.ListarOrgaosAsync();

            var viewModel = new EntidadeViewModel
            {
                TiposDisponiveis = new()
                {
                    new SelectListItem("Pública", "Pública"),
                    new SelectListItem("Privada", "Privada")
                },
                OrgaosDisponiveis = orgaos.Select(o => new SelectListItem
                {
                    Text = $"{o.Nome} ({o.Sigla})",
                    Value = o.Id.ToString()
                }).ToList()
            };

            if (id.HasValue)
            {
                var entidade = (await _entidadeRepositorio.ListarEntidadesAsync())
                               .FirstOrDefault(e => e.Id == id.Value);

                if (entidade == null) return NotFound();

                viewModel.Id = entidade.Id;
                viewModel.Nome = entidade.Nome;
                viewModel.Sigla = entidade.Sigla;
                viewModel.Endereco = entidade.Endereco;
                viewModel.Tipo = entidade.Tipo;
                viewModel.CNPJ = entidade.CNPJ;
                viewModel.Ativa = entidade.Ativa;
                viewModel.OrgaoId = entidade.OrgaoId;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarEditar(EntidadeViewModel viewModel)
        {
            var orgaos = await _orgaoRepositorio.ListarOrgaosAsync();

            viewModel.TiposDisponiveis = new()
            {
                new SelectListItem("Pública", "Pública"),
                new SelectListItem("Privada", "Privada")
            };

            viewModel.OrgaosDisponiveis = orgaos.Select(o => new SelectListItem
            {
                Text = $"{o.Nome} ({o.Sigla})",
                Value = o.Id.ToString()
            }).ToList();

            if (!ModelState.IsValid)
                return View(viewModel);

            if (await _entidadeRepositorio.VerificarCnpjDuplicadoAsync(
                    viewModel.CNPJ, viewModel.Id == 0 ? null : viewModel.Id))
            {
                ModelState.AddModelError("CNPJ", "Já existe uma entidade com esse CNPJ.");
                return View(viewModel);
            }

            if (await _entidadeRepositorio.VerificarCnpjIgualAoOrgaoAsync(
                    viewModel.CNPJ, viewModel.OrgaoId))
            {
                ModelState.AddModelError("CNPJ", "O CNPJ da entidade não pode ser igual ao do órgão pai.");
                return View(viewModel);
            }

            var entidade = new Entidade
            {
                Id = viewModel.Id,
                Nome = viewModel.Nome,
                Sigla = viewModel.Sigla,
                Endereco = viewModel.Endereco,
                Tipo = viewModel.Tipo,
                CNPJ = viewModel.CNPJ,
                Ativa = viewModel.Ativa,
                OrgaoId = viewModel.OrgaoId
            };

            if (entidade.Id == 0)
            {
                await _entidadeRepositorio.CadastrarEntidadeAsync(entidade);
                TempData["MensagemSucesso"] = "Entidade cadastrada com sucesso!";
            }
            else
            {
                await _entidadeRepositorio.AtualizarEntidadeAsync(entidade);
                TempData["MensagemSucesso"] = "Informações da entidade atualizadas com sucesso!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}