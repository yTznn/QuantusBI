using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using QuantusBI.Models;
using QuantusBI.Infraestrutura;
using QuantusBI.ViewModels;
using System; // Para DateTime

namespace QuantusBI.Repositorio
{
    public class MetaRepositorio : IMetaRepositorio
    {
        private readonly IConnectionFactory _connectionFactory;

        public MetaRepositorio(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task CadastrarMetaAsync(Meta meta)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();
            string sql = @"
                INSERT INTO Meta (DocumentoContratualId, Nome, Descricao, VolumePactuadoMensal, PercentualContrato, Ativa)
                VALUES (@DocumentoContratualId, @Nome, @Descricao, @VolumePactuadoMensal, @PercentualContrato, @Ativa);";
            await connection.ExecuteAsync(sql, meta);
        }

        public async Task AtualizarMetaAsync(Meta meta)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();
            string sql = @"
                UPDATE Meta SET
                    DocumentoContratualId = @DocumentoContratualId,
                    Nome = @Nome,
                    Descricao = @Descricao,
                    VolumePactuadoMensal = @VolumePactuadoMensal,
                    PercentualContrato = @PercentualContrato,
                    Ativa = @Ativa
                WHERE Id = @Id;";
            await connection.ExecuteAsync(sql, meta);
        }

        public async Task<Meta?> ObterMetaPorIdAsync(int id)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();
            string sql = "SELECT * FROM Meta WHERE Id = @Id;";
            return await connection.QuerySingleOrDefaultAsync<Meta>(sql, new { Id = id });
        }

        public async Task<Meta?> ObterPorIdAsync(int id)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();
            string sql = "SELECT * FROM Meta WHERE Id = @Id;";
            return await connection.QueryFirstOrDefaultAsync<Meta>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Meta>> ListarMetasPorDocumentoContratualAsync(int documentoContratualId)
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();
            string sql = "SELECT * FROM Meta WHERE DocumentoContratualId = @DocumentoContratualId;";
            return await connection.QueryAsync<Meta>(sql, new { DocumentoContratualId = documentoContratualId });
        }

        public async Task<IEnumerable<Meta>> ListarTodasMetasAsync()
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();
            string sql = "SELECT * FROM Meta;";
            return await connection.QueryAsync<Meta>(sql);
        }

        // MÉTODO ATUALIZADO COM OS FILTROS DE MÊS/ANO E AGREGAÇÃO SQL
        public async Task<IEnumerable<MetaDashboardViewModel>> ListarMetasParaDashboardAsync(
            int? orgaoId = null,
            int? entidadeId = null,
            int? documentoContratualId = null,
            int? mesReferencia = null,
            int? anoReferencia = null)
        {
            using var connection = _connectionFactory.CreateConnection();

            // SQL com joins para obter todos os dados necessários e filtros opcionais
            // e AGREGANDO os valores por período no SQL!
            // Isso é mais eficiente do que somar em memória.
            string sql = @"
                SELECT
                    m.Id AS MetaId,
                    m.Nome AS MetaNome,
                    m.VolumePactuadoMensal,
                    m.PercentualContrato,
                    dc.NumeroContrato AS DocumentoContratualNumero,
                    e.Nome AS EntidadeNome,
                    o.Sigla AS OrgaoSigla,
                    -- Soma os valores atingidos para o mês/ano de referência
                    -- COALESCE(SUM(...), 0) garante que o TotalAtingidoMes seja 0 se não houver lançamentos.
                    COALESCE(SUM(
                        CASE
                            -- Se não há filtro de mês/ano, ou o período do lançamento cruza o mês de referência de 28 dias
                            WHEN @MesReferencia IS NULL OR @AnoReferencia IS NULL THEN mpv.ValorAtingido
                            WHEN mpv.DataInicioPeriodo <= DATEFROMPARTS(@AnoReferencia, @MesReferencia, 28)
                                 AND mpv.DataFimPeriodo >= DATEFROMPARTS(@AnoReferencia, @MesReferencia, 1) THEN mpv.ValorAtingido
                            ELSE 0
                        END
                    ), 0) AS TotalAtingidoMes
                FROM Meta m
                INNER JOIN DocumentoContratual dc ON m.DocumentoContratualId = dc.Id
                INNER JOIN Entidade e ON dc.EntidadeId = e.Id
                INNER JOIN Orgao o ON e.OrgaoId = o.Id
                LEFT JOIN MetaPeriodoValor mpv ON m.Id = mpv.MetaId
                WHERE (@OrgaoId IS NULL OR o.Id = @OrgaoId)
                  AND (@EntidadeId IS NULL OR e.Id = @EntidadeId)
                  AND (@DocumentoContratualId IS NULL OR dc.Id = @DocumentoContratualId)
                GROUP BY m.Id, m.Nome, m.VolumePactuadoMensal, m.PercentualContrato,
                         dc.NumeroContrato, e.Nome, o.Sigla
                ORDER BY m.Nome;";

            var parameters = new
            {
                OrgaoId = orgaoId,
                EntidadeId = entidadeId,
                DocumentoContratualId = documentoContratualId,
                MesReferencia = mesReferencia,
                AnoReferencia = anoReferencia
            };

            // Dapper mapeia diretamente para MetaDashboardViewModel,
            // pois TotalAtingidoMes já virá calculado do SQL.
            var metasDashboard = await connection.QueryAsync<MetaDashboardViewModel>(sql, parameters);

            // Agora, vamos calcular os percentuais e valores financeiros em memória
            foreach (var metaVm in metasDashboard)
            {
                if (metaVm.VolumePactuadoMensal > 0)
                {
                    metaVm.PercentualMetaAtingida = (metaVm.TotalAtingidoMes / metaVm.VolumePactuadoMensal) * 100;
                }
                else
                {
                    metaVm.PercentualMetaAtingida = 0;
                }

                // Calcular Percentual de Recebimento baseado nos critérios
                if (metaVm.PercentualMetaAtingida >= 100)
                {
                    metaVm.PercentualRecebimento = 100;
                }
                else if (metaVm.PercentualMetaAtingida >= 90)
                {
                    metaVm.PercentualRecebimento = 100;
                }
                else if (metaVm.PercentualMetaAtingida >= 80)
                {
                    metaVm.PercentualRecebimento = 90;
                }
                else if (metaVm.PercentualMetaAtingida >= 70)
                {
                    metaVm.PercentualRecebimento = 80;
                }
                else
                {
                    metaVm.PercentualRecebimento = 70;
                }

                // Calcular Valor da Meta no Contrato (usando o valor fixo que você mencionou)
                // IDEALMENTE: Buscar o valor total do contrato do DocumentoContratual aqui,
                // ou passá-lo como parâmetro para este método. Por enquanto, mantemos fixo para teste.
                metaVm.ValorMetaNoContrato = (metaVm.PercentualContrato / 100) * 6097982.84m; // Valor do contrato total

                // Calcular Valor Recebido da Meta
                metaVm.ValorRecebidoDaMeta = (metaVm.ValorMetaNoContrato * metaVm.PercentualRecebimento) / 100;

                // Calcular Valor Descontado da Meta
                metaVm.ValorDescontadoDaMeta = metaVm.ValorMetaNoContrato - metaVm.ValorRecebidoDaMeta;
            }

            return metasDashboard;
        }
    }
}