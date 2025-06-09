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
        public bool availableWork { get; set; }
        public bool isWorking { get; set; }
        public officeModel office { get; set; }
        public bool alertWork { get; set; }
    }
}
