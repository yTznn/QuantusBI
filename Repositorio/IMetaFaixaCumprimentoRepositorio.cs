using QuantusBI.Models;

namespace QuantusBI.Repositorio
{
    /// <summary>
    /// Define os métodos de acesso a dados para as faixas de cumprimento das metas.
    /// Cada faixa determina quanto será pago com base no percentual atingido.
    /// </summary>
    public interface IMetaFaixaCumprimentoRepositorio
    {
        /// <summary>
        /// Obtém todas as faixas de cumprimento vinculadas a uma meta específica.
        /// </summary>
        /// <param name="metaId">ID da meta.</param>
        /// <returns>Lista de faixas cadastradas para a meta.</returns>
        Task<IEnumerable<MetaFaixaCumprimento>> ObterPorMetaIdAsync(int metaId);

        /// <summary>
        /// Insere uma nova faixa de cumprimento para uma meta.
        /// </summary>
        /// <param name="faixa">Objeto contendo os dados da faixa.</param>
        Task InserirAsync(MetaFaixaCumprimento faixa);

        /// <summary>
        /// Remove todas as faixas associadas a uma meta específica.
        /// Útil para recriar as faixas em processos de edição.
        /// </summary>
        /// <param name="metaId">ID da meta.</param>
        Task DeletarPorMetaIdAsync(int metaId);
    }
}