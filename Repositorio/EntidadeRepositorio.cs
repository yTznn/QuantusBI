using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using QuantusBI.Models;
using QuantusBI.Infraestrutura;
using QuantusBI.ViewModels;

namespace QuantusBI.Repositorio
{
    /// <summary>
    /// Repositório responsável pelas operações de banco de dados relacionadas à Entidade (filial).
    /// </summary>
    public class EntidadeRepositorio : IEntidadeRepositorio
    {
        private readonly IConnectionFactory _connectionFactory;

        public EntidadeRepositorio(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task CadastrarEntidadeAsync(Entidade entidade)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();

            string sql = @"
                INSERT INTO Entidade 
                (Nome, Sigla, Endereco, Tipo, CNPJ, Ativa, OrgaoId)
                VALUES 
                (@Nome, @Sigla, @Endereco, @Tipo, @CNPJ, @Ativa, @OrgaoId);";

            await connection.ExecuteAsync(sql, entidade);
        }

        public async Task AtualizarEntidadeAsync(Entidade entidade)
        {
            const string sql = @"
                UPDATE Entidade
                SET Nome = @Nome,
                    Sigla = @Sigla,
                    Endereco = @Endereco,
                    Tipo = @Tipo,
                    CNPJ = @CNPJ,
                    Ativa = @Ativa,
                    OrgaoId = @OrgaoId
                WHERE Id = @Id;";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, entidade);
        }

        public async Task<IEnumerable<Entidade>> ListarEntidadesAsync()
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();

            const string sql = @"
                SELECT 
                    Id, Nome, Sigla, Endereco, Tipo, CNPJ, Ativa, OrgaoId
                FROM Entidade;";

            return await connection.QueryAsync<Entidade>(sql);
        }

        public async Task<IEnumerable<EntidadeViewModel>> ListarEntidadesComSiglaOrgaoAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                SELECT 
                    e.Id,
                    e.Nome,
                    e.Sigla,
                    e.Endereco,
                    e.Tipo,
                    e.CNPJ,
                    e.Ativa,
                    e.OrgaoId,
                    o.Sigla AS OrgaoSigla
                FROM Entidade e
                INNER JOIN Orgao o ON e.OrgaoId = o.Id;";

            return await connection.QueryAsync<EntidadeViewModel>(sql);
        }

        public async Task<bool> VerificarCnpjDuplicadoAsync(string cnpj, int? ignorarId = null)
        {
            using var connection = _connectionFactory.CreateConnection();

            string sql = "SELECT COUNT(1) FROM Entidade WHERE CNPJ = @Cnpj";

            if (ignorarId.HasValue)
                sql += " AND Id != @IgnorarId";

            int count = await connection.ExecuteScalarAsync<int>(sql, new { Cnpj = cnpj, IgnorarId = ignorarId });
            return count > 0;
        }

        public async Task<bool> VerificarCnpjIgualAoOrgaoAsync(string cnpj, int orgaoId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = "SELECT COUNT(1) FROM Orgao WHERE Id = @OrgaoId AND CNPJ = @Cnpj";

            int count = await connection.ExecuteScalarAsync<int>(sql, new { Cnpj = cnpj, OrgaoId = orgaoId });
            return count > 0;
        }
    }
}