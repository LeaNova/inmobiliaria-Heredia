using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliaria_Heredia.Models {
	
	public interface IRepositorio<T> {

		int Alta(T t);

		int Baja(int id);

		int Modificar(T t);

		IList<T> ObtenerTodos();
        
		T ObtenerPorId(int id);
	}
}
