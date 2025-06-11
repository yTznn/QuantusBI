using System.Collections.Generic;
using System.Threading.Tasks;
using QuantusBI.Models;

namespace QuantusBI.Repositorio
{
    public interface IMetaPeriodoValorRepositorio
    {
        Task CadastrarMetaPeriodoValorAsync(MetaPeriodoValor metaPeriodoValor);
        Task AtualizarMetaPeriodoValorAsync(MetaPeriodoValor metaPeriodoValor);
        Task<MetaPeriodoValor?> ObterMetaPeriodoValorPorIdAsync(int id);
        Task<IEnumerable<MetaPeriodoValor>> ListarMetaPeriodoValoresPorMetaAsync(int metaId);
    }
}