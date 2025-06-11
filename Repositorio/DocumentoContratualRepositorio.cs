using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq; // Adicione este using
using System.Threading.Tasks;
using QuantusBI.Models;
using QuantusBI.Infraestrutura;
using QuantusBI.ViewModels; // Adicione este using

namespace QuantusBI.Repositorio
{
    /// <summary>
    /// Implementação do repositório responsável pelas operações de banco de dados relacionadas ao Documento Contratual.
    /// Utiliza Dapper para execução de queries SQL com alto desempenho.
    /// </summary>
    public class DocumentoContratualRepositorio : IDocumentoContratualRepositorio
    {
        private readonly IConnectionFactory _connectionFactory;

        public DocumentoContratualRepositorio(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task CadastrarDocumentoContratualAsync(DocumentoContratual documentoContratual)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();

            string sql = @"
                INSERT INTO DocumentoContratual
                (EntidadeId, TipoContrato, NumeroContrato, DataInicio, DataFim, Valor, Status, Observacoes)
                VALUES
                (@EntidadeId, @TipoContrato, @NumeroContrato, @DataInicio, @DataFim, @Valor, @Status, @Observacoes);";

            await connection.ExecuteAsync(sql, documentoContratual);
        }

        public async Task AtualizarDocumentoContratualAsync(DocumentoContratual documentoContratual)
        {
            const string sql = @"
                UPDATE DocumentoContratual
                SET EntidadeId = @EntidadeId,
                    TipoContrato = @TipoContrato,
                    NumeroContrato = @NumeroContrato,
                    DataInicio = @DataInicio,
                    DataFim = @DataFim,
                    Valor = @Valor,
                    Status = @Status,
                    Observacoes = @Observacoes
                WHERE Id = @Id;";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, documentoContratual);
        }

        public async Task<DocumentoContratual?> ObterDocumentoContratualPorIdAsync(int id)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM DocumentoContratual WHERE Id = @Id;";
            return await connection.QuerySingleOrDefaultAsync<DocumentoContratual>(sql, new { Id = id });
        }

        public async Task<IEnumerable<DocumentoContratualViewModel>> ListarDocumentosContratuaisAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            // Consulta que une DocumentoContratual com Entidade para obter o nome/sigla da entidade
            const string sql = @"
                SELECT
                    dc.Id,
                    dc.EntidadeId,
                    e.Nome AS EntidadeNome, -- Adiciona o nome da entidade ao resultado
                    e.Sigla AS EntidadeSigla, -- Adiciona a sigla da entidade ao resultado
                    dc.TipoContrato,
                    dc.NumeroContrato,
                    dc.DataInicio,
                    dc.DataFim,
                    dc.Valor,
                    dc.Status,
                    dc.Observacoes
                FROM DocumentoContratual dc
                INNER JOIN Entidade e ON dc.EntidadeId = e.Id
                INNER JOIN Orgao o ON e.OrgaoId = o.Id
                ORDER BY dc.DataFim DESC;"; // Ordena por data de fim, por exemplo

            // Mapeia para DocumentoContratualViewModel
            var result = await connection.QueryAsync<dynamic>(sql);

            return result.Select(r => new DocumentoContratualViewModel
            {
                DocumentoContratual = new DocumentoContratual
                {
                    Id = r.Id,
                    EntidadeId = r.EntidadeId,
                    TipoContrato = r.TipoContrato,
                    NumeroContrato = r.NumeroContrato,
                    DataInicio = r.DataInicio,
                    DataFim = r.DataFim,
                    Valor = r.Valor,
                    Status = r.Status,
                    Observacoes = r.Observacoes
                },
                // Atribui o nome e a sigla da entidade obtidos da junção
                EntidadeNome = r.EntidadeNome,
                EntidadeSigla = r.EntidadeSigla,
                OrgaoSigla = r.OrgaoSigla
            }).ToList();
        }
    }
}