using Dapper;
using QuantusBI.Infraestrutura;
using QuantusBI.Models;

namespace QuantusBI.Repositorio
{
    /// <summary>
    /// Implementa as operações de acesso ao banco de dados para as configurações de meta.
    /// Utiliza Dapper para consultas e persistência.
    /// </summary>
    public class MetaConfiguracaoRepositorio : IMetaConfiguracaoRepositorio
    {
        private readonly IConnectionFactory _connectionFactory;

        /// <summary>
        /// Construtor do repositório de configurações de meta.
        /// </summary>
        public MetaConfiguracaoRepositorio(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MetaConfiguracao>> ListarTodasAsync()
        {
            const string sql = "SELECT * FROM MetaConfiguracao ORDER BY Nome";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<MetaConfiguracao>(sql);
        }

        /// <inheritdoc />
        public async Task<MetaConfiguracao?> ObterPorIdAsync(int id)
        {
            const string sql = "SELECT * FROM MetaConfiguracao WHERE Id = @Id";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<MetaConfiguracao>(sql, new { Id = id });
        }

        /// <inheritdoc />
        public async Task InserirAsync(MetaConfiguracao configuracao)
        {
            const string sql = @"
                INSERT INTO MetaConfiguracao (Nome, Descricao, ExpressaoCondicional, TipoComparacao, Ativa)
                VALUES (@Nome, @Descricao, @ExpressaoCondicional, @TipoComparacao, @Ativa)";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, configuracao);
        }

        /// <inheritdoc />
        public async Task AtualizarAsync(MetaConfiguracao configuracao)
        {
            const string sql = @"
                UPDATE MetaConfiguracao
                SET Nome = @Nome,
                    Descricao = @Descricao,
                    ExpressaoCondicional = @ExpressaoCondicional,
                    TipoComparacao = @TipoComparacao,
                    Ativa = @Ativa
                WHERE Id = @Id";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, configuracao);
        }
    }
}