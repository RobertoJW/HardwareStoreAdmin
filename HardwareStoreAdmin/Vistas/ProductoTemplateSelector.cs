using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardwareStoreAdmin.Modelo;

namespace HardwareStoreAdmin.Vistas
{
    public class ProductoTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MovilTemplate { get; set; }
        public DataTemplate PortatilTemplate { get; set; }
        public DataTemplate SobremesaTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return item switch
            {
                Movil => MovilTemplate,
                Portatil => PortatilTemplate,
                Sobremesa => SobremesaTemplate,
                _ => null
            };
        }
    }
}
