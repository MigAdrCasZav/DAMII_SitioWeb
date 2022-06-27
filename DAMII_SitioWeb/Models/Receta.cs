using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DAMII_SitioWeb.Models
{
    public class Receta
    {
        [DisplayName("CODIGO")]
        public int codigo { get; set; }
        [DisplayName("NOMBRE")]
        public String nombre { get; set; }
        [DisplayName("TIPO DE COMIDA")] 
        public String tipocomida { get; set; }
        [DisplayName("VIDEO")]
        public String video { get; set; }
        [DisplayName("FOTO")]
        public String foto { get; set; }
        [DisplayName("TIEMPO DE PREPARACION")]
        public String tiempoprepa { get; set; }
        [DisplayName("CANTIDAD")]
        public int cantidad { get; set; }
        [DisplayName("INGREDIENTES")]
        public String ingredienes { get; set; }
        [DisplayName("PREPARACION")]
        public String preparacion { get; set; }
        [DisplayName("CATEGORIA")]
        public string categoria { get; set; }

    }
}