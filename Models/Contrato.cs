using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria_Heredia.Models;

public class Contrato {

    [Display(Name = "Código")]
    public int idContrato { get; set; }

    [Display(Name = "F. Inicial")]
    public DateTime fechaInicio { get; set; }

    [Display(Name = "F. Finalización")]
    public DateTime fechaFinal { get; set; }

    [Display(Name = "Alquiler")]
    public double alquilerMensual { get; set; }

    [Display(Name = "Inmueble")]
    public int inmuebleId { get; set; }

    [Display(Name = "Inquilino")]
    public int inquilinoId { get; set; }

    //new
    [Display(Name = "Pago")]
    public int pagoId { get; set; }

    //new
    public bool vigente { get; set; }
    
    [ForeignKey(nameof(inmuebleId))]
    public Inmueble propiedad { get; set; }

    [ForeignKey(nameof(inquilinoId))]
    public Inquilino inquilino { get; set; }

    //new
    [ForeignKey(nameof(pagoId))]
    public Pago tipoPago { get; set; }
}