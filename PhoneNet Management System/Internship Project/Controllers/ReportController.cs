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
    [RoutePrefix("api/Report")]
    public class ReportController : ApiController
    {
        ObjectsMapper Om = new ObjectsMapper();


        [Route("getClientTypeStatistics")]
        [HttpGet]
        public IHttpActionResult getClienTypeStatistics()
        {
            string query = "getClientTypeStatistics";
            List<ClientTypeStatistic> statistics = DatabaseHelper.ExecuteQuery(query, Om.MapClientTypeStatistic);
            return Ok(statistics);
        }

        [Route("getFilteredClientTypeStatistics")]
        [HttpGet]
        public IHttpActionResult getFilteredClientTypeStatistics(int ClientType)
        {
            string query = "getFilteredClientTypeStatistics";
            List<ClientTypeStatistic> statistics;
            if (ClientType == -1)
            {
                statistics = DatabaseHelper.ExecuteQuery(query, Om.MapClientTypeStatistic);
            }
            else
            {
                SqlParameter clientTypeParameter = new SqlParameter("@ClientType", ClientType);
                statistics = DatabaseHelper.ExecuteQuery(query, Om.MapClientTypeStatistic,clientTypeParameter);

            }
            return Ok(statistics);
        }

        [Route("getDeviceStatistics")]
        [HttpGet]
        public IHttpActionResult getDeviceStatistics()
        {
            string query = "getDeviceStatistics";
            List<DeviceStatistic> statistics = DatabaseHelper.ExecuteQuery(query, Om.MapDeviceStatistic);
            return Ok(statistics);
        }

        [Route("getFilteredDeviceStatistics")]
        [HttpGet]
        public IHttpActionResult getFilteredDeviceStatistics(int status , int DeviceId)
        {
            string query = "getFilteredDeviceStatistics";
            List<DeviceStatistic> statistics;
            if (status == -1)
            {
                if (DeviceId == -1)
                {
                    statistics = DatabaseHelper.ExecuteQuery(query, Om.MapDeviceStatistic);
                }
                else
                {
                    SqlParameter DeviceIdParameter = new SqlParameter("@DeviceId",DeviceId);
                    statistics = DatabaseHelper.ExecuteQuery(query, Om.MapDeviceStatistic,DeviceIdParameter);
                }
            }
            else 
            {
                SqlParameter statusParameter = new SqlParameter("@Status", status);
                if (DeviceId == -1)
                {
                    statistics = DatabaseHelper.ExecuteQuery(query, Om.MapDeviceStatistic,statusParameter);
                }
                else
                {
                    SqlParameter DeviceIdParameter = new SqlParameter("@DeviceId", DeviceId);
                    statistics = DatabaseHelper.ExecuteQuery(query, Om.MapDeviceStatistic, DeviceIdParameter,statusParameter);
                }
            }
            return Ok(statistics);
        }
    }
}
