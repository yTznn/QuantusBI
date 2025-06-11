using Microsoft.AspNetCore.Mvc.Rendering;
using QuantusBI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuantusBI.ViewModels
{
    public class MetaViewModel
    {
        public Meta Meta { get; set; } = new Meta();
        public IEnumerable<SelectListItem> DocumentosContratuaisDisponiveis { get; set; } = new List<SelectListItem>();
        public string? DocumentoContratualNumero { get; set; }
        public string? EntidadeNome { get; set; }
        public string? OrgaoSigla { get; set; }
    }
}