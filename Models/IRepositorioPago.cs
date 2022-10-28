using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliaria_Heredia.Models {

	public interface IRepositorioPago : IRepositorio<Pago> {

        IList<Pago> ObtenerPorContrato(int id);
    }
}