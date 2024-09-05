using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Internship_Project.Models
{
    public class Device
    {
        public string Name { get; set; }
        public int id { get; set; }
        public List<PhoneNumber> phoneNumbers { get; set; }
    }
}