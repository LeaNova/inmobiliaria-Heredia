using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliaria_Heredia.Models {

	public interface IRepositorioContrato : IRepositorio<Contrato> {

        IList<Contrato> ObtenerPorFechas(DateTime fecha1, DateTime fecha2);

        IList<Contrato> ObtenerPorInmueble(int id);

        IList<Contrato> ObtenerInmueblesDisponibles(DateTime fecha1, DateTime fecha2);
    }
}