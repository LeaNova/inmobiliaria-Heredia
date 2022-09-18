using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliaria_Heredia.Models {

	public interface IRepositorioUsuario : IRepositorio<Usuario> {
        
        Usuario ObtenerPorMail(string mail);
    }
}