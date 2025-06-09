using System.Collections.Generic;
using System.Threading.Tasks;
using QuantusBI.Models;

namespace QuantusBI.Repositorio
{
    /// <summary>
    /// Interface para operações de acesso a dados relacionadas ao cadastro de Órgãos.
    /// </summary>
    public interface IOrgaoRepositorio
    {
        /// <summary>
        /// Cadastra um novo órgão no sistema.
        /// </summary>
        Task CadastrarOrgaoAsync(Orgao orgao);

        /// <summary>
        /// Atualiza os dados de um órgão existente no sistema.
        /// </summary>
        Task AtualizarOrgaoAsync(Orgao orgao);

        /// <summary>
        /// Retorna a lista de todos os órgãos cadastrados.
        /// </summary>
        Task<IEnumerable<Orgao>> ListarOrgaosAsync();

        /// <summary>
        /// Verifica se já existe um órgão com o CNPJ informado.
        /// </summary>
        Task<bool> VerificarCnpjDuplicadoAsync(string cnpj, int? ignorarId = null);
    }
}