using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using QuantusBI.Models;
using QuantusBI.Infraestrutura;

namespace QuantusBI.Repositorio
{
    /// <summary>
    /// Implementação do repositório responsável pelas operações de banco de dados relacionadas ao Órgão.
    /// Utiliza Dapper para execução de queries SQL com alto desempenho.
    /// </summary>
    public class OrgaoRepositorio : IOrgaoRepositorio
    {
        private readonly IConnectionFactory _connectionFactory;

        public OrgaoRepositorio(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task CadastrarOrgaoAsync(Orgao orgao)
        {
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                string sql = @"
                    INSERT INTO Orgao 
                    (Nome, Sigla, CNPJ, Email, Telefone, Endereco, Ativo, DataCadastro)
                    VALUES 
                    (@Nome, @Sigla, @CNPJ, @Email, @Telefone, @Endereco, @Ativo, @DataCadastro);";

                await connection.ExecuteAsync(sql, orgao);
            }
        }

        public async Task AtualizarOrgaoAsync(Orgao orgao)
        {
            const string sql = @"
                UPDATE Orgao
                SET Nome = @Nome,
                    Sigla = @Sigla,
                    CNPJ = @CNPJ,
                    Email = @Email,
                    Telefone = @Telefone,
                    Endereco = @Endereco,
                    Ativo = @Ativo
                WHERE Id = @Id;";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, orgao);
        }

        public async Task<IEnumerable<Orgao>> ListarOrgaosAsync()
        {
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                string sql = @"
                    SELECT 
                        Id, Nome, Sigla, CNPJ, Email, Telefone, Endereco, Ativo, DataCadastro
                    FROM Orgao;";

                return await connection.QueryAsync<Orgao>(sql);
            }
        }

        public async Task<bool> VerificarCnpjDuplicadoAsync(string cnpj, int? ignorarId = null)
        {
            using var connection = _connectionFactory.CreateConnection();

            string sql = "SELECT COUNT(1) FROM Orgao WHERE CNPJ = @Cnpj";

            if (ignorarId.HasValue)
                sql += " AND Id != @IgnorarId";

            var count = await connection.ExecuteScalarAsync<int>(sql, new { Cnpj = cnpj, IgnorarId = ignorarId });
            return count > 0;
        }
    }
}