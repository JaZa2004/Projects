using Internship_Project.Models;
using Internship_Project.Views;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Web.Http;

namespace Internship_Project.Controllers
{
    [RoutePrefix("api/clients")]
    public class ClientsController : ApiController
    {
        ObjectsMapper Om = new ObjectsMapper();

        [Route("getAllClients")]
        [HttpGet]
        public IHttpActionResult GetAllClients()
        {
            string query = "GetAllClients";
            var clients = DatabaseHelper.ExecuteQuery(query, Om.MapClient);
            return Ok(clients);
        }

        [Route("getFilteredClients")]
        [HttpGet]
        public IHttpActionResult GetFilteredClients(string searchtext)
        {
                string query = "GetFilteredClients";
                SqlParameter searchParam = new SqlParameter("@SearchText", searchtext);
                var clients = DatabaseHelper.ExecuteQuery(query, Om.MapClient, searchParam);
                return Ok(clients);
        }


        [Route("getFilteredTypeClients")]
        [HttpGet]
        public IHttpActionResult GetFilteredTypeClients(int typeInt)
        {
            if (typeInt ==0 || typeInt==1)
            {
                string query = "GetFilteredTypeClients";
                SqlParameter searchParam = new SqlParameter("@typeInt", typeInt);
                var clients = DatabaseHelper.ExecuteQuery(query, Om.MapClient, searchParam);
                return Ok(clients);

            }
            else
            {
                string query = "GetAllClients";
                var clients = DatabaseHelper.ExecuteQuery(query, Om.MapClient);
                return Ok(clients);

            }
        }

        [HttpPost]
        [Route("addClient")]
        public IHttpActionResult AddClient([FromBody] Client client)
        {
            if (client == null || string.IsNullOrEmpty(client.Name))
            {
                return BadRequest("Client must have a name.");
            }

            System.Diagnostics.Debug.WriteLine($"Local BirthDate: {client.BirthDate}");

            string query = "AddClient";
            SqlParameter nameParam = new SqlParameter("@Name", client.Name);
            SqlParameter typeParam = new SqlParameter("@Type", (int)client.Type);

            DateTime? ModifedBirthDate = client.BirthDate.HasValue ?
                                      (DateTime?)client.BirthDate.Value.ToUniversalTime() :
                                      null;

            System.Diagnostics.Debug.WriteLine($"UTC BirthDate: {ModifedBirthDate}");

            SqlParameter birthDateParam = new SqlParameter("@BirthDate", ModifedBirthDate.HasValue ? (object)ModifedBirthDate.Value : DBNull.Value);

            DatabaseHelper.ExecuteNonQuery(query, nameParam, typeParam, birthDateParam);
            string query2 = "GetAllClients";
            var clients = DatabaseHelper.ExecuteQuery(query2, Om.MapClient);
            return Ok(clients);
        }

        [HttpPut]
        [Route("updateClient")]
        public IHttpActionResult UpdateClient([FromBody] Client client)
        {
            if (client == null || string.IsNullOrEmpty(client.Name))
            {
                return BadRequest("Client must have a name.");
            }

            System.Diagnostics.Debug.WriteLine($"Local BirthDate: {client.BirthDate}");

            string query = "UpdateClient";
            SqlParameter idParam = new SqlParameter("@Id", client.Id);
            SqlParameter nameParam = new SqlParameter("@Name", client.Name);
            SqlParameter typeParam = new SqlParameter("@Type", (int)client.Type);

            DateTime? ModifiedBirthDate = client.BirthDate.HasValue && (int)client.Type ==0 ?
                                      (DateTime?)client.BirthDate.Value.ToUniversalTime() :
                                      null;

            System.Diagnostics.Debug.WriteLine($"UTC BirthDate: {ModifiedBirthDate}");

            SqlParameter birthDateParam = new SqlParameter("@BirthDate", ModifiedBirthDate.HasValue ? (object)ModifiedBirthDate.Value : DBNull.Value);

            DatabaseHelper.ExecuteNonQuery(query, idParam, nameParam, typeParam, birthDateParam);

            string query2 = "GetAllClients";
            var clients = DatabaseHelper.ExecuteQuery(query2, Om.MapClient);
            return Ok(clients);
        }

        [HttpGet]
        [Route("CheckDuplicateClient")]
        public IHttpActionResult CheckDuplicateClient(string ClientName)
        {
            string query = "CheckClientDuplicate";
            SqlParameter parameter = new SqlParameter("@ClientName",ClientName);
            int ClientPresence = DatabaseHelper.ExecuteScalarQuery(query, parameter);
            if (ClientPresence > 0)
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
