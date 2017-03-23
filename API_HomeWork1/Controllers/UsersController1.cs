using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_HomeWork1.Controllers
{
    [Route("api/[controller]")]
    public class UsersController1 : Controller
    {
        const string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=API_Users_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        List<User> Users = new List<User>();

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
        [HttpPut("{id}")]
        public string AddUser([FromBody]User user)
        {
                        
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT UserName FROM Users WHERE UserName=@UserName",conn);
                cmd.Parameters.AddWithValue("@UserName",user.userName);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return "User Taken";
                    }
                }
            }
            int rowsAffected = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Users(UserName,Password) VALUES (@UserName,@Password)", conn);
                cmd.Parameters.AddWithValue("@UserName", user.userName);
                cmd.Parameters.AddWithValue("@Password", user.password);
                rowsAffected = cmd.ExecuteNonQuery();
            }
            if (rowsAffected == 1)
            {
                return "User added successfuly";
            }
            return "Failure in adding User";
        }

        // DELETE api/values/5
        [HttpDelete()]
        public string Delete([FromBody]User user)
        {
            
            int rowsAffected = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT UserName FROM Users WHERE UserName=@UserName AND Password=@Password",conn);
                cmd.Parameters.AddWithValue("@UserName", user.userName);
                cmd.Parameters.AddWithValue("@Password", user.password);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = "DELETE FROM Users WHERE UserName=@UserName";
                        cmd.Parameters.AddWithValue("@UserName", user.userName);
                        rowsAffected = cmd.ExecuteNonQuery();
                        return "User Deleted";
                    }
                    return "Error - please check UserName or Password";
                }
            }
        }
    }
}
