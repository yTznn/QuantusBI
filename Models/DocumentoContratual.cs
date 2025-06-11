using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantusBI.Models
{
    /// <summary>
    /// Representa um documento contratual entre a entidade de saúde e a SES via IPGSE.
    /// </summary>
    public class DocumentoContratual
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A Entidade é obrigatória.")] // Adicionei mensagem de erro personalizada, se preferir.
        [Display(Name = "Entidade")]
        public int? EntidadeId { get; set; } // <--- ESTA É A ALTERAÇÃO CRÍTICA! De int para int?

        [ForeignKey("EntidadeId")]
        public Entidade? Entidade { get; set; } // Recomendo tornar a propriedade de navegação anulável também.

        [Required(ErrorMessage = "O Tipo de Contrato é obrigatório.")]
        [Display(Name = "Tipo de Contrato")]
        public string TipoContrato { get; set; } = string.Empty; // Boas práticas: inicialize strings para evitar nulls

        [Required(ErrorMessage = "O Número do Contrato é obrigatório.")]
        [Display(Name = "Número do Contrato")]
        public string NumeroContrato { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Data de Início é obrigatória.")]
        [Display(Name = "Data de Início")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A Data de Fim é obrigatória.")]
        [Display(Name = "Data de Fim")]
        [DataType(DataType.Date)]
        public DateTime DataFim { get; set; }

        [Required(ErrorMessage = "O Valor é obrigatório.")]
        [Display(Name = "Valor")]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O Status é obrigatório.")]
        public bool Status { get; set; }

        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }
    }
}