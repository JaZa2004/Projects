using Internship_Project.Models;
using System;
using System.Collections.Generic;

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ClientType Type { get; set; }
    public DateTime? BirthDate { get; set; }
    public List<PhoneNumber> ReservedPhoneNumbers { get; set; } = new List<PhoneNumber>();
}

public enum ClientType
{
    Individual = 0,
    Organization = 1
}

