using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria_Heredia.Models;

public class Pago {

    [Display(Name = "Número de Pago")]
    public int numPago { get; set; }

    [Display(Name = "Fecha de Pago")]
    public DateTime fechaPago { get; set; }

    [Display(Name = "Importe")]
    public double importe { get; set; }

    [Display(Name = "Contrato")]
    public int contratoId { get; set; }

    [Display(Name = "Detalle")]
    public string detalle { get; set; }

    [ForeignKey(nameof(contratoId))]
    public Contrato? contrato { get; set; }
}