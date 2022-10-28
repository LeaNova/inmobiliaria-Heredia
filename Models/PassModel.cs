using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria_Heredia.Models;

public class PassModel {

    [Display(Name = "Contraseña vieja"), DataType(DataType.Password)]
    public string passOld { get; set; }

    [Display(Name = "Contraseña nueva"), DataType(DataType.Password)]
    public string passNew { get; set; }

    [Display(Name = "Repetir Contraseña"), DataType(DataType.Password)]
    public string pass { get; set; }
}