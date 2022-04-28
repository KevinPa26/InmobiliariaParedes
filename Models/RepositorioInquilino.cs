using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;

namespace InmobiliariaParedes.Models
{
	public class RepositorioInquilino
	{
        string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariaparedes;SslMode=none";
		public int Alta(Inquilino i)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"INSERT INTO inquilino (dni, nombre, apellido, tel, email, direccion) " +
					$"VALUES (@dni, @nombre, @apellido, @tel, @email, @direccion);" +
					"SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@dni", i.dni);
					command.Parameters.AddWithValue("@nombre", i.nombre);
					command.Parameters.AddWithValue("@apellido", i.apellido);
					command.Parameters.AddWithValue("@tel", i.tel);
					command.Parameters.AddWithValue("@email", i.email);
					command.Parameters.AddWithValue("@direccion", i.direccion);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
                    i.id = res;
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
				string sql = $"UPDATE Inquilino SET estado=@estado" +
					$" WHERE id = @id";
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
		public int Modificacion(Inquilino i)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"UPDATE Inquilino SET nombre=@nombre, apellido=@apellido, dni=@dni, tel=@tel, email=@email, direccion=@direccion, estado=@estado, modificado=@modificado " +
					$"WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", i.nombre);
					command.Parameters.AddWithValue("@apellido", i.apellido);
					command.Parameters.AddWithValue("@dni", i.dni);
					command.Parameters.AddWithValue("@tel", i.tel);
					command.Parameters.AddWithValue("@email", i.email);
					command.Parameters.AddWithValue("@direccion", i.direccion);
                    command.Parameters.AddWithValue("@estado", i.estado);
					command.Parameters.AddWithValue("@modificado", System.DateTime.Now);
					command.Parameters.AddWithValue("@id", i.id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Inquilino> ObtenerTodos()
		{
			IList<Inquilino> res = new List<Inquilino>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id, nombre, apellido, dni, tel, email, direccion, estado, creado, modificado" +
                    $" FROM Inquilino";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inquilino i = new Inquilino
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
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Inquilino ObtenerPorId(int id)
		{
			Inquilino i = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id, nombre, apellido, dni, tel, email, direccion, estado, creado, modificado FROM Inquilino" +
					$" WHERE Id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new Inquilino
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
			return i;
        }

        public Inquilino ObtenerPorEmail(string email)
        {
            Inquilino i = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT id, nombre, apellido, dni, tel, email, direccion, estado, creado, modificado FROM Inquilino" +
                    $" WHERE email=@email";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Inquilino
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
            return i;
        }

        public IList<Inquilino> BuscarPorNombre(string nombre)
        {
            List<Inquilino> res = new List<Inquilino>();
            Inquilino i = null;
			nombre = "%" + nombre + "%";
			using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT id, nombre, apellido, dni, tel, email, direccion, estado, creado, modificado" + "FROM Inquilino" +
                    $" WHERE nombre LIKE @nombre OR apellido LIKE @nombre";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@nombre", MySqlDbType.VarChar).Value = nombre;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        i = new Inquilino
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
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}
