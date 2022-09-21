using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliaria_Heredia.Models {
    public class RepositorioContrato : RepositorioBase, IRepositorioContrato {

        public RepositorioContrato(IConfiguration configuration) : base(configuration) {

        }

        public int Alta(Contrato c) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    INSERT INTO Contrato (fechaInicio, fechaFinal, alquilerMensual, inmuebleId, inquilinoId)
                    VALUES (@fechaInicio, @fechaFinal, @alquilerMensual @inmuebleId, @inmuebleId);
                    SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@fechaInicio", c.fechaInicio);
                    command.Parameters.AddWithValue("@fechaFinal", c.fechaFinal);
                    command.Parameters.AddWithValue("@alquilerMensual", c.alquilerMensual);
                    command.Parameters.AddWithValue("@inmuebleId", c.inmuebleId);
                    command.Parameters.AddWithValue("@inquilinoId", c.inquilinoId);

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    c.idContrato = res;
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(int id) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    DELETE FROM Contrato
                    WHERE idContrato = @id";
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

        public int Modificar(Contrato c) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    UPDATE Contrato
                    SET fechaInicio = @fechaInicio, fechaFinal = @fechaFinal, alquilerMensual = @alquilerMensual, inmuebleId = @inmuebleId, inquilinoId = @inquilinoId
                    WHERE idContrato = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@fechaInicio", c.fechaInicio);
                    command.Parameters.AddWithValue("@fechaFinal", c.fechaFinal);
                    command.Parameters.AddWithValue("@alquilerMensual", c.alquilerMensual);
                    command.Parameters.AddWithValue("@inmuebleId", c.inmuebleId);
                    command.Parameters.AddWithValue("@inquilinoId", c.inquilinoId);
                    command.Parameters.AddWithValue("@id", c.idContrato);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Contrato> ObtenerTodos() {
            IList<Contrato> res = new List<Contrato>();
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    SELECT idContrato, fechaInicio, fechaFinal, alquilerMensual, inmuebleId, inquilinoId, inm.direccion, inq.nombre, inq.apellido
                    FROM Contrato c
                    INNER JOIN Inmueble inm ON c.inmuebleId = inm.idInmueble
                    INNER JOIN Inquilino inq ON c.inquilinoId = inq.idInquilino";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.CommandType = CommandType.Text;
                    connection.Open();

                    var reader = command.ExecuteReader();
                    while (reader.Read()) {
                        Contrato c = new Contrato {
                            idContrato = reader.GetInt32(0),
                            fechaInicio = reader.GetDateTime(1),
                            fechaFinal = reader.GetDateTime(2),
                            alquilerMensual = reader.GetDouble(3),
                            inmuebleId = reader.GetInt32(4),
                            inquilinoId = reader.GetInt32(5),
                            propiedad = new Inmueble {
                                idInmueble = reader.GetInt32(4),
                                direccion = reader.GetString(6)
                            },
                            inquilino = new Inquilino {
                                idInquilino = reader.GetInt32(5),
                                nombre = reader.GetString(7),
                                apellido = reader.GetString(8)
                            }
                        };
                        res.Add(c);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Contrato ObtenerPorId(int id) {
            Contrato c = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    SELECT idContrato, fechaInicio, fechaFinal, alquilerMensual, inmuebleId, inquilinoId, inm.direccion, inq.nombre, inq.apellido
                    FROM Contrato c
                    INNER JOIN Inmueble inm ON c.inmuebleId = inm.idInmueble
                    INNER JOIN Inquilino inq ON c.inquilinoId = inq.idInquilino
                    WHERE idContrato = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read()) {
                        c = new Contrato {
                            idContrato = reader.GetInt32(0),
                            fechaInicio = reader.GetDateTime(1),
                            fechaFinal = reader.GetDateTime(2),
                            alquilerMensual = reader.GetDouble(3),
                            inmuebleId = reader.GetInt32(4),
                            inquilinoId = reader.GetInt32(5),
                            propiedad = new Inmueble {
                                idInmueble = reader.GetInt32(4),
                                direccion = reader.GetString(6)
                            },
                            inquilino = new Inquilino {
                                idInquilino = reader.GetInt32(5),
                                nombre = reader.GetString(7),
                                apellido = reader.GetString(8)
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return c;
        }
    }
}