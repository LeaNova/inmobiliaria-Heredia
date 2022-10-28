using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_Heredia.Models;

public class Inquilino {

    [Key]
    public int idInquilino { get; set; }

    public String nombre { get; set; }

    public String apellido { get; set; }

    public String DNI { get; set; }

    public String telefono { get; set; }

    [EmailAddress]
    public String Email { get; set; }
}