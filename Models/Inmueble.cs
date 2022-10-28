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

    [Key]
    public int idInmueble { get; set; }

    public string direccion { get; set; }

    public int uso { get; set; }

    public int tipo { get; set; }

    public int cantAmbientes { get; set; }

    public string coordenadas { get; set; }

    public double precio { get; set; }

    public bool disponible { get; set; }

    public int propietarioId { get; set; }

    public string? foto { get; set; }

    [ForeignKey(nameof(propietarioId))]
    public Propietario? duenio { get; set; }

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
    public string TipoNombre => tipo > 0 ? ((enTipos)tipo).ToString() : "";

    public static IDictionary<int, string> ObtenerTipos() {
        SortedDictionary<int, string> tipos = new SortedDictionary<int, string>();
        Type tipoEnumTipo = typeof(enTipos);

        foreach(var valor in Enum.GetValues(tipoEnumTipo)) {
            tipos.Add((int)valor, Enum.GetName(tipoEnumTipo, valor));
        }
        return tipos;
    }
}