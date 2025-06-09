using System.Collections.Generic;
using System.Threading.Tasks;
using QuantusBI.Models;
using QuantusBI.ViewModels;

namespace QuantusBI.Repositorio
{
    /// <summary>
    /// Interface para operações de acesso a dados relacionadas ao cadastro de Entidades (filiais).
    /// </summary>
    public interface IEntidadeRepositorio
    {
        /// <summary>
        /// Cadastra uma nova entidade no sistema.
        /// </summary>
        Task CadastrarEntidadeAsync(Entidade entidade);

        /// <summary>
        /// Atualiza os dados de uma entidade existente.
        /// </summary>
        Task AtualizarEntidadeAsync(Entidade entidade);

        /// <summary>
        /// Retorna a lista de todas as entidades cadastradas.
        /// </summary>
        Task<IEnumerable<Entidade>> ListarEntidadesAsync();

        /// <summary>
        /// Verifica se já existe uma entidade com o CNPJ informado.
        /// </summary>
        Task<bool> VerificarCnpjDuplicadoAsync(string cnpj, int? ignorarId = null);

        /// <summary>
        /// Verifica se o CNPJ da entidade é igual ao do órgão pai.
        /// </summary>
        Task<bool> VerificarCnpjIgualAoOrgaoAsync(string cnpj, int orgaoId);

        /// <summary>
        /// Retorna a lista de entidades com a sigla do órgão pai.
        /// </summary>
        Task<IEnumerable<EntidadeViewModel>> ListarEntidadesComSiglaOrgaoAsync();
    }
}