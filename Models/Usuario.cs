using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace InmobiliariaParedes.Models
{
	public enum enRoles
    {
        SuperAdministrador = 1,
        Administrador = 2,
        Empleado = 3,
    }

    public class Usuario
    {
        [Key]
        [Display(Name = "Código")]
        public int id { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        public string apellido { get; set; }
        [Required, EmailAddress]
        public string email { get; set; }
        [Required, DataType(DataType.Password)]
        public string clave { get; set; }
        public string avatar { get; set; }
        [NotMapped]//Para EF
        public IFormFile avatarFile { get; set; }
        public int rol { get; set; }
        [NotMapped]//Para EF
        public string rolNombre => rol > 0 ? ((enRoles)rol).ToString() : "";

        public static IDictionary<int, string> ObtenerRoles()
        {
            SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
            Type tipoEnumRol = typeof(enRoles);
            foreach (var valor in Enum.GetValues(tipoEnumRol))
            {
                roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
            }
            return roles;
        }
    }
}