using System.ComponentModel.DataAnnotations;

namespace QuantusBI.Models
{
    /// <summary>
    /// Representa uma Entidade (filial) vinculada a um Órgão (matriz).
    /// </summary>
    public class Entidade
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(150)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A sigla é obrigatória.")]
        [StringLength(20)]
        public string Sigla { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(255)]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "O tipo é obrigatório.")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O CNPJ deve conter 14 dígitos.")]
        public string CNPJ { get; set; }

        [Display(Name = "Entidade Ativa?")]
        public bool Ativa { get; set; }

        [Required(ErrorMessage = "O Órgão Pai é obrigatório.")]
        [Display(Name = "Órgão Pai")]
        public int OrgaoId { get; set; }

        // Navegação (opcional)
        public Orgao Orgao { get; set; }
    }
}