using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Internship_Project.Models
{
    public class PhoneNumber
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public Device Device { get; set; }
    }
}