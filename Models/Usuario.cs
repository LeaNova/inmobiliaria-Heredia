using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria_Heredia.Models;

public class Usuario {
    public enum enAccess {
        Administrador = 1,
        Empleado = 2
    }

    [Display(Name = "Índice")]
    public int idUsuario { get; set; }

    [Display(Name = "Nombre")]
    public string nombre { get; set; }

    [Display(Name = "Apellido")]
    public string apellido { get; set; }

    [Display(Name = "Usuario"), EmailAddress]
    public string user { get; set; }

    [Display(Name = "Contraseña"), DataType(DataType.Password)]
    public string pass { get; set; }
    
    [Display(Name = "Avatar")]
    public string avatar { get; set; }

    [Display(Name = "Avatar"), NotMapped]
    public IFormFile avatarFile { get; set; }

    [Display(Name = "Acceso")]
    public int access { get; set; }

    //Obtencion del Access
    public string AccessNombre => access > 0 ? ((enAccess)access).ToString() : "";

    public static IDictionary<int, string> ObtenerAccess() {
        SortedDictionary<int, string> access = new SortedDictionary<int, string>();
        Type tipoEnumAccess = typeof(enAccess);

        foreach(var valor in Enum.GetValues(tipoEnumAccess)) {
            access.Add((int)valor, Enum.GetName(tipoEnumAccess, valor));
        }
        return access;
    }
}