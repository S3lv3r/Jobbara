using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobbara.Models
{
    internal class usersModel
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool isWorker { get; set; }
        public string office { get; set; }
        public bool alertWork { get; set; }
        public string curp { get; set; }
        public string rfc { get; set; }
        public string ine { get; set; }
        public string oficio { get; set; }
        public string domicilio { get; set; }
    }
}
