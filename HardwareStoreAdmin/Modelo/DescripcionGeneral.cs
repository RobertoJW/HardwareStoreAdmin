using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareStoreAdmin.Modelo
{
    public class DescripcionGeneral
    {
        [Key]
        public int IdDescripcionGeneral { get; set; }

        [ForeignKey("Producto")]
        public int IdProducto { get; set; } // Foreign key a Producto
        public string CPU { get; set; }
        public string RAM { get; set; }
        public string GPU { get; set; }
        public string Almacenamiento { get; set; }
        public string SistemaOperativo { get; set; }
        public string PlacaBase { get; set; }
        public double Peso { get; set; }
        public string Dimensiones { get; set; } // Ej: 40 x 21 x 45 cm

        public virtual Producto? Producto { get; set; }
        public DescripcionGeneral(string CPU, string RAM, string GPU, string Almacenamiento, string SistemaOperativo, string PlacaBase, double peso, string dimensiones)
        {
            this.CPU = CPU;
            this.RAM = RAM;
            this.GPU = GPU;
            this.Almacenamiento = Almacenamiento;
            this.SistemaOperativo = SistemaOperativo;
            this.PlacaBase = PlacaBase;
            this.Peso = peso;
            this.Dimensiones = dimensiones;
        }
        public DescripcionGeneral() { }
    }
}
