using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliaria_Heredia.Models {
    public class RepositorioPropietario : RepositorioBase {

        public RepositorioPropietario() : base() {

        }

        public int Alta(Propietario p) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    INSERT INTO Propietario (nombre, apellido, DNI, telefono, Email)
                    VALUES (@nombre, @apellido, @dni, @telefono, @email);
                    SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", p.nombre);
                    command.Parameters.AddWithValue("@apellido", p.apellido);
                    command.Parameters.AddWithValue("@dni", p.DNI);
                    command.Parameters.AddWithValue("@telefono", p.telefono);
                    command.Parameters.AddWithValue("@email", p.Email);
                    
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.idPropietario = res;
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(int id) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    DELETE FROM propietario
                    WHERE idPropietario = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue(@"id", id);
                    
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificar(Propietario p) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    UPDATE propietario
                    SET nombre = @nombre, apellido = @apellido, DNI = @dni, telefono = @telefono, Email = @email
                    WHERE idPropietario = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", p.nombre);
                    command.Parameters.AddWithValue("@apellido", p.apellido);
                    command.Parameters.AddWithValue("@dni", p.DNI);
                    command.Parameters.AddWithValue("@telefono", p.telefono);
                    command.Parameters.AddWithValue("@email", p.Email);
                    command.Parameters.AddWithValue("@id", p.idPropietario);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        public IList<Propietario> ObtenerTodos() {
            IList<Propietario> res = new List<Propietario>();
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {

                string sql = @"
                    SELECT idPropietario, nombre, apellido, DNI, telefono, Email
                    FROM Propietario";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.CommandType = CommandType.Text;
                    connection.Open();

                    var reader = command.ExecuteReader();
                    while (reader.Read()) {
                        Propietario p = new Propietario {
                            idPropietario = reader.GetInt32(0),
                            nombre = reader.GetString(1),
                            apellido = reader.GetString(2),
                            DNI = reader.GetString(3),
                            telefono = reader.GetString(4),
                            Email = reader.GetString(5)
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }

            return res;
        }

        public Propietario ObtenerPorId(int id) {
            Propietario p = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {

                string sql = @"
                    SELECT idPropietario, nombre, apellido, DNI, telefono, Email
                    FROM propietario
                    WHERE idPropietario = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    var reader = command.ExecuteReader();
                    if(reader.Read()) {
                        
                        p = new Propietario {
                            idPropietario = reader.GetInt32(0),
                            nombre = reader.GetString(1),
                            apellido = reader.GetString(2),
                            DNI = reader.GetString(3),
                            telefono = reader.GetString(4),
                            Email = reader.GetString(5)
                        };
                    }
                connection.Close();
                }
            }
            return p;
        }
    }
}