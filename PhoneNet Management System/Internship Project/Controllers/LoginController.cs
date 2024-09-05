using Internship_Project.Models;
using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Internship_Project.Views;
using System.Reflection;

namespace Internship_Project.Controllers
{
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        [Route("authenticate")]
        [HttpPost]
        public IHttpActionResult Authenticate([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.username) || string.IsNullOrEmpty(user.password))
            {
                return BadRequest("Invalid login attempt.");
            }

            string hashedPassword = HashPassword(user.password);

            string query = "CheckPresence";
            SqlParameter usernameParameter = new SqlParameter("@username", user.username);
            SqlParameter passwordParameter = new SqlParameter("@hashedPassword", hashedPassword);
            int result = DatabaseHelper.ExecuteScalarQuery(query, usernameParameter, passwordParameter);

            if (result > 0)
            { 
                return Ok(new { success = true});
            }
            else
            {
                return Ok(new { success = false });
            }
        }

        [Route("register")]
        [HttpPost]
        public IHttpActionResult Register([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.username) || string.IsNullOrEmpty(user.password))
            {
                return BadRequest("Invalid registration attempt.");
            }

            string hashedPassword = HashPassword(user.password);

            string checkQuery = "CheckDuplicate";
            SqlParameter usernameParameterForChecking = new SqlParameter("@username", user.username);
            int usersPresence = DatabaseHelper.ExecuteScalarQuery(checkQuery, usernameParameterForChecking);

            if (usersPresence > 0)
            {
                return Conflict();
            }

            string insertQuery = "RegisterUser";
            SqlParameter passwordParameter = new SqlParameter("@hashedPassword", hashedPassword);
            SqlParameter usernameParameterForInsert = new SqlParameter("@username", user.username);
            int rowsAffected = DatabaseHelper.ExecuteNonQuery(insertQuery, usernameParameterForInsert, passwordParameter);

            if (rowsAffected > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Registration failed.");
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
