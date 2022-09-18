using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliaria_Heredia.Models {
    public class RepositorioInquilino : RepositorioBase, IRepositorioInquilino {

        public RepositorioInquilino(IConfiguration configuration) : base(configuration) {

        }

        public int Alta(Inquilino i) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    INSERT INTO Inquilino (nombre, apellido, DNI, telefono, Email)
                    VALUES (@nombre, @apellido, @dni, @telefono, @email);
                    SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", i.nombre);
                    command.Parameters.AddWithValue("@apellido", i.apellido);
                    command.Parameters.AddWithValue("@dni", i.DNI);
                    command.Parameters.AddWithValue("@telefono", i.telefono);
                    command.Parameters.AddWithValue("@email", i.Email);
                    
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    i.idInquilino = res;
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(int id) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    DELETE FROM Inquilino
                    WHERE idInquilino = @id";
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

        public int Modificar(Inquilino i) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    UPDATE Inquilino
                    SET nombre = @nombre, apellido = @apellido, DNI = @dni, telefono = @telefono, Email = @email
                    WHERE idInquilino = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", i.nombre);
                    command.Parameters.AddWithValue("@apellido", i.apellido);
                    command.Parameters.AddWithValue("@dni", i.DNI);
                    command.Parameters.AddWithValue("@telefono", i.telefono);
                    command.Parameters.AddWithValue("@email", i.Email);
                    command.Parameters.AddWithValue("@id", i.idInquilino);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        
        public IList<Inquilino> ObtenerTodos() {
            IList<Inquilino> res = new List<Inquilino>();
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {

                string sql = @"
                    SELECT idInquilino, nombre, apellido, DNI, telefono, Email
                    FROM Inquilino";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.CommandType = CommandType.Text;
                    connection.Open();

                    var reader = command.ExecuteReader();
                    while (reader.Read()) {
                        Inquilino i = new Inquilino {
                            idInquilino = reader.GetInt32(0),
                            nombre = reader.GetString(1),
                            apellido = reader.GetString(2),
                            DNI = reader.GetString(3),
                            telefono = reader.GetString(4),
                            Email = reader.GetString(5)
                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Inquilino ObtenerPorId(int id) {
            Inquilino i = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    SELECT idInquilino, nombre, apellido, DNI, telefono, Email
                    FROM Inquilino
                    WHERE idInquilino = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    var reader = command.ExecuteReader();
                    if(reader.Read()) {
                        i = new Inquilino {
                            idInquilino = reader.GetInt32(0),
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
            return i;
        }
    }
}