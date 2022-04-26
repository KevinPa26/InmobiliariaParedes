using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;

namespace InmobiliariaParedes.Models
{
	public class RepositorioPropietario
	{
        string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariaparedes;SslMode=none";
		public int Alta(Propietario p)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Propietario (dni, nombre, apellido, tel, email, direccion) " +
					$"VALUES (@dni, @nombre, @apellido, @tel, @email, @direccion);" +
					"SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", p.dni);
					command.Parameters.AddWithValue("@nombre", p.nombre);
					command.Parameters.AddWithValue("@apellido", p.apellido);
					command.Parameters.AddWithValue("@tel", p.tel);
					command.Parameters.AddWithValue("@email", p.email);
					command.Parameters.AddWithValue("@direccion", p.direccion);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
                    p.id = res;
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
				string sql = $"UPDATE Propietario SET estado=@estado" +
					$"WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@estado", 0);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public int Modificacion(Propietario p)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"UPDATE Propietario SET nombre=@nombre, apellido=@apellido, dni=@dni, tel=@tel, email=@email, direccion=@direccion, estado=@estado, modificado=@modificado " +
					$"WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", p.nombre);
					command.Parameters.AddWithValue("@apellido", p.apellido);
					command.Parameters.AddWithValue("@dni", p.dni);
					command.Parameters.AddWithValue("@tel", p.tel);
					command.Parameters.AddWithValue("@email", p.email);
					command.Parameters.AddWithValue("@direccion", p.direccion);
                    command.Parameters.AddWithValue("@estado", p.estado);
					command.Parameters.AddWithValue("@modificado", System.DateTime.Now);
					command.Parameters.AddWithValue("@id", p.id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Propietario> ObtenerTodos()
		{
			IList<Propietario> res = new List<Propietario>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id, nombre, apellido, dni, tel, email, direccion, estado, creado, modificado" +
                    $" FROM Propietario";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Propietario p = new Propietario
						{
							id			= reader.GetInt32(0),
							nombre		= reader.GetString(1),
							apellido	= reader.GetString(2),
							dni			= reader.GetString(3),
							tel			= reader.GetString(4),
							email		= reader.GetString(5),
							direccion	= reader.GetString(6),
                            estado		= reader.GetByte(7),
                            creado		= reader.GetDateTime(8),
                            modificado	= reader.GetDateTime(9),
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Propietario ObtenerPorId(int id)
		{
			Propietario p = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id, nombre, apellido, dni, tel, email, direccion, estado, creado, modificado FROM propietario" +
					$" WHERE Id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Propietario
						{
							id			= reader.GetInt32(0),
							nombre		= reader.GetString(1),
							apellido	= reader.GetString(2),
							dni			= reader.GetString(3),
							tel			= reader.GetString(4),
							email		= reader.GetString(5),
							direccion	= reader.GetString(6),
                            estado		= reader.GetByte(7),
                            creado		= reader.GetDateTime(8),
                            modificado	= reader.GetDateTime(9),
						};
					}
					connection.Close();
				}
			}
			return p;
        }

        public Propietario ObtenerPorEmail(string email)
        {
            Propietario p = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT id, nombre, apellido, dni, tel, email, direccion, estado, creado, modificado FROM Propietario" +
                    $" WHERE email=@email";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Propietario
                        {
                            id			= reader.GetInt32(0),
							nombre		= reader.GetString(1),
							apellido	= reader.GetString(2),
							dni			= reader.GetString(3),
							tel			= reader.GetString(4),
							email		= reader.GetString(5),
							direccion	= reader.GetString(6),
                            estado		= reader.GetByte(7),
                            creado		= reader.GetDateTime(8),
                            modificado	= reader.GetDateTime(9),
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public IList<Propietario> BuscarPorNombre(string nombre)
        {
            List<Propietario> res = new List<Propietario>();
            Propietario p = null;
			nombre = "%" + nombre + "%";
			using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT id, nombre, apellido, dni, tel, email, direccion, estado, creado, modificado" +
                    $" WHERE nombre LIKE @nombre OR apellido LIKE @nombre";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@nombre", MySqlDbType.VarChar).Value = nombre;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        p = new Propietario
                        {
                            id			= reader.GetInt32(0),
							nombre		= reader.GetString(1),
							apellido	= reader.GetString(2),
							dni			= reader.GetString(3),
							tel			= reader.GetString(4),
							email		= reader.GetString(5),
							direccion	= reader.GetString(6),
                            estado		= reader.GetByte(7),
                            creado		= reader.GetDateTime(8),
                            modificado	= reader.GetDateTime(9),
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}
