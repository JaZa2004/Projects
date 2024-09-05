using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Internship_Project.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public Client client { get; set; }
        public PhoneNumber phonenumber { get; set; }
        public DateTime BED { get; set; }
        public DateTime? EED { get; set; }
    }
}