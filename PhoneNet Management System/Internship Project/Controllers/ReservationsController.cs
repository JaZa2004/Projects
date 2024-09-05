using Internship_Project.Models;
using Internship_Project.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Internship_Project.Controllers
{
    [RoutePrefix("api/Reservations")]
    public class ReservationsController : ApiController
    {
        ObjectsMapper Om = new ObjectsMapper();
        [Route("getAllReservations")]
        [HttpGet]
        public IHttpActionResult GetAllReservations()
        {
            List<Reservation> reservations;
            string query = "GetAllReservations";
            reservations = DatabaseHelper.ExecuteQuery(query, Om.MapReservation);
            return Ok(reservations);
        }

        [Route("getFilteredReservations")]
        [HttpGet]
        public IHttpActionResult GetFilteredReservations(int clientId , int PhoneNumberId)
        {
            List<Reservation> reservations;
            string query = "getFilteredReservations";
            if (clientId != -1 && PhoneNumberId != -1)
            {
                SqlParameter clientIdParameter = new SqlParameter("@ClientId", clientId);
                SqlParameter phoneNumberIdParameter = new SqlParameter("@PhoneNumberId", PhoneNumberId);
                reservations = DatabaseHelper.ExecuteQuery(query, Om.MapReservation, clientIdParameter, phoneNumberIdParameter);
            }
            else if (clientId != -1)
            {
                SqlParameter clientIdParameter = new SqlParameter("@ClientId", clientId);
                reservations = DatabaseHelper.ExecuteQuery(query, Om.MapReservation, clientIdParameter);
            }
            else if (PhoneNumberId != -1)
            {
                SqlParameter phoneNumberIdParameter = new SqlParameter("@PhoneNumberId", PhoneNumberId);
                reservations = DatabaseHelper.ExecuteQuery(query, Om.MapReservation, phoneNumberIdParameter);
            }
            else
            {
                reservations = DatabaseHelper.ExecuteQuery(query, Om.MapReservation);
            }
            return Ok(reservations);
        }


        [HttpPost]
        [Route("addReservation")]
        public IHttpActionResult AddReservation(dynamic reservationData)
        {
            int ClientId = reservationData.ClientId;/////////////
            int PhoneNumberId = reservationData.PhoneNumberId;////////////////
            string query = "AddReservation";
            SqlParameter clientIdParameter = new SqlParameter("@ClientId",ClientId);
            SqlParameter PhoneNumberIdParameter = new SqlParameter("@PhoneNumberId", PhoneNumberId);
            SqlParameter BEDParameter = new SqlParameter("@BED",DateTime.Now);
            DatabaseHelper.ExecuteNonQuery(query, clientIdParameter, PhoneNumberIdParameter, BEDParameter);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateReservation")]
        public IHttpActionResult UpdateReservation(dynamic UnreservationData)
        {
            int ClientId = UnreservationData.ClientId;/////////////
            int PhoneNumberId = UnreservationData.PhoneNumberId;////////////////
            string query = "UpdateReservation";
            SqlParameter clientIdParameter = new SqlParameter("@ClientId", ClientId);
            SqlParameter PhoneNumberIdParameter = new SqlParameter("@PhoneNumberId", PhoneNumberId);
            SqlParameter EEDParameter = new SqlParameter("@EED", DateTime.Now);
            DatabaseHelper.ExecuteNonQuery(query, clientIdParameter, PhoneNumberIdParameter, EEDParameter);
            return Ok();
        }

        /*private Reservation MapReservation(SqlDataReader reader)
        {
            Reservation reservation=new Reservation();
            reservation.Id = (int)reader["Id"];
            reservation.BED = (DateTime)(reader["BED"]);
            reservation.clientName = (string)(reader["Name"]);
            reservation.Number = (string)reader["Number"];
            return reservation;
        }*/

    }
}
