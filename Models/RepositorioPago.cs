using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace InmobiliariaParedes.Models
{
	public class RepositorioPago
    {
		private readonly RepositorioContrato repoContrato = new RepositorioContrato();

		string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariaparedes;SslMode=none";
		public int Alta(Pago pago)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"INSERT INTO pago (contratoid, fecha, importe, num_pago) " +
					"VALUES (@contratoid, @fecha, @importe, @num_pago);" +
					"SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@contratoid", pago.contratoid);
					command.Parameters.AddWithValue("@fecha", pago.fecha);
					command.Parameters.AddWithValue("@importe", pago.importe);
					command.Parameters.AddWithValue("@num_pago", pago.num_pago);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					pago.id = res;
					connection.Close();
				}
			}
			return res;
		}
		public int Baja(int id)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Pago " + $"WHERE id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public int Modificacion(Pago pago)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
                string sql = "UPDATE pago SET " +
					"contratoid=@contratoid, fecha=@fecha, importe=@importe, num_pago=@num_pago, modificado=@modificado " +
					"WHERE id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@contratoid", pago.contratoid);
					command.Parameters.AddWithValue("@fecha", pago.fecha);
					command.Parameters.AddWithValue("@importe", pago.importe);
					command.Parameters.AddWithValue("@num_pago", pago.num_pago);
					command.Parameters.AddWithValue("@modificado", System.DateTime.Now);
					command.Parameters.AddWithValue("@id", pago.id);
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Pago> ObtenerTodos()
		{
			IList<Pago> res = new List<Pago>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = "SELECT id, contratoid, fecha, importe, num_pago, creado, modificado " +
                 "FROM pago";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago pago = new Pago
						{
							id              = reader.GetInt32(0),
                            contratoid      = reader.GetInt32(1),
                            fecha           = reader.GetDateTime(2),
                            importe         = reader.GetDouble(3),
                            num_pago        = reader.GetInt32(4),
                            creado          = reader.GetDateTime(5),
                            modificado      = reader.GetDateTime(6),
                            contrato        = repoContrato.ObtenerPorId(reader.GetInt32(1))
						};
						res.Add(pago);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Pago ObtenerPorId(int id)
		{
			Pago pago = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
                string sql = $"SELECT id, contratoid, fecha, importe, num_pago, creado, modificado " +
                    $"FROM pago" +
                    $" WHERE id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						pago = new Pago
						{
                            id              = reader.GetInt32(0),
                            contratoid      = reader.GetInt32(1),
                            fecha           = reader.GetDateTime(2),
                            importe         = reader.GetDouble(3),
                            num_pago        = reader.GetInt32(4),
                            creado          = reader.GetDateTime(5),
                            modificado      = reader.GetDateTime(6),
                            contrato        = repoContrato.ObtenerPorId(reader.GetInt32(1))
                        };
					}
					connection.Close();
				}
			}
			return pago;
        }

		public IList<Pago> ObtenerPorContrato(int id)
		{
			IList<Pago> res = new List<Pago>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
                string sql = $"SELECT id, contratoid, fecha, importe, num_pago, creado, modificado " +
                    $"FROM pago" +
                    $" WHERE contratoid=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago pago = new Pago
						{
							id              = reader.GetInt32(0),
                            contratoid      = reader.GetInt32(1),
                            fecha           = reader.GetDateTime(2),
                            importe         = reader.GetDouble(3),
                            num_pago        = reader.GetInt32(4),
                            creado          = reader.GetDateTime(5),
                            modificado      = reader.GetDateTime(6),
                            contrato        = repoContrato.ObtenerPorId(reader.GetInt32(1))
						};
						res.Add(pago);
					}
					connection.Close();
				}
			}
			return res;
        }
    }
}
