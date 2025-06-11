using System.Collections.Generic;
using System.Threading.Tasks;
using QuantusBI.Models;
using QuantusBI.ViewModels;

namespace QuantusBI.Repositorio
{
    public interface IMetaRepositorio
    {
        Task CadastrarMetaAsync(Meta meta);
        Task AtualizarMetaAsync(Meta meta);
        Task<Meta?> ObterMetaPorIdAsync(int id);
        Task<Meta?> ObterPorIdAsync(int id); // NOVO MÉTODO
        Task<IEnumerable<Meta>> ListarMetasPorDocumentoContratualAsync(int documentoContratualId);
        Task<IEnumerable<Meta>> ListarTodasMetasAsync();
        Task<IEnumerable<MetaDashboardViewModel>> ListarMetasParaDashboardAsync(
            int? orgaoId = null,
            int? entidadeId = null,
            int? documentoContratualId = null,
            int? mesReferencia = null,
            int? anoReferencia = null);
    }
}