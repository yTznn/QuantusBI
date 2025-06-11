using Dapper;
using QuantusBI.Models;
using QuantusBI.Infraestrutura;

namespace QuantusBI.Repositorio
{
    /// <summary>
    /// Implementa as operações de acesso ao banco de dados para faixas de cumprimento de metas.
    /// Utiliza Dapper para consultas e comandos SQL.
    /// </summary>
    public class MetaFaixaCumprimentoRepositorio : IMetaFaixaCumprimentoRepositorio
    {
        private readonly IConnectionFactory _connectionFactory;

        /// <summary>
        /// Construtor do repositório de faixas de cumprimento.
        /// </summary>
        /// <param name="connectionFactory">Fábrica de conexões com o banco de dados.</param>
        public MetaFaixaCumprimentoRepositorio(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MetaFaixaCumprimento>> ObterPorMetaIdAsync(int metaId)
        {
            const string sql = @"
                SELECT * 
                FROM MetaFaixaCumprimento
                WHERE MetaId = @MetaId
                ORDER BY PercentualMinimo DESC";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<MetaFaixaCumprimento>(sql, new { MetaId = metaId });
        }

        /// <inheritdoc />
        public async Task InserirAsync(MetaFaixaCumprimento faixa)
        {
            const string sql = @"
                INSERT INTO MetaFaixaCumprimento 
                    (MetaId, PercentualMinimo, PercentualMaximo, PercentualPagamento)
                VALUES 
                    (@MetaId, @PercentualMinimo, @PercentualMaximo, @PercentualPagamento)";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, faixa);
        }

        /// <inheritdoc />
        public async Task DeletarPorMetaIdAsync(int metaId)
        {
            const string sql = "DELETE FROM MetaFaixaCumprimento WHERE MetaId = @MetaId";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, new { MetaId = metaId });
        }
    }
}