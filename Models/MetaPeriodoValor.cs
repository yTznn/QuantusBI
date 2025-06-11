using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantusBI.Models
{
    /// <summary>
    /// Representa o valor atingido para uma Meta em um determinado período.
    /// Ex: 71 saídas de 01/05 a 07/05 para a meta de Clínica Cirúrgica.
    /// </summary>
    public class MetaPeriodoValor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A meta é obrigatória.")]
        [Display(Name = "Meta")]
        public int? MetaId { get; set; }

        [ForeignKey("MetaId")]
        public Meta? Meta { get; set; }

        [Required(ErrorMessage = "A data de início do período é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Início do Período")]
        public DateTime DataInicioPeriodo { get; set; }

        [Required(ErrorMessage = "A data de fim do período é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Fim do Período")]
        public DateTime DataFimPeriodo { get; set; }

        [Required(ErrorMessage = "O valor atingido no período é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O valor atingido deve ser um número não negativo.")]
        [Display(Name = "Valor Atingido")]
        public decimal ValorAtingido { get; set; }

        [Display(Name = "Observações")]
        [StringLength(500)]
        public string? Observacoes { get; set; }
    }
}