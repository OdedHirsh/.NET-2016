using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Customers_API
{
    public class Customer
    {
        public int customerID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public string Email { get; set; }
        public bool active { get; set; }
        public int Birthdate { get; set; }

        public Customer(int customerID, string firstName, string lastName, string phoneNumber, string Email, bool active, int Brithdate)
        {
            this.customerID = customerID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.phoneNumber = phoneNumber;
            this.Email = Email;
            this.active = active;
            this.Birthdate = Birthdate;
    }


    }
}
