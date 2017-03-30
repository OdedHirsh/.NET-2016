using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Customers_API.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        const string connstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CW_API;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public List<Customer> customersList = new List<Customer>();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<Customer> customers = new List<Customer>();
            Customer customer;

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Customers",conn);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        customers.Add(new Customer(
                            dr.GetInt32(0),
                            dr.GetString(1),
                            dr.GetString(2),
                            dr.GetString(3),
                            dr.GetString(4),
                            dr.GetBoolean(5),
                            dr.GetInt32(6)
                            ));
                    }
                }
            }



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

        [HttpPut]
        public bool AddCustomer([FromBody]Customer customer)
        {
            var customerID = customer.customerID;
            var fisrtName = customer.firstName;
            var lastName = customer.lastName;
            var phoneNUmber = customer.phoneNumber;
            var email = customer.Email;
            var active = customer.active;
            var birthdate = customer.Birthdate;
                        
            int rowsAffected = 0;

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Customers (FirstName,LastName,PhonNumber,Email,BirthDate) VALUES (@FirstName,@LastName,@PhonNumber,@Email,@BirthDate)", conn);
                cmd.Parameters.AddWithValue("@FirstName", fisrtName );
                cmd.Parameters.AddWithValue("@LastName", lastName );
                cmd.Parameters.AddWithValue("@PhonNumber",phoneNUmber);
                cmd.Parameters.AddWithValue("@Email", email );
                cmd.Parameters.AddWithValue("@BirthDate", birthdate);
                rowsAffected = cmd.ExecuteNonQuery();

            }
            return rowsAffected==1;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
