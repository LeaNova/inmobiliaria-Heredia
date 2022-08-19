using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliaria_Heredia.Models
{
	public abstract class RepositorioBase
	{
        protected readonly string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria_heredia;SslMode=none";

        protected RepositorioBase()
        {
            
        }
    }
}