using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliaria_Heredia.Models {
    public class RepositorioUsuario : RepositorioBase, IRepositorioUsuario {

        public RepositorioUsuario(IConfiguration configuration) : base(configuration) {

        }

        public int Alta(Usuario u) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    INSERT INTO Usuario (nombre, apellido, user, pass, avatar, access)
                    VALUES (@nombre, @apellido, @user, @pass, @avatar, @access);
                    SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", u.nombre);
                    command.Parameters.AddWithValue("@apellido", u.apellido);
                    command.Parameters.AddWithValue("@user", u.user);
                    command.Parameters.AddWithValue("@pass", u.pass);
                    if(String.IsNullOrEmpty(u.avatar)) {
                        command.Parameters.AddWithValue("@avatar", DBNull.Value);
                    } else {
                        command.Parameters.AddWithValue("@avatar", u.avatar);
                    }
                    command.Parameters.AddWithValue("@access", u.access);
                    
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    u.idUsuario = res;
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(int id) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    DELETE FROM Usuario
                    WHERE idUsuario = @id";
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

        public int Modificar(Usuario u) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    UPDATE Usuario
                    SET nombre = @nombre, apellido = @apellido, user = @user, pass = @pass, avatar = @avatar, access = @access
                    WHERE idUsuario = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", u.nombre);
                    command.Parameters.AddWithValue("@apellido", u.apellido);
                    command.Parameters.AddWithValue("@user", u.user);
                    command.Parameters.AddWithValue("@pass", u.pass);
                    command.Parameters.AddWithValue("@avatar", u.avatar);
                    command.Parameters.AddWithValue("@access", u.access);
                    command.Parameters.AddWithValue("@id", u.idUsuario);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        
        public IList<Usuario> ObtenerTodos() {
            IList<Usuario> res = new List<Usuario>();
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {

                string sql = @"
                    SELECT idUsuario, nombre, apellido, user, pass, avatar, access
                    FROM Usuario";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.CommandType = CommandType.Text;
                    connection.Open();

                    var reader = command.ExecuteReader();
                    while (reader.Read()) {
                        Usuario u = new Usuario {
                            idUsuario = reader.GetInt32(0),
                            nombre = reader.GetString(1),
                            apellido = reader.GetString(2),
                            user = reader.GetString(3),
                            pass = reader.GetString(4),
                            avatar = reader["avatar"].ToString(),
                            access = reader.GetInt32(6)
                        };
                        res.Add(u);
                    }
                    connection.Close();
                }
            }

            return res;
        }

        public Usuario ObtenerPorId(int id) {
            Usuario u = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {

                string sql = @"
                    SELECT idUsuario, nombre, apellido, user, pass, avatar, access
                    FROM Usuario
                    WHERE idUsuario = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    var reader = command.ExecuteReader();
                    if(reader.Read()) {
                        
                        u = new Usuario {
                            idUsuario = reader.GetInt32(0),
                            nombre = reader.GetString(1),
                            apellido = reader.GetString(2),
                            user = reader.GetString(3),
                            pass = reader.GetString(4),
                            avatar = reader["avatar"].ToString(),
                            access = reader.GetInt32(6)
                        };
                    }
                connection.Close();
                }
            }
            return u;
        }

        public Usuario ObtenerPorMail(string mail) {
            Usuario u = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {

                string sql = @"
                    SELECT idUsuario, nombre, apellido, user, pass, avatar, access
                    FROM Usuario
                    WHERE user = @mail";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.Parameters.Add("@mail", MySqlDbType.VarChar).Value = mail;
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    var reader = command.ExecuteReader();
                    if(reader.Read()) {
                        
                        u = new Usuario {
                            idUsuario = reader.GetInt32(0),
                            nombre = reader.GetString(1),
                            apellido = reader.GetString(2),
                            user = reader.GetString(3),
                            pass = reader.GetString(4),
                            avatar = reader["avatar"].ToString(),
                            access = reader.GetInt32(6)
                        };
                    }
                connection.Close();
                }
            }
            return u;
        }
    }
}