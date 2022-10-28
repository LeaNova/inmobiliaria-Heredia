using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria_Heredia.Models;

public class PassView {

    [DataType(DataType.Password)]
    public string passOld { get; set; }

    [DataType(DataType.Password)]
    public string passNew { get; set; }

    [DataType(DataType.Password)]
    public string passNewR { get; set; }
}