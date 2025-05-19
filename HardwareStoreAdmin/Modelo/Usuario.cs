using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareStoreAdmin.Modelo
{
    public class Usuario
    {
        [Key]
        public int userId { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
        // la contraseña no puede ser modificado. 
        public string password { get; private set; }
        // añadimos '?' para permitir valores nulos para fotos de perfil. 
        public string? UrlprofilePhoto { get; set; }

        public CarritoCompra CarritoCompra { get; set; }
        public ListaFavoritos ListaFavoritos { get; set; }

        public Usuario(string email, string userName, string password, string? UrlprofilePhoto)
        {
            this.email = email;
            this.userName = userName;
            this.password = password;
            this.UrlprofilePhoto = UrlprofilePhoto;
        }

        public Usuario() { }
    }
}
