using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobbara.Models
{
    public class notificationWorkModel
    {
        public static string key { get; set; }
        public string userRequesting { get; set; }
        public officeModel officeRequired { get; set; }
        public string workDescription { get; set; }
        public string payment { get; set; }
        public string address { get; set; }
        public bool isAccepted { get; set; }
        public string whoAccepted { get; set; }
    }
}
