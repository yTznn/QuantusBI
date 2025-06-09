using Microsoft.AspNetCore.Mvc;
using QuantusBI.Models;
using QuantusBI.Repositorio;
using QuantusBI.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace QuantusBI.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar os órgãos (Organizações Sociais) no sistema.
    /// </summary>
    public class OrgaosController : Controller
    {
        private readonly IOrgaoRepositorio _orgaoRepositorio;

        public OrgaosController(IOrgaoRepositorio orgaoRepositorio)
        {
            _orgaoRepositorio = orgaoRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var orgaos = await _orgaoRepositorio.ListarOrgaosAsync();
            return View(orgaos);
        }

        [HttpGet]
        public async Task<IActionResult> CriarEditar(int? id)
        {
            if (id.HasValue)
            {
                var orgaos = await _orgaoRepositorio.ListarOrgaosAsync();
                var orgao = orgaos.FirstOrDefault(o => o.Id == id.Value);

                if (orgao == null)
                    return NotFound();

                var viewModel = new OrgaoViewModel
                {
                    Id = orgao.Id,
                    Nome = orgao.Nome,
                    Sigla = orgao.Sigla,
                    CNPJ = orgao.CNPJ,
                    Email = orgao.Email,
                    Telefone = orgao.Telefone,
                    Endereco = orgao.Endereco,
                    Ativo = orgao.Ativo
                };

                return View(viewModel);
            }

            return View(new OrgaoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarEditar(OrgaoViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            bool cnpjDuplicado = await _orgaoRepositorio.VerificarCnpjDuplicadoAsync(
                viewModel.CNPJ,
                viewModel.Id == 0 ? null : viewModel.Id
            );

            if (cnpjDuplicado)
            {
                ModelState.AddModelError("CNPJ", "Já existe um órgão com esse CNPJ cadastrado.");
                return View(viewModel);
            }

            var orgao = new Orgao
            {
                Id = viewModel.Id,
                Nome = viewModel.Nome,
                Sigla = viewModel.Sigla,
                CNPJ = viewModel.CNPJ,
                Email = viewModel.Email,
                Telefone = viewModel.Telefone,
                Endereco = viewModel.Endereco,
                Ativo = viewModel.Ativo,
                DataCadastro = viewModel.Id == 0 ? System.DateTime.Now : default
            };

            if (orgao.Id == 0)
            {
                await _orgaoRepositorio.CadastrarOrgaoAsync(orgao);
                TempData["MensagemSucesso"] = "Órgão cadastrado com sucesso!";
            }
            else
            {
                await _orgaoRepositorio.AtualizarOrgaoAsync(orgao);
                TempData["MensagemSucesso"] = "Informações do órgão atualizadas com sucesso!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}