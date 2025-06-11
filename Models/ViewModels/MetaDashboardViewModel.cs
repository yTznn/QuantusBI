using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuantusBI.ViewModels
{
    /// <summary>
    /// ViewModel para exibir os dados de BI das metas.
    /// </summary>
    public class MetaDashboardViewModel
    {
        public int MetaId { get; set; }
        public string MetaNome { get; set; } = string.Empty;
        public string DocumentoContratualNumero { get; set; } = string.Empty;
        public string EntidadeNome { get; set; } = string.Empty;
        public string OrgaoSigla { get; set; } = string.Empty;

        [Display(Name = "Volume Pactuado Mensal")]
        public decimal VolumePactuadoMensal { get; set; }

        [Display(Name = "Percentual Contrato (%)")]
        public decimal PercentualContrato { get; set; }

        [Display(Name = "Total Atingido no Mês")]
        public decimal TotalAtingidoMes { get; set; }

        [Display(Name = "% da Meta Atingida")]
        public decimal PercentualMetaAtingida { get; set; } // (TotalAtingidoMes / VolumePactuadoMensal) * 100

        [Display(Name = "Valor da Meta no Contrato")]
        [DataType(DataType.Currency)]
        public decimal ValorMetaNoContrato { get; set; } // Baseado no PercentualContrato da Meta

        [Display(Name = "Percentual de Recebimento (%)")]
        public decimal PercentualRecebimento { get; set; } // Baseado nos critérios (70%, 80%, etc.)

        [Display(Name = "Valor Recebido da Meta")]
        [DataType(DataType.Currency)]
        public decimal ValorRecebidoDaMeta { get; set; } // (ValorMetaNoContrato * PercentualRecebimento) / 100

        [Display(Name = "Valor Descontado da Meta")]
        [DataType(DataType.Currency)]
        public decimal ValorDescontadoDaMeta { get; set; } // ValorMetaNoContrato - ValorRecebidoDaMeta

        // Propriedade para guardar os valores detalhados por período (para drill-down)
        public List<MetaPeriodoValorDetalheViewModel> ValoresPorPeriodo { get; set; } = new List<MetaPeriodoValorDetalheViewModel>();
    }

    public class MetaPeriodoValorDetalheViewModel
    {
        public int Id { get; set; }
        public DateTime DataInicioPeriodo { get; set; }
        public DateTime DataFimPeriodo { get; set; }
        public decimal ValorAtingido { get; set; }
        public string? Observacoes { get; set; }
    }
}