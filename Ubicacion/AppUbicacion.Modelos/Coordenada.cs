using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppUbicacion.Modelos
{
    public class Coordenada
    {
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}