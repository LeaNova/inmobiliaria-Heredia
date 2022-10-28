using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliaria_Heredia.Models {
    public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble {

        public RepositorioInmueble(IConfiguration configuration) : base(configuration) {

        }
        
        public int Alta(Inmueble i) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    INSERT INTO Inmueble (direccion, uso, tipo, cantAmbientes, coordenadas, precio, disponible, propietarioId)
                    VALUES (@direccion, @uso, @tipo, @cantAmbientes, @coordenadas, @precio, @disponible, @propietarioId);
                    SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@direccion", i.direccion);
                    command.Parameters.AddWithValue("@uso", i.uso);
                    command.Parameters.AddWithValue("@tipo", i.tipo);
                    command.Parameters.AddWithValue("@cantAmbientes", i.cantAmbientes);
                    command.Parameters.AddWithValue("@coordenadas", i.coordenadas);
                    command.Parameters.AddWithValue("@precio", i.precio);
                    command.Parameters.AddWithValue("@disponible", i.disponible);
                    command.Parameters.AddWithValue("@propietarioId", i.propietarioId);

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    i.idInmueble = res;
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(int id) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    DELETE FROM Inmueble
                    WHERE idInmueble = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificar(Inmueble i) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    UPDATE Inmueble
                    SET direccion = @direccion, uso = @uso, tipo = @tipo, cantAmbientes = @cantAmbientes, coordenadas = @coordenadas, precio = @precio, disponible = @disponible, propietarioId = @propietarioId
                    WHERE idInmueble = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@direccion", i.direccion);
                    command.Parameters.AddWithValue("@uso", i.uso);
                    command.Parameters.AddWithValue("@tipo", i.tipo);
                    command.Parameters.AddWithValue("@cantAmbientes", i.cantAmbientes);
                    command.Parameters.AddWithValue("@coordenadas", i.coordenadas);
                    command.Parameters.AddWithValue("@precio", i.precio);
                    command.Parameters.AddWithValue("@disponible", i.disponible);
                    command.Parameters.AddWithValue("@propietarioId", i.propietarioId);
                    command.Parameters.AddWithValue("@id", i.idInmueble);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Inmueble> ObtenerTodos() {
            IList<Inmueble> res = new List<Inmueble>();
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    SELECT idInmueble, direccion, uso, tipo, cantAmbientes, coordenadas, precio, disponible, propietarioId, p.nombre, p.apellido
                    FROM Inmueble i
                    INNER JOIN Propietario p ON i.propietarioId = p.idPropietario";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.CommandType = CommandType.Text;
                    connection.Open();

                    var reader = command.ExecuteReader();
                    while (reader.Read()) {
                        Inmueble i = new Inmueble {
                            idInmueble = reader.GetInt32(0), 
                            direccion = reader.GetString(1),
                            uso = reader.GetInt32(2),
                            tipo = reader.GetInt32(3),
                            cantAmbientes = reader.GetInt32(4),
                            coordenadas = reader.GetString(5),
                            precio = reader.GetDouble(6),
                            disponible = reader.GetBoolean(7),
                            propietarioId = reader.GetInt32(8),
                            duenio = new Propietario {
                                idPropietario = reader.GetInt32(8),
                                nombre = reader.GetString(9),
                                apellido = reader.GetString(10)
                            }
                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Inmueble> ObtenerPorPropietario(Propietario p) {
            IList<Inmueble> res = new List<Inmueble>();
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    SELECT idInmueble, direccion, uso, tipo, cantAmbientes, coordenadas, precio, disponible, propietarioId, p.nombre, p.apellido
                    FROM Inmueble i
                    INNER JOIN Propietario p ON i.propietarioId = p.idPropietario
                    WHERE i.propietarioId = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = p.idPropietario;
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read()) {
                        Inmueble i = new Inmueble {
                            idInmueble = reader.GetInt32(0), 
                            direccion = reader.GetString(1),
                            uso = reader.GetInt32(2),
                            tipo = reader.GetInt32(3),
                            cantAmbientes = reader.GetInt32(4),
                            coordenadas = reader.GetString(5),
                            precio = reader.GetDouble(6),
                            disponible = reader.GetBoolean(7),
                            propietarioId = reader.GetInt32(8),
                            duenio = new Propietario {
                                idPropietario = reader.GetInt32(8),
                                nombre = reader.GetString(9),
                                apellido = reader.GetString(10)
                            }
                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Inmueble ObtenerPorId(int id) {
            Inmueble i = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    SELECT idInmueble, direccion, uso, tipo, cantAmbientes, coordenadas, precio, disponible, propietarioId, p.nombre, p.apellido
                    FROM Inmueble i
                    INNER JOIN Propietario p ON i.propietarioId = p.idPropietario
                    WHERE idInmueble = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read()) {
                        i = new Inmueble {
                            idInmueble = reader.GetInt32(0),
                            direccion = reader.GetString(1),
                            uso = reader.GetInt32(2),
                            tipo = reader.GetInt32(3),
                            cantAmbientes = reader.GetInt32(4),
                            coordenadas = reader.GetString(5),
                            precio = reader.GetDouble(6),
                            disponible = reader.GetBoolean(7),
                            propietarioId = reader.GetInt32(8),
                            duenio = new Propietario {
                                idPropietario = reader.GetInt32(8),
                                nombre = reader.GetString(9),
                                apellido = reader.GetString(10)
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return i;
        }
    }
}