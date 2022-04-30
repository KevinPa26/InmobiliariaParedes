using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace InmobiliariaParedes.Models
{
	public class RepositorioInmueble
    {
		private readonly RepositorioPropietario repoPropietario = new RepositorioPropietario();

		string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariaparedes;SslMode=none";
		public int Alta(Inmueble inmueble)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"INSERT INTO inmueble (propietarioid, direccion, uso, tipo, cant_ambiente, precio, superficie) " +
					"VALUES (@propietarioid, @direccion, @uso, @tipo, @cant_ambiente, @precio, @superficie);" +
					"SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@propietarioid", inmueble.propietarioid);
					command.Parameters.AddWithValue("@direccion", inmueble.direccion);
					command.Parameters.AddWithValue("@uso", inmueble.uso);
					command.Parameters.AddWithValue("@tipo", inmueble.tipo);
					command.Parameters.AddWithValue("@cant_ambiente", inmueble.cant_ambiente);
					command.Parameters.AddWithValue("@precio", inmueble.precio);
                    command.Parameters.AddWithValue("@superficie", inmueble.superficie);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					inmueble.id = res;
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
				string sql = $"UPDATE Inmueble SET estado=@estado" + $" WHERE id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@estado", 2);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int Alquilado(int id)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"UPDATE Inmueble SET estado=@estado" + $" WHERE id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@estado", 3);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int Modificacion(Inmueble inmueble)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
                string sql = "UPDATE Inmueble SET " +
					"propietarioid=@propietarioid, direccion=@direccion, uso=@uso, tipo=@tipo, cant_ambiente=@cant_ambiente, precio=@precio, superficie=@superficie, estado=@estado, modificado=@modificado " +
					"WHERE id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@propietarioid", inmueble.propietarioid);
					command.Parameters.AddWithValue("@direccion", inmueble.direccion);
					command.Parameters.AddWithValue("@uso", inmueble.uso);
					command.Parameters.AddWithValue("@tipo", inmueble.tipo);
					command.Parameters.AddWithValue("@cant_ambiente", inmueble.cant_ambiente);
					command.Parameters.AddWithValue("@precio", inmueble.precio);
                    command.Parameters.AddWithValue("@superficie", inmueble.superficie);
					command.Parameters.AddWithValue("@estado", inmueble.estado);
					command.Parameters.AddWithValue("@modificado", System.DateTime.Now);
					command.Parameters.AddWithValue("@id", inmueble.id);
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Inmueble> ObtenerTodos()
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = "SELECT id, propietarioid, direccion, uso, tipo, cant_ambiente, precio, superficie, estado, creado, modificado" +
                    " FROM Inmueble";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble entidad = new Inmueble
						{
							id					= reader.GetInt32(0),
							propietarioid		= reader.GetInt32(1),
                            direccion			= reader.GetString(2),
                            uso					= reader.GetByte(3),
                            tipo				= reader.GetByte(4),
                            cant_ambiente		= reader.GetInt32(5),
                            precio				= reader.GetDouble(6),
                            superficie			= reader.GetInt32(7),
							estado				= reader.GetByte(8),
							creado				= reader.GetDateTime(9),
							modificado			= reader.GetDateTime(10),
                            duenio				= repoPropietario.ObtenerPorId(reader.GetInt32(1))
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Inmueble ObtenerPorId(int id)
		{
			Inmueble entidad = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
                string sql = $"SELECT id, propietarioid, direccion, uso, tipo, cant_ambiente, precio, superficie, estado, creado, modificado" +
                    $" FROM inmueble" +
                    $" WHERE id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						entidad = new Inmueble
						{
                            id				= reader.GetInt32(0),
							propietarioid	= reader.GetInt32(1),
                            direccion		= reader.GetString(2),
                            uso				= reader.GetByte(3),
                            tipo			= reader.GetByte(4),
                            cant_ambiente	= reader.GetInt32(5),
                            precio			= reader.GetDouble(6),
                            superficie		= reader.GetInt32(7),
							estado			= reader.GetByte(8),
							creado			= reader.GetDateTime(9),
							modificado		= reader.GetDateTime(10),
                            duenio			= repoPropietario.ObtenerPorId(reader.GetInt32(1))
                        };
					}
					connection.Close();
				}
			}
			return entidad;
        }

        public IList<Inmueble> BuscarPorPropietario(int idPropietario)
        {
            List<Inmueble> res = new List<Inmueble>();
            Inmueble entidad = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT id, propietarioid, direccion, uso, tipo, cant_ambiente, precio, superficie, estado, creado, modificado" +
                    $" FROM Inmueble" +
                    $" WHERE propietarioid=@idPropietario";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@idPropietario", MySqlDbType.Int32).Value = idPropietario;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        entidad = new Inmueble
                        {
                            id				= reader.GetInt32(0),
							propietarioid	= reader.GetInt32(1),
                            direccion		= reader.GetString(2),
                            uso				= reader.GetByte(3),
                            tipo			= reader.GetByte(4),
                            cant_ambiente	= reader.GetInt32(5),
                            precio			= reader.GetDouble(6),
                            superficie		= reader.GetInt32(7),
							estado			= reader.GetByte(8),
							creado			= reader.GetDateTime(9),
							modificado		= reader.GetDateTime(10),
                            duenio			= repoPropietario.ObtenerPorId(reader.GetInt32(1))
                        };
                        res.Add(entidad);
                    }
                    connection.Close();
                }
            }
            return res;
        }

		public IList<Inmueble> BuscarPorFecha(DateTime inicio, DateTime fin){

			List<Inmueble> res = new List<Inmueble>();
            Inmueble entidad = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM inmueble WHERE id in (SELECT resultado.id FROM (SELECT inmuebleid as id, MAX(fecha_fin) AS Fecha FROM contrato GROUP BY inmuebleid) as resultado WHERE resultado.Fecha < @inicio AND resultado.Fecha < @fin) || estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
					command.Parameters.Add("@inicio", MySqlDbType.Date).Value = inicio;
					command.Parameters.Add("@fin", MySqlDbType.Date).Value = fin;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        entidad = new Inmueble
                        {
                            id				= reader.GetInt32(0),
							propietarioid	= reader.GetInt32(1),
                            direccion		= reader.GetString(2),
                            uso				= reader.GetByte(3),
                            tipo			= reader.GetByte(4),
                            cant_ambiente	= reader.GetInt32(5),
                            precio			= reader.GetDouble(6),
                            superficie		= reader.GetInt32(7),
							estado			= reader.GetByte(8),
							creado			= reader.GetDateTime(9),
							modificado		= reader.GetDateTime(10),
                            duenio			= repoPropietario.ObtenerPorId(reader.GetInt32(1))
                        };
                        res.Add(entidad);
                    }
                    connection.Close();
                }
            }
            return res;
		}
    }
}
