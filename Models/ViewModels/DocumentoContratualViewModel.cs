using Microsoft.AspNetCore.Mvc.Rendering;
using QuantusBI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Adicione este using

namespace QuantusBI.ViewModels
{
    public class DocumentoContratualViewModel
    {
        public DocumentoContratual DocumentoContratual { get; set; } = new DocumentoContratual(); // Inicializado para evitar null reference

        public IEnumerable<SelectListItem> Entidades { get; set; } = new List<SelectListItem>(); // Inicializado

        public IEnumerable<SelectListItem> TiposContrato { get; set; } = new List<SelectListItem>(); // Inicializado e pré-preenchido

        // Adicionadas para exibição na listagem
        [Display(Name = "Entidade")]
        public string EntidadeNome { get; set; } = string.Empty;
        public string EntidadeSigla { get; set; } = string.Empty;
        public string OrgaoSigla { get; set; } = string.Empty;

        public DocumentoContratualViewModel()
        {
            // Opcional: Popular TiposContrato aqui ou no Controller
            TiposContrato = new List<SelectListItem>
            {
                new SelectListItem { Value = "Contrato de Gestão", Text = "Contrato de Gestão" },
                new SelectListItem { Value = "Termo de Colaboração", Text = "Termo de Colaboração" }
            };
        }
    }
}