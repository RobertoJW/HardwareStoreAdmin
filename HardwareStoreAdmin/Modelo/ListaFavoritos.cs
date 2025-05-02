using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareStoreAdmin.Modelo
{
    public class ListaFavoritos
    {
        [Key]
        public int id_favorito { get; set; }

        public int userId { get; set; }
        [ForeignKey("userId")]
        public virtual Usuario Usuario { get; set; }

        public int id_producto { get; set; }
        [ForeignKey("id_producto")]
        public virtual Producto Producto { get; set; }
    }
}
