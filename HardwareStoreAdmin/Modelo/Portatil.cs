using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HardwareStoreAdmin.Enumerado;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareStoreAdmin.Modelo
{
    public class Portatil : Producto
    {
        public tipoPc tipoPc { get; set; }
        public int Pulgadas { get; set; }
        public int Bateria { get; set; }
        public string Resolucion { get; set; }

        public Portatil(string image, string companyBrand, string nameProduct, string category, decimal price, int pulgadas, int bateria, tipoPc tipoPc, string resolucion)
            : base(image, companyBrand, nameProduct, category, price)
        {
            this.tipoPc = tipoPc;
            this.Bateria = bateria;
            this.Pulgadas = pulgadas;
            this.Resolucion = resolucion;
        }
        public Portatil() {}
    }
}
