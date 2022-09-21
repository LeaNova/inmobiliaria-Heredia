using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliaria_Heredia.Models {
    public class RepositorioPago : RepositorioBase, IRepositorioPago {

        public RepositorioPago(IConfiguration configuration) : base(configuration) {

        }

        public int Alta(Pago p) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    INSERT INTO Pago (fechaPago, importe, contratoId, detalle)
                    VALUES (@fechaPago, @importe, @detalle);
                    SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@fechaPago", p.fechaPago);
                    command.Parameters.AddWithValue("@importe", p.importe);
                    command.Parameters.AddWithValue("@contratoId", p.contratoId);
                    command.Parameters.AddWithValue("@detalle", p.detalle);

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.numPago = res;
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(int id) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    DELETE FROM Pago
                    WHERE numPago = @id";
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

        public int Modificar(Pago p) {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    UPDATE Pago
                    SET fechaPago = @fechaPago, importe = @importe, contratoId = @contratoId, detalle = @detalle
                    WHERE numPago = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@fechaPago", p.fechaPago);
                    command.Parameters.AddWithValue("@importe", p.importe);
                    command.Parameters.AddWithValue("@contratoId", p.contratoId);
                    command.Parameters.AddWithValue("@detalle", p.detalle);
                    command.Parameters.AddWithValue("@id", p.numPago);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Pago> ObtenerTodos() {
            IList<Pago> res = new List<Pago>();
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    SELECT numPago, fechaPago, importe, c.idContrato, detalle
                    FROM Pago p
                    INNER JOIN Contrato c ON p.contratoId = c.idContrato";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {

                    command.CommandType = CommandType.Text;
                    connection.Open();

                    var reader = command.ExecuteReader();
                    while (reader.Read()) {
                        Pago p = new Pago {
                            numPago = reader.GetInt32(0),
                            fechaPago = reader.GetDateTime(1),
                            importe = reader.GetDouble(2),
                            contratoId = reader.GetInt32(3),
                            detalle = reader.GetString(4),
                            contrato = new Contrato {
                                idContrato = reader.GetInt32(3)
                            }
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }
        
        public IList<Pago> ObtenerPorContrato(int id) {
            IList<Pago> res = new List<Pago>();
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    SELECT numPago, fechaPago, importe, c.idContrato, detalle
                    FROM Pago p
                    INNER JOIN Contrato c ON p.contratoId = c.idContrato
                    WHERE c.idContrato = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read()) {
                        Pago p = new Pago {
                            numPago = reader.GetInt32(0),
                            fechaPago = reader.GetDateTime(1),
                            importe = reader.GetDouble(2),
                            contratoId = reader.GetInt32(3),
                            detalle = reader.GetString(4),
                            contrato = new Contrato {
                                idContrato = reader.GetInt32(3)
                            }
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Pago ObtenerPorId(int id) {
            Pago p = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                string sql = @"
                    SELECT numPago, fechaPago, importe, c.idContrato, detalle
                    FROM Pago p
                    INNER JOIN Contrato c ON p.contratoId = c.idContrato
                    WHERE numPago = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection)) {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read()) {
                        p = new Pago {
                            numPago = reader.GetInt32(0),
                            fechaPago = reader.GetDateTime(1),
                            importe = reader.GetDouble(2),
                            contratoId = reader.GetInt32(3),
                            detalle = reader.GetString(4),
                            contrato = new Contrato {
                                idContrato = reader.GetInt32(3)
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }
    }
}