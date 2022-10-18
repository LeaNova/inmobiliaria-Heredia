using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_Heredia.Models;

public class PropietarioView {
    
    public PropietarioView(Propietario p) {
        this.nombre = p.nombre;
        this.apellido = p.apellido;
        this.DNI = p.DNI;
        this.telefono = p.telefono;
        this.Email = p.Email;
    }

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