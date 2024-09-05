using Internship_Project.Models;
using Internship_Project.Views;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Internship_Project.Controllers
{
    public class ObjectsMapper
    {
        public PhoneNumber MapPhoneNumber(SqlDataReader reader)
        {
            PhoneNumber phonenumber = new PhoneNumber();
            phonenumber.Id = (int)reader["PId"];
            phonenumber.Number = (string)reader["Number"];
            Device device = new Device();
            device.id = (int)reader["DId"];
            device.Name = (string)reader["Name"];
            phonenumber.Device = device;
            return phonenumber;
        }

        public Client MapClient(SqlDataReader reader)
        {
            return new Client
            {
                Id = (int)reader["Id"],
                Name = reader["Name"].ToString(),
                Type = (ClientType)reader["Type"],
                BirthDate = reader["BirthDate"] as DateTime?,
                ReservedPhoneNumbers = DatabaseHelper.ExecuteQuery("[getPhoneNumbersReservedByClient]", MapPhoneNumber, new SqlParameter("@ClientId", (int)reader["Id"]))
            };
        }

        public  Device DeviceMapper(SqlDataReader reader)
        {
            Device device = new Device();
            device.Name = reader["Name"].ToString();
            device.id = (int)reader["id"];
            device.phoneNumbers = DatabaseHelper.ExecuteQuery("getDevicePhoneNumbers", MapPhoneNumber, new SqlParameter("DeviceId", (int)reader["Id"]));
            return device;
        }

        public Reservation MapReservation(SqlDataReader reader)
        {
            Reservation reservation = new Reservation();
            reservation.Id = (int)reader["Id"];
            reservation.BED = (DateTime)(reader["BED"]);
            reservation.EED = reader["EED"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["EED"];
            reservation.client = DatabaseHelper.ExecuteQuery("getClientById", MapClient, new SqlParameter("@ClientId", (int)reader["ClientId"]))[0];
            reservation.phonenumber = DatabaseHelper.ExecuteQuery("getPhoneNumberById", MapPhoneNumber, new SqlParameter("@PhoneNumberId", (int)reader["PhoneNumberId"]))[0];
            return reservation;
        }

        public ClientTypeStatistic MapClientTypeStatistic(SqlDataReader reader)
        {
            ClientTypeStatistic statistic = new ClientTypeStatistic();
            statistic.Type = (int)reader["Type"];
            statistic.nbIndividuals = (int)reader["nbIndividuals"];
            return statistic;
        }

        public DeviceStatistic MapDeviceStatistic(SqlDataReader reader)
        {
            DeviceStatistic statistic = new DeviceStatistic();
            statistic.device = (Device)DatabaseHelper.ExecuteQuery("getDeviceById", DeviceMapper, new SqlParameter("@DeviceId", (int)reader["DeviceId"]))[0];
            statistic.nbReservedPn = (int)reader["nbReservedPn"];
            statistic.nbNonReservedPn = (int)reader["nbNonReservedPn"];
            return statistic;
        }
    }
}