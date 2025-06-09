using System;
using System.ComponentModel.DataAnnotations;

namespace QuantusBI.Models
{
    /// <summary>
    /// Representa um Órgão (Organização Social) cadastrado no sistema.
    /// </summary>
    public class Orgao
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(150)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A sigla é obrigatória.")]
        [StringLength(20)]
        public string Sigla { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O CNPJ deve ter 14 caracteres.")]
        public string CNPJ { get; set; }

        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [StringLength(150)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [StringLength(20)]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(255)]
        public string Endereco { get; set; }

        [Display(Name = "Ativo?")]
        public bool Ativo { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}