using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_Heredia.Models;

public class LoginView {

    [Display(Name = "Usuario"), EmailAddress]
    public string user { get; set; }

    [Display(Name = "Contraseña"), DataType(DataType.Password)]
    public string pass { get; set; }
}
