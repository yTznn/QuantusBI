using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuantusBI.ViewModels
{
    public class DashboardFilterViewModel
    {
        public int? SelectedOrgaoId { get; set; }
        public int? SelectedEntidadeId { get; set; }
        public int? SelectedDocumentoContratualId { get; set; }

        [Display(Name = "Mês de Referência")]
        public int? SelectedMonth { get; set; }
        [Display(Name = "Ano de Referência")]
        public int? SelectedYear { get; set; }

        // MUDANÇAS AQUI: De IEnumerable<SelectListItem> para List<SelectListItem>
        public List<SelectListItem> Orgaos { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Entidades { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> DocumentosContratuais { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> AvailableMonths { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AvailableYears { get; set; } = new List<SelectListItem>();

        public IEnumerable<MetaDashboardViewModel> Metas { get; set; } = new List<MetaDashboardViewModel>();
    }
}