using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_Heredia.Models;

public class Pago {
    public int numPago { get; set; }

    public DateTime fechaPago { get; set;}

    public double importe { get; set; }
}