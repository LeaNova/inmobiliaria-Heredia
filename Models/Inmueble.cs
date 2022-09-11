using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria_Heredia.Models;

public class Inmueble {
    public enum enUsos {
        Residencial = 1,
        Comercial = 2
    }

    public enum enTipos {
        Casa = 1,
        Departamento = 2,
        Local = 3,
        Depósito = 4,
        Oficinas = 5,
        Individual = 6
    }

    [Display(Name = "Código")]
    public int idInmueble { get; set; }

    [Display(Name = "Dirección")]
    public string direccion { get; set; }

    [Display(Name = "Uso")]
    public int uso { get; set; }

    [Display(Name = "Tipo")]
    public int tipo { get; set; }

    [Display(Name = "Ambientes")]
    public int cantAmbientes { get; set; }

    [Display(Name = "Coordenadas")]
    public string coordenadas { get; set; }

    [Display(Name = "Precio")]
    public double precio { get; set; }

    [Display(Name = "Disponible")]
    public bool disponible { get; set; }

    [Display(Name = "Propietario")]
    public int propietarioId { get; set; }

    [ForeignKey(nameof(propietarioId))]
    public Propietario duenio { get; set; }

    //Obtencion del Uso
    public string UsoNombre => uso > 0 ? ((enUsos)uso).ToString() : "";

    public static IDictionary<int, string> ObtenerUsos() {
        SortedDictionary<int, string> usos = new SortedDictionary<int, string>();
        Type tipoEnumUso = typeof(enUsos);

        foreach(var valor in Enum.GetValues(tipoEnumUso)) {
            usos.Add((int)valor, Enum.GetName(tipoEnumUso, valor));
        }
        return usos;
    }

    //Obtencion del Tipo
    public string TipoNombre => uso > 0 ? ((enTipos)uso).ToString() : "";

    public static IDictionary<int, string> ObtenerTipos() {
        SortedDictionary<int, string> usos = new SortedDictionary<int, string>();
        Type tipoEnumTipo = typeof(enTipos);

        foreach(var valor in Enum.GetValues(tipoEnumTipo)) {
            usos.Add((int)valor, Enum.GetName(tipoEnumTipo, valor));
        }
        return usos;
    }
}