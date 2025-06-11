using System.ComponentModel.DataAnnotations;

namespace QuantusBI.Models.ViewModels
{
    /// <summary>
    /// Representa os dados utilizados para cadastrar ou editar uma configuração de meta.
    /// Configurações definem o comportamento esperado da meta (ex: Direta, Inversa, Intervalar).
    /// </summary>
    public class MetaConfiguracaoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da configuração é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        [Display(Name = "Nome da Configuração")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "A descrição deve ter no máximo 255 caracteres.")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Expressão Condicional (ex: ≥ 90%)")]
        public string? ExpressaoCondicional { get; set; }

        [StringLength(30)]
        [Display(Name = "Tipo de Comparação")]
        public string? TipoComparacao { get; set; }

        [Display(Name = "Configuração Ativa?")]
        public bool Ativa { get; set; } = true;
    }
}