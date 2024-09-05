using Internship_Project.Models;
using Internship_Project.Views;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Internship_Project.Controllers
{
    [RoutePrefix("api/PhoneNumbers")]
    public class PhoneNumbersController : ApiController
    {
        ObjectsMapper Om = new ObjectsMapper();
        [HttpGet]
        [Route("getAllPhoneNumbers")]
        public IHttpActionResult getAllPhoneNumbers()
        {
            string query = "GetAllPhoneNumbers";
            var phoneNumbers = DatabaseHelper.ExecuteQuery(query, Om.MapPhoneNumber);
            return Ok(phoneNumbers);
        }
        [HttpGet]
        [Route("getNonReservedPhoneNumbers")]
        public IHttpActionResult getNonReservedPhoneNumbers()
        {
            string query = "getNonReservedPhoneNumbers";
            var phoneNumbers = DatabaseHelper.ExecuteQuery(query, Om.MapPhoneNumber);
            return Ok(phoneNumbers);
        }

        [HttpGet]
        [Route("getPhoneNumbersReservedByClient")]
        public IHttpActionResult getPhoneNumbersReservedByClient(int Id)
        {
            string query = "getPhoneNumbersReservedByClient";
            var phoneNumbers = DatabaseHelper.ExecuteQuery(query, Om.MapPhoneNumber, new SqlParameter("@ClientId", Id));
            return Ok(phoneNumbers);
        }

        [HttpGet]
        [Route("getFilteredPhoneNumbers")]
        public IHttpActionResult getFilteredPhoneNumbers(string Number, int DeviceId)
        {
            List<PhoneNumber> phoneNumbers;
            if (Number == "" && DeviceId == -1)
            {
                phoneNumbers = DatabaseHelper.ExecuteQuery("GetAllPhoneNumbers", Om.MapPhoneNumber);
            }
            else if (Number == "")
            {
                SqlParameter DeviceIdParameter = new SqlParameter("@DeviceId", DeviceId);
                phoneNumbers = DatabaseHelper.ExecuteQuery("GetFilteredPhoneNumbers", Om.MapPhoneNumber, DeviceIdParameter);
            }
            else if (DeviceId == -1)
            {
                SqlParameter NumberParameter = new SqlParameter("@Number", Number);
                phoneNumbers = DatabaseHelper.ExecuteQuery("GetFilteredPhoneNumbers", Om.MapPhoneNumber, NumberParameter);
            }
            else
            {
                SqlParameter DeviceIdParameter = new SqlParameter("@DeviceId", DeviceId);
                SqlParameter NumberParameter = new SqlParameter("@Number", Number);
                phoneNumbers = DatabaseHelper.ExecuteQuery("GetFilteredPhoneNumbers", Om.MapPhoneNumber, NumberParameter, DeviceIdParameter);
            }

            return Ok(phoneNumbers);
        }

        [HttpPost]
        [Route("addPhoneNumber")]
        public IHttpActionResult addPhoneNumber([FromBody] PhoneNumber phoneNumber)
        {
            if (phoneNumber == null)
            {
                return BadRequest();
            }
            if (phoneNumber.Device == null)
            {
                return BadRequest("Device is required.");
            }

            string query = "AddPhoneNumber";

            SqlParameter[] parameters = {
                new SqlParameter("@PhoneNumber", phoneNumber.Number),
                new SqlParameter("@DeviceId",phoneNumber.Device.id)
            };
            int rowsaffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            if (rowsaffected > 0)
            {
                var phoneNumbers = DatabaseHelper.ExecuteQuery("GetAllPhoneNumbers", Om.MapPhoneNumber);
                return Ok(phoneNumbers);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("updatePhoneNumber")]
        public IHttpActionResult updatePhoneNumber(PhoneNumber phoneNumber)
        {
            if (phoneNumber == null)
            {
                return BadRequest();
            }
            string query = "UpdatePhoneNumber";
            SqlParameter[] parameters =
            {
                new SqlParameter("@PhoneNumber",phoneNumber.Number),
                new SqlParameter("@id",phoneNumber.Id),
                new SqlParameter("@DeviceId",phoneNumber.Device.id)
            };
            int rowsaffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            if (rowsaffected > 0)
            {
                var phoneNumbers = DatabaseHelper.ExecuteQuery("GetAllPhoneNumbers", Om.MapPhoneNumber);
                return Ok(phoneNumbers);
            }
            else { return BadRequest(); }
        }

        [HttpGet]
        [Route("CheckDuplicatePhoneNumber")]
        public IHttpActionResult CheckDuplicatePhoneNumber(string Number)
        {
            string query = "CheckPhoneNumberDuplicate";
            SqlParameter parameter = new SqlParameter("@Number", Number);
            int NumberPresence = DatabaseHelper.ExecuteScalarQuery(query, parameter);
            if (NumberPresence > 0)
            {
                return Conflict();
            }
            else
            {
                return Ok();
            }
        }
    }
}
