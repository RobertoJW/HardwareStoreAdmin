using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwareStoreAdmin.Modelo
{
    public class ListaFavoritos
    {
        [Key]
        public int id_favorito { get; set; }

        public int userId { get; set; }
        public virtual Usuario? Usuario { get; set; }

        // Una lista puede tener muchos productos   
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
