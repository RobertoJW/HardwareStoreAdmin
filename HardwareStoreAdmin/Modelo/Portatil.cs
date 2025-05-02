using Microsoft.EntityFrameworkCore;
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
        public int pulgadas { get; set; }

        public Portatil(string image, string companyBrand, string nameProduct, string description, string category, double price, tipoPc tipoPc, int pulgadas)
            : base(image, companyBrand, nameProduct, description, category, price)
        {
            this.tipoPc = tipoPc;
            this.pulgadas = pulgadas;
        }
        public Portatil() {}
    }
}
