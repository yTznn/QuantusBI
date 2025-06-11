using Microsoft.AspNetCore.Mvc.Rendering;
using QuantusBI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuantusBI.ViewModels
{
    public class MetaPeriodoValorViewModel
    {
        public MetaPeriodoValor MetaPeriodoValor { get; set; } = new MetaPeriodoValor();

        public IEnumerable<SelectListItem> MetasDisponiveis { get; set; } = new List<SelectListItem>();

        // Propriedades para exibição na listagem/cálculo
        public string? MetaNome { get; set; }
        public string? DocumentoContratualNumero { get; set; }
        public string? EntidadeNome { get; set; }
        public decimal VolumePactuadoMensalMeta { get; set; } // Para facilitar cálculos no front/back
        public decimal PercentualContratoMeta { get; set; } // Para facilitar cálculos no front/back
    }
}