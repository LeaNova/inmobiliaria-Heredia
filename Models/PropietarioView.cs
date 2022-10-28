using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_Heredia.Models;

public class PropietarioView {
    
    public PropietarioView(Propietario p) {
        this.nombre = p.nombre;
        this.apellido = p.apellido;
        this.DNI = p.DNI;
        this.telefono = p.telefono;
        this.Email = p.Email;
        this.foto = p.foto;
    }

    public String nombre { get; set; }

    public String apellido { get; set; }

    public String DNI { get; set; }

    public String telefono { get; set; }

    [EmailAddress]
    public String Email { get; set; }

    public String foto { get; set; }
}