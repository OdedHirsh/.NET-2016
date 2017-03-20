using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeWork_1.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        const string ConnString = (@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=API_Users_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut()]
        public string AddUser([FromBody]string UserName, [FromBody]string Password)
        {
            
            int rowsAffected = -1;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Users(UserName,Password) VALUES (@UserName,@Password)",conn);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@Password", Password);
                rowsAffected = cmd.ExecuteNonQuery();                
            }
            if (rowsAffected == 1)
            {
                return "User added successfuly";
            }
            return "";
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
