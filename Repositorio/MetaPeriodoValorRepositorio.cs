using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using QuantusBI.Models;
using QuantusBI.Infraestrutura;

namespace QuantusBI.Repositorio
{
    public class MetaPeriodoValorRepositorio : IMetaPeriodoValorRepositorio
    {
        private readonly IConnectionFactory _connectionFactory;

        public MetaPeriodoValorRepositorio(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task CadastrarMetaPeriodoValorAsync(MetaPeriodoValor metaPeriodoValor)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();
            string sql = @"
                INSERT INTO MetaPeriodoValor (MetaId, DataInicioPeriodo, DataFimPeriodo, ValorAtingido, Observacoes)
                VALUES (@MetaId, @DataInicioPeriodo, @DataFimPeriodo, @ValorAtingido, @Observacoes);";
            await connection.ExecuteAsync(sql, metaPeriodoValor);
        }

        public async Task AtualizarMetaPeriodoValorAsync(MetaPeriodoValor metaPeriodoValor)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();
            string sql = @"
                UPDATE MetaPeriodoValor SET
                    MetaId = @MetaId,
                    DataInicioPeriodo = @DataInicioPeriodo,
                    DataFimPeriodo = @DataFimPeriodo,
                    ValorAtingido = @ValorAtingido,
                    Observacoes = @Observacoes
                WHERE Id = @Id;";
            await connection.ExecuteAsync(sql, metaPeriodoValor);
        }

        public async Task<MetaPeriodoValor?> ObterMetaPeriodoValorPorIdAsync(int id)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();
            string sql = "SELECT * FROM MetaPeriodoValor WHERE Id = @Id;";
            return await connection.QuerySingleOrDefaultAsync<MetaPeriodoValor>(sql, new { Id = id });
        }

        public async Task<IEnumerable<MetaPeriodoValor>> ListarMetaPeriodoValoresPorMetaAsync(int metaId)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();
            string sql = "SELECT * FROM MetaPeriodoValor WHERE MetaId = @MetaId ORDER BY DataInicioPeriodo;";
            return await connection.QueryAsync<MetaPeriodoValor>(sql, new { MetaId = metaId });
        }
    }
}