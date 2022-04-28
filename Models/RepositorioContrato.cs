using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace InmobiliariaParedes.Models
{
	public class RepositorioContrato
    {
		private readonly RepositorioInquilino repoInquilino = new RepositorioInquilino();
        private readonly RepositorioInmueble repoInmueble = new RepositorioInmueble();

		string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariaparedes;SslMode=none";
		public int Alta(Contrato contrato)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"INSERT INTO contrato (inmuebleid, inquilinoid, fecha_inicio, fecha_fin, monto_mes, garante_nombre, garante_apellido, garante_dni, garante_tel) " +
					"VALUES (@inmuebleid, @inquilinoid, @fecha_inicio, @fecha_fin, @monto_mes, @garante_nombre, @garante_apellido, @garante_dni, @garante_tel);" +
					"SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@inmuebleid", contrato.inmuebleid);
					command.Parameters.AddWithValue("@inquilinoid", contrato.inquilinoid);
					command.Parameters.AddWithValue("@fecha_inicio", contrato.fecha_inicio);
					command.Parameters.AddWithValue("@fecha_fin", contrato.fecha_fin);
					command.Parameters.AddWithValue("@monto_mes", contrato.monto_mes);
					command.Parameters.AddWithValue("@garante_nombre", contrato.garante_nombre);
                    command.Parameters.AddWithValue("@garante_apellido", contrato.garante_apellido);
                    command.Parameters.AddWithValue("@garante_dni", contrato.garante_dni);
                    command.Parameters.AddWithValue("@garante_tel", contrato.garante_tel);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					contrato.id = res;
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
				string sql = $"DELETE FROM Contrato " + $"WHERE id = @id";
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
		public int Modificacion(Contrato contrato)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
                string sql = "UPDATE contrato SET " +
					"inmuebleid=@inmuebleid, inquilinoid=@inquilinoid, fecha_inicio=@fecha_inicio, fecha_fin=@fecha_fin, monto_mes=@monto_mes, garante_nombre=@garante_nombre, garante_apellido=@garante_apellido, garante_dni=@garante_dni, garante_tel=@garante_tel, modificado=@modificado " +
					"WHERE id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@inmubleid", contrato.inmuebleid);
					command.Parameters.AddWithValue("@inquilinoid", contrato.inquilinoid);
					command.Parameters.AddWithValue("@fecha_inicio", contrato.fecha_inicio);
					command.Parameters.AddWithValue("@fecha_fin", contrato.fecha_fin);
					command.Parameters.AddWithValue("@monto_mes", contrato.monto_mes);
					command.Parameters.AddWithValue("@garante_nombre", contrato.garante_nombre);
                    command.Parameters.AddWithValue("@garante_apellido", contrato.garante_apellido);
                    command.Parameters.AddWithValue("@garante_dni", contrato.garante_dni);
                    command.Parameters.AddWithValue("@garante_tel", contrato.garante_tel);
					command.Parameters.AddWithValue("@modificado", System.DateTime.Now);
					command.Parameters.AddWithValue("@id", contrato.id);
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Contrato> ObtenerTodos()
		{
			IList<Contrato> res = new List<Contrato>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = "SELECT id, inmuebleid, inquilinoid, fecha_inicio, fecha_fin, monto_mes, garante_nombre, garante_apellido, garante_dni, garante_tel, creado, modificado " +
                 "FROM contrato";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato contrato = new Contrato
						{
							id                  = reader.GetInt32(0),
                            inmuebleid          = reader.GetInt32(1),
                            inquilinoid         = reader.GetInt32(2),
                            fecha_inicio        = reader.GetDateTime(3),
                            fecha_fin           = reader.GetDateTime(4),
                            monto_mes           = reader.GetDouble(5),
                            garante_nombre      = reader.GetString(6),
                            garante_apellido    = reader.GetString(7),
                            garante_dni         = reader.GetString(8),
							garante_tel         = reader.GetString(9),
							creado              = reader.GetDateTime(10),
							modificado          = reader.GetDateTime(11),
                            inmueble            = repoInmueble.ObtenerPorId(reader.GetInt32(1)),
                            inquilino           = repoInquilino.ObtenerPorId(reader.GetInt32(2))
						};
						res.Add(contrato);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Contrato ObtenerPorId(int id)
		{
			Contrato contrato = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
                string sql = $"SELECT c.id, c.inmuebleid, c.inquilinoid, c.fecha_inicio, c.fecha_fin, c.monto_mes, c.garante_nombre, c.garante_apellido, c.garante_dni, c.garante_tel, c.creado, c.modificado " +
                    $"FROM contrato c INNER JOIN inmueble inm ON c.inmuebleid = inm.id INNER JOIN inquilino inq ON c.inquilinoid = inq.id" +
                    $" WHERE c.id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						contrato = new Contrato
						{
                            id                  = reader.GetInt32(0),
                            inmuebleid          = reader.GetInt32(1),
                            inquilinoid         = reader.GetInt32(2),
                            fecha_inicio        = reader.GetDateTime(3),
                            fecha_fin           = reader.GetDateTime(4),
                            monto_mes           = reader.GetDouble(5),
                            garante_nombre      = reader.GetString(6),
                            garante_apellido    = reader.GetString(7),
                            garante_dni         = reader.GetString(8),
							garante_tel         = reader.GetString(9),
							creado              = reader.GetDateTime(10),
							modificado          = reader.GetDateTime(11),
                            inmueble            = repoInmueble.ObtenerPorId(reader.GetInt32(1)),
                            inquilino           = repoInquilino.ObtenerPorId(reader.GetInt32(2))
                        };
					}
					connection.Close();
				}
			}
			return contrato;
        }

		public IList<Contrato> ObtenerPorInmueble(int id)
		{
			IList<Contrato> res = new List<Contrato>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
                string sql = $"SELECT id, inmuebleid, inquilinoid, fecha_inicio, fecha_fin, monto_mes, garante_nombre, garante_apellido, garante_dni, garante_tel, creado, modificado " +
                    $"FROM contrato" +
                    $" WHERE inmuebleid=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato contrato = new Contrato
						{
							id                  = reader.GetInt32(0),
                            inmuebleid          = reader.GetInt32(1),
                            inquilinoid         = reader.GetInt32(2),
                            fecha_inicio        = reader.GetDateTime(3),
                            fecha_fin           = reader.GetDateTime(4),
                            monto_mes           = reader.GetDouble(5),
                            garante_nombre      = reader.GetString(6),
                            garante_apellido    = reader.GetString(7),
                            garante_dni         = reader.GetString(8),
							garante_tel         = reader.GetString(9),
							creado              = reader.GetDateTime(10),
							modificado          = reader.GetDateTime(11),
                            inmueble            = repoInmueble.ObtenerPorId(reader.GetInt32(1)),
                            inquilino           = repoInquilino.ObtenerPorId(reader.GetInt32(2))
						};
						res.Add(contrato);
					}
					connection.Close();
				}
			}
			return res;
        }

		public IList<Contrato> ObtenerPorInquilino(int id)
		{
			IList<Contrato> res = new List<Contrato>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
                string sql = $"SELECT id, inmuebleid, inquilinoid, fecha_inicio, fecha_fin, monto_mes, garante_nombre, garante_apellido, garante_dni, garante_tel, creado, modificado " +
                    $"FROM contrato" +
                    $" WHERE inquilinoid=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato contrato = new Contrato
						{
							id                  = reader.GetInt32(0),
                            inmuebleid          = reader.GetInt32(1),
                            inquilinoid         = reader.GetInt32(2),
                            fecha_inicio        = reader.GetDateTime(3),
                            fecha_fin           = reader.GetDateTime(4),
                            monto_mes           = reader.GetDouble(5),
                            garante_nombre      = reader.GetString(6),
                            garante_apellido    = reader.GetString(7),
                            garante_dni         = reader.GetString(8),
							garante_tel         = reader.GetString(9),
							creado              = reader.GetDateTime(10),
							modificado          = reader.GetDateTime(11),
                            inmueble            = repoInmueble.ObtenerPorId(reader.GetInt32(1)),
                            inquilino           = repoInquilino.ObtenerPorId(reader.GetInt32(2))
						};
						res.Add(contrato);
					}
					connection.Close();
				}
			}
			return res;
        }
    }
}
