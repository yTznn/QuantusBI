using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace QuantusBI.Infraestrutura
{
    /// <summary>
    /// Implementação concreta da IConnectionFactory, responsável por criar conexões SQL com base no appsettings.json.
    /// </summary>
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Construtor que recebe a injeção de dependência da configuração do projeto.
        /// </summary>
        /// <param name="configuration">Instância de IConfiguration para acessar a string de conexão.</param>
        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Cria e retorna uma conexão com o banco de dados.
        /// </summary>
        /// <returns>Instância de IDbConnection.</returns>
        public IDbConnection CreateConnection()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            return new SqlConnection(connectionString);
        }
    }
}