using Internship_Project.Models;
using Internship_Project.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace Internship_Project.Controllers
{
    [RoutePrefix("api/devices")]
    public class DevicesController : ApiController
    {
        ObjectsMapper Om = new ObjectsMapper();
        [Route("getAllDevices")]
        [HttpGet]
        public IHttpActionResult GetAllDevices()
        {
            List<Device> devices;
            string query = "GetDevices";
            devices = DatabaseHelper.ExecuteQuery(query, Om.DeviceMapper);
            return Ok(devices);
        }

        [Route("getFilteredDevices")]
        [HttpGet]
        public IHttpActionResult GetFilteredDevices(string searchtext)
        {
            List<Device> devices;
            string query = "GetDevices";
            if (string.IsNullOrEmpty(searchtext))
            {
                 devices = DatabaseHelper.ExecuteQuery(query, Om.DeviceMapper);

            }
            else
            {
                SqlParameter [] parameters = {
                    new SqlParameter("@searchtext", searchtext),
                    };
                devices = DatabaseHelper.ExecuteQuery(query, Om.DeviceMapper,parameters);

            }
           return Ok(devices);
        }

        [HttpPost]
        [Route("addDevice")]
        public IHttpActionResult AddDevice([FromBody] Device device)
        {

            if (device == null || string.IsNullOrEmpty(device.Name))
            {
                return BadRequest("Problem Occurred While Adding the Device");
            }
            try
            {
                // string quey ="Insert into Device (Name) values (@Name);";
                string query = "AddDevice";

                SqlParameter[] parameters = {
                    new SqlParameter("@Name", device.Name),
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
                if (rowsAffected > 0)
                {
                    string SecondQuery = "GetDevices";
                    var devices = DatabaseHelper.ExecuteQuery(SecondQuery, Om.DeviceMapper);
                    return Ok(devices);
                }
                else
                {
                    return BadRequest("Device was not Added Due to Some Errors");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPut]
        [Route("updateDevice")]
        public IHttpActionResult UpdateDevice([FromBody] Device UpdatedDevice)
        {
            if (UpdatedDevice == null || string.IsNullOrEmpty(UpdatedDevice.Name))
            {
                return BadRequest("Device must have a name.");
            }

            try
            {
                // string query = "update Device set Name = (@Name) where id = (@id);";
                string query = "UpdateDevice";
                SqlParameter[] parameters =
                {
                    new SqlParameter ("@Name",UpdatedDevice.Name),
                    new SqlParameter("@id",UpdatedDevice.id)
                };
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
                if(rowsAffected > 0)
                {
                    string SecondQuery = "GetDevices";
                    var devices = DatabaseHelper.ExecuteQuery(SecondQuery, Om.DeviceMapper);
                    return Ok(devices);
                }
                return BadRequest("Device was not Updated Due to Some Errors");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("CheckDuplicateDevice")]
        public IHttpActionResult CheckDuplicateDevice(string DeviceName)
        {
            string query = "CheckDeviceDuplicate";
            SqlParameter parameter = new SqlParameter ("@DeviceName",DeviceName);
            int DevicesPresence = DatabaseHelper.ExecuteScalarQuery(query,parameter);
            if (DevicesPresence > 0)
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
