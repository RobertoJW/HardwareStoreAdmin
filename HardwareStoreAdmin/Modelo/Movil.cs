using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareStoreAdmin.Modelo
{
    public class Movil : Producto
    {
        public int pulgadas { get; set; }

        public Movil(string image, string companyBrand, string nameProduct, string description, string category, double price, int pulgadas)
            : base(image, companyBrand, nameProduct, description, category, price)
        {
            this.pulgadas = pulgadas;
        }

        public Movil() { }
    }
}
