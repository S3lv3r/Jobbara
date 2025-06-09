using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobbara.Models
{
    public class ChambeadorModel
    {
        public string Username { get; set; } // Usa UserSessionData.username_usd para identificar
        public string Oficio { get; set; }
        public string INE { get; set; }
        public string CURP { get; set; }
        public string Domicilio { get; set; }
        public string RFC { get; set; }
        public bool IsWorker { get; set; }
    }

}
