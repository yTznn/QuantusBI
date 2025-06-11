using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantusBI.Models
{
    /// <summary>
    /// Representa uma meta específica dentro de um Documento Contratual.
    /// Ex: Saídas da Clínica Cirúrgica.
    /// </summary>
    public class Meta
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O documento contratual é obrigatório.")]
        [Display(Name = "Documento Contratual")]
        public int? DocumentoContratualId { get; set; }

        [ForeignKey("DocumentoContratualId")]
        public DocumentoContratual? DocumentoContratual { get; set; }

        [Required(ErrorMessage = "O nome da meta é obrigatório.")]
        [StringLength(200)]
        [Display(Name = "Nome da Meta")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Descrição Detalhada")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O volume pactuado mensal é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O volume pactuado mensal deve ser maior que zero.")]
        [Display(Name = "Volume Pactuado Mensal")]
        public decimal VolumePactuadoMensal { get; set; }

        // Poderíamos adicionar os volumes diário e semanal aqui também, se eles forem fixos e não calculados.
        // Por agora, vamos focar no mensal que parece ser a base para o recebimento.

        [Required(ErrorMessage = "O percentual no contrato é obrigatório.")]
        [Range(0.01, 100.00, ErrorMessage = "O percentual deve estar entre 0.01 e 100.")]
        [Display(Name = "Percentual do Contrato (%)")]
        public decimal PercentualContrato { get; set; } // Ex: 5.74

        [Display(Name = "Meta Ativa?")]
        public bool Ativa { get; set; }

        // Propriedades para os critérios de recebimento (em porcentagem do pactuado)
        // Isso pode ser uma lista de objetos separados ou hardcoded/enumerable no código,
        // mas para flexibilidade no BD, é melhor ter uma estrutura relacionada (MetaCriterioRecebimento).
        // Por enquanto, vamos manter simples e calcular no Controller/Serviço.
        // Se precisar de flexibilidade total, criamos uma tabela de "MetaCriterioRecebimento".
    }
}