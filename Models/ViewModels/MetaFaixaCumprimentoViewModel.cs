using System.ComponentModel.DataAnnotations;

namespace QuantusBI.Models.ViewModels
{
    /// <summary>
    /// Representa os dados de entrada para cadastro ou edição de uma faixa de cumprimento de meta.
    /// Essa estrutura será usada na interface e nas validações do lado do servidor.
    /// </summary>
    public class MetaFaixaCumprimentoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A meta é obrigatória.")]
        [Display(Name = "Meta")]
        public int? MetaId { get; set; }

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