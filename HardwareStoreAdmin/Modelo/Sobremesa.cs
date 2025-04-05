using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HardwareStoreAdmin.Enumerado;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwareStoreAdmin.Modelo
{
    public class Sobremesa : Producto
    {
        public tipoPc tipoPc { get; set; }

        public Sobremesa(byte[] image, string companyBrand, string nameProduct, string description, string category, double price)
            : base(image, companyBrand, nameProduct, description, category, price)
        {
            this.tipoPc = tipoPc;
        }
        public Sobremesa() {}
    }
}
