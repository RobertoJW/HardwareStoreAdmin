using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareStoreAdmin.Modelo
{
    public class CarritoCompra
    {
        [Key]
        public int id_carrito { get; set; }
        public int cantidad { get; set; }
        public int? userId { get; set; }

        public virtual Usuario? Usuario { get; set; }
        // Una lista de favoritos puede tener multiples productos.
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}

