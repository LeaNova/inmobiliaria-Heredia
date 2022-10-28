using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliaria_Heredia.Models {
    
	public interface IRepositorioInmueble : IRepositorio<Inmueble> {
        
        IList<Inmueble> ObtenerPorPropietario(Propietario p);
    }
}