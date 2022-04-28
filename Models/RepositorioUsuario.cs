using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace InmobiliariaParedes.Models
{
	public class RepositorioUsuario
    {
		private readonly RepositorioInquilino repoInquilino = new RepositorioInquilino();
        private readonly RepositorioInmueble repoInmueble = new RepositorioInmueble();

		string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariaparedes;SslMode=none";
		public int Alta(Usuario e)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"INSERT INTO usuario (nombre, apellido, avatar, email, clave, rol) " +
					$"VALUES (@nombre, @apellido, @avatar, @email, @clave, @rol);" +
					"SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.nombre);
					command.Parameters.AddWithValue("@apellido", e.apellido);
					if (String.IsNullOrEmpty(e.avatar))
						command.Parameters.AddWithValue("@avatar", DBNull.Value);
					else
						command.Parameters.AddWithValue("@avatar", e.avatar);
					command.Parameters.AddWithValue("@email", e.email);
					command.Parameters.AddWithValue("@clave", e.clave);
					command.Parameters.AddWithValue("@rol", e.rol);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					e.id = res;
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
				string sql = $"DELETE FROM Usuario WHERE id = @id";
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
		public int Modificacion(Usuario e)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"UPDATE usuario SET nombre=@nombre, apellido=@apellido, avatar=@avatar, email=@email, clave=@clave, rol=@rol " +
					$"WHERE id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.nombre);
					command.Parameters.AddWithValue("@apellido", e.apellido);
					command.Parameters.AddWithValue("@avatar", e.avatar);
					command.Parameters.AddWithValue("@email", e.email);
					command.Parameters.AddWithValue("@clave", e.clave);
					command.Parameters.AddWithValue("@rol", e.rol);
					command.Parameters.AddWithValue("@id", e.id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Usuario> ObtenerTodos()
		{
			IList<Usuario> res = new List<Usuario>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id, nombre, apellido, avatar, email, clave, rol" +
					$" FROM usuario";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Usuario e = new Usuario
						{
							id = reader.GetInt32(0),
							nombre = reader.GetString(1),
							apellido = reader.GetString(2),
							avatar = reader["Avatar"].ToString(),
							email = reader.GetString(4),
							clave = reader.GetString(5),
							rol = reader.GetInt32(6),
						};
						res.Add(e);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Usuario ObtenerPorId(int id)
		{
			Usuario e = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id, nombre, apellido, avatar, email, clave, rol FROM usuario" +
					$" WHERE id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Usuario
						{
							id			= reader.GetInt32(0),
							nombre		= reader.GetString(1),
							apellido	= reader.GetString(2),
							avatar		= reader["Avatar"].ToString(),
							email		= reader.GetString(4),
							clave		= reader.GetString(5),
							rol			= reader.GetInt32(6),
						};
					}
					connection.Close();
				}
			}
			return e;
		}

		public Usuario ObtenerPorEmail(string email)
		{
			Usuario e = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id, nombre, apellido, avatar, email, clave, rol FROM usuario" +
					$" WHERE email=@email";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Usuario
						{
							id			= reader.GetInt32(0),
							nombre		= reader.GetString(1),
							apellido	= reader.GetString(2),
							avatar		= reader["avatar"].ToString(),
							email		= reader.GetString(4),
							clave		= reader.GetString(5),
							rol			= reader.GetInt32(6),
						};
					}
					connection.Close();
				}
			}
			return e;
		}
    }
}
