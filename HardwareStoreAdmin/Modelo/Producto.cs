using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareStoreAdmin.Modelo
{
    public abstract class Producto
    {
        [Key] // Key tratará a id_producto como clave primaria y autoincremental
        public int IdProducto { get; set; }
        public string ImagenUrl { get; set; }
        public string NombreEmpresa { get; set; }
        public string NombreProducto { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }

        // Relacion uno a uno
        public virtual DescripcionGeneral? DescripcionGeneral { get; set; }

        public Producto(string ImagenUrl, string NombreEmpresa, string NombreProducto, string Categoria, decimal Precio)
        {
            this.ImagenUrl = ImagenUrl;
            this.NombreEmpresa = NombreEmpresa;
            this.NombreProducto = NombreProducto;
            this.Categoria = Categoria;
            this.Precio = Precio;
        }

        public Producto() { }
    }
}
