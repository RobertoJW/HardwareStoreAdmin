using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HardwareStoreAdmin.Enumerado;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardwareStoreAdmin.Modelo;

namespace HardwareStoreAPI.Modelo
{
    public class Sobremesa : Producto
    {
        public tipoPc tipoPc { get; set; }

        public Sobremesa(string image, string companyBrand, string nameProduct, string category, decimal price, tipoPc tipoPc)
            : base(image, companyBrand, nameProduct, category, price)
        {
            this.tipoPc = tipoPc;
        }
        public Sobremesa() { }
    }
}
