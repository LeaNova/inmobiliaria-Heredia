using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria_Heredia.Models;

public class Pago {

    [Key]
    public int numPago { get; set; }

    public string fechaPago { get; set; }
    
    //public DateTime fechaPago { get; set; }

    public double importe { get; set; }

    public int contratoId { get; set; }

    public string detalle { get; set; }

    [ForeignKey(nameof(contratoId))]
    public Contrato? contrato { get; set; }
}