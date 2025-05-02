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
    public class Producto
    {
        [Key] // Key tratará a id_producto como clave primaria y autoincremental
        public int id_producto { get; set; }
        public string imageUrl { get; set; }
        public string companyBrand { get; set; }
        public string nameProduct { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public double price { get; set; }

        public string TipoProducto { get; set; }

        public Producto(string imageUrl, string companyBrand, string nameProduct, string description, string category, double price)
        {
            this.imageUrl = imageUrl;
            this.companyBrand = companyBrand;
            this.nameProduct = nameProduct;
            this.description = description;
            this.category = category;
            this.price = price;
        }

        public Producto() { }
    }
}
