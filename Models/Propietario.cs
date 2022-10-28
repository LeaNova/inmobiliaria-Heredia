using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_Heredia.Models;

public class Propietario {
    
    [Key]
    public int idPropietario { get; set; }

    public String nombre { get; set; }

    public String apellido { get; set; }

    public String DNI { get; set; }

    public String telefono { get; set; }

    [EmailAddress]
    public String Email { get; set; }

    public String pass {get; set; }
}