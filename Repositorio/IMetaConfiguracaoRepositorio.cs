using QuantusBI.Models;

namespace QuantusBI.Repositorio
{
    /// <summary>
    /// Define os métodos de acesso a dados para as configurações de meta.
    /// Cada configuração representa uma regra de comportamento para interpretação da meta.
    /// </summary>
    public interface IMetaConfiguracaoRepositorio
    {
        /// <summary>
        /// Obtém todas as configurações de meta ativas ou inativas.
        /// </summary>
        /// <returns>Lista de configurações cadastradas.</returns>
        Task<IEnumerable<MetaConfiguracao>> ListarTodasAsync();

        /// <summary>
        /// Obtém uma configuração de meta específica pelo ID.
        /// </summary>
        /// <param name="id">ID da configuração.</param>
        /// <returns>Objeto da configuração ou null se não encontrado.</returns>
        Task<MetaConfiguracao?> ObterPorIdAsync(int id);

        /// <summary>
        /// Insere uma nova configuração de meta no banco de dados.
        /// </summary>
        /// <param name="configuracao">Objeto contendo os dados da configuração.</param>
        Task InserirAsync(MetaConfiguracao configuracao);

        /// <summary>
        /// Atualiza os dados de uma configuração existente.
        /// </summary>
        /// <param name="configuracao">Objeto atualizado.</param>
        Task AtualizarAsync(MetaConfiguracao configuracao);
    }
}