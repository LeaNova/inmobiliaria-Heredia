using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_Heredia.Models;

public class Propietario {
    
    [Display(Name = "Código")]
    public int idPropietario { get; set; }

    [Display(Name = "Nombre")]
    public String nombre { get; set; }

    [Display(Name = "Apellido")]
    public String apellido { get; set; }

    public String DNI { get; set; }

    [Display(Name = "Teléfono")]
    public String telefono { get; set; }

    [EmailAddress]
    public String Email { get; set; }
}