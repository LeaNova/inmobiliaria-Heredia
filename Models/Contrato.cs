using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria_Heredia.Models;

public class Contrato {

    [Key]
    public int idContrato { get; set; }

    public string fechaInicio { get; set; }

    public string fechaFinal { get; set; }

    /*
    public DateTime fechaInicio { get; set; }

    public DateTime fechaFinal { get; set; }
    */

    public double alquilerMensual { get; set; }

    public int inmuebleId { get; set; }

    public int inquilinoId { get; set; }

    [ForeignKey(nameof(inmuebleId))]
    public Inmueble? propiedad { get; set; }

    [ForeignKey(nameof(inquilinoId))]
    public Inquilino? inquilino { get; set; }
}