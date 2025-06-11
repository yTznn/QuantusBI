using System.Collections.Generic;
using System.Threading.Tasks;
using QuantusBI.Models;
using QuantusBI.ViewModels; // Adicione este using

namespace QuantusBI.Repositorio
{
    /// <summary>
    /// Interface para operações de acesso a dados relacionadas ao cadastro de Documentos Contratuais.
    /// </summary>
    public interface IDocumentoContratualRepositorio
    {
        /// <summary>
        /// Cadastra um novo documento contratual no sistema.
        /// </summary>
        Task CadastrarDocumentoContratualAsync(DocumentoContratual documentoContratual);

        /// <summary>
        /// Atualiza os dados de um documento contratual existente.
        /// </summary>
        Task AtualizarDocumentoContratualAsync(DocumentoContratual documentoContratual);

        /// <summary>
        /// Retorna um documento contratual pelo seu Id.
        /// </summary>
        Task<DocumentoContratual?> ObterDocumentoContratualPorIdAsync(int id);

        /// <summary>
        /// Retorna a lista de todos os documentos contratuais, incluindo informações da entidade.
        /// </summary>
        Task<IEnumerable<DocumentoContratualViewModel>> ListarDocumentosContratuaisAsync();
    }
}