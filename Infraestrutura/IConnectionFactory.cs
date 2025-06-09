using System.Data;

namespace QuantusBI.Infraestrutura
{
    /// <summary>
    /// Contrato para criação de conexões com o banco de dados.
    /// Permite abstrair a implementação concreta da fábrica de conexões.
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Cria uma conexão com o banco de dados.
        /// </summary>
        /// <returns>Instância de IDbConnection.</returns>
        IDbConnection CreateConnection();
    }
}