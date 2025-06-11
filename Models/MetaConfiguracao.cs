using System.ComponentModel.DataAnnotations;

namespace QuantusBI.Models
{
    /// <summary>
    /// Representa uma configuração de comportamento que pode ser atribuída a uma meta.
    /// Ex: Direta, Inversamente Proporcional, Faixa Intervalar, Condição Exata.
    /// Essa configuração define como a meta será interpretada e medida.
    /// </summary>
    public class MetaConfiguracao
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da configuração é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        [Display(Name = "Nome da Configuração")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "A descrição deve ter no máximo 255 caracteres.")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Expressão Condicional (Ex: >= 90%)")]
        public string? ExpressaoCondicional { get; set; }

        [StringLength(30)]
        [Display(Name = "Tipo de Cálculo")]
        public string? TipoComparacao { get; set; } // Ex: "Direta", "Inversa", "Intervalar"

        [Display(Name = "Configuração Ativa?")]
        public bool Ativa { get; set; } = true;
    }
}