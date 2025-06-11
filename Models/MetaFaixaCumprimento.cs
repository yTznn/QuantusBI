using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantusBI.Models
{
    /// <summary>
    /// Representa uma faixa de cumprimento vinculada a uma meta.
    /// Cada faixa define quanto do valor total da meta será pago
    /// de acordo com o percentual atingido da meta.
    /// Exemplo: Se atingir ≥ 90%, paga-se 100% do valor da meta.
    /// </summary>
    public class MetaFaixaCumprimento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A meta é obrigatória.")]
        [Display(Name = "Meta")]
        public int? MetaId { get; set; }

        [ForeignKey("MetaId")]
        public Meta? Meta { get; set; }

        [Required(ErrorMessage = "O percentual mínimo é obrigatório.")]
        [Range(0.00, 100.00, ErrorMessage = "O percentual mínimo deve estar entre 0 e 100.")]
        [Display(Name = "Percentual Mínimo (%)")]
        public decimal PercentualMinimo { get; set; }

        [Range(0.00, 100.00, ErrorMessage = "O percentual máximo deve estar entre 0 e 100.")]
        [Display(Name = "Percentual Máximo (%)")]
        public decimal? PercentualMaximo { get; set; }

        [Required(ErrorMessage = "O percentual de pagamento é obrigatório.")]
        [Range(0.00, 100.00, ErrorMessage = "O percentual de pagamento deve estar entre 0 e 100.")]
        [Display(Name = "Percentual de Pagamento (%)")]
        public decimal PercentualPagamento { get; set; }
    }
}