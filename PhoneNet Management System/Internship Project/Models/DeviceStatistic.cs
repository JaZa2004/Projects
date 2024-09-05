using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Internship_Project.Models
{
    public class DeviceStatistic
    {
        public Device device { get; set; }
        public int nbNonReservedPn {  get; set; }
        public int nbReservedPn { get; set; }
    }
}