using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace WebApplication5
{
    public partial class api : System.Web.UI.Page
    {
        const string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=shook;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        protected void Page_Load(object sender, EventArgs e)
        {
            
            string action = Request["action"];
            if (action == null)
                return;
            switch (action)
            {
                case "getNadlan":
                    getNadlan();
                    break;
                case "getNadlanVirsion":
                    getNadlanVirsion();
                    break;
                case "publisheNadaln":
                    publishNadaln();
                    break;
                case "editNadlan":
                    editNadlan();
                    break;
                case "removeAd":
                    removeAd();
                    break;
                case "signup":
                    signup();
                    break;
                case "login":
                    login();
                    break;
            }
        }

        void getNadlan()
        {
            int cityId = 0;


            using (SqlConnection conn = new SqlConnection(connString))
            {

            }
        }
        void getNadlanVirsion()
        {

        }
        void publishNadaln()
        {
            string userName = Request["userName"];
            string password = Request["password"];
            if (userName == null || password == null)
                return;
            bool success = false;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                if (!validateUser(userName, password, conn))
                    return;
                
                int cityId = 0;
                int propertyTypeId = 0;
                int rooms = 0;
                int price = 0;
                int size = 0;
                int floorNumber = 0;

                try
                {
                    cityId = int.Parse(Request["cityId"]);
                    propertyTypeId = int.Parse(Request["propertyTypeId"]);
                    rooms = int.Parse(Request["rooms"]);
                    price = int.Parse(Request["price"]);
                    size = int.Parse(Request["size"]);
                    floorNumber = int.Parse(Request["floorNumber"]);
                }
                catch (Exception ex)
                {
                    return;
                }
                string propertyDescription = Request["propertyDescription"];
                if (propertyDescription == null || propertyDescription.Length>100)
                    return;

                //TODO:
                SqlCommand cmd = new SqlCommand("INSERT INTO Nadlan(CityId, ProportyTypeId, Rooms, Price, Size, FloorNumber, PropertyDescription, UserName) VALUES(@CityId, @ProportyTypeId, @Rooms, @Price, @Size, @FloorNumber, @PropertyDescription, @UserName)", conn);
                cmd.Parameters.AddWithValue("@CityId", cityId);
                cmd.Parameters.AddWithValue("@ProportyTypeId", propertyTypeId);
                cmd.Parameters.AddWithValue("@Rooms", rooms);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Size", size);
                cmd.Parameters.AddWithValue("@FloorNumber", floorNumber);
                cmd.Parameters.AddWithValue("@PropertyDescription", propertyDescription);
                cmd.Parameters.AddWithValue("@UserName", userName);
                success = cmd.ExecuteNonQuery() == 1;
            }
            Response.Write(success?"success":"failure");
        }


        void editNadlan()
        {

        }
        void removeAd()
        {

        }
        void signup()
        {
            string userName = Request["userName"];
            string password = Request["password"];
            string firstName = Request["firstName"];
            string lastName = Request["lastName"];
            string phoneNumber = Request["phoneNumber"];
            string email = Request["email"];
            if (userName == null || password == null || firstName == null || lastName == null || phoneNumber == null || email == null)
                return;
            bool success = false;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT UserName FROM Users WHERE userName=@userName",conn);
                cmd.Parameters.AddWithValue("@UserName", userName);
                bool userNameTaken = false;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    userNameTaken = dr.Read();
                }
                if (!userNameTaken)
                {
                    cmd.CommandText = "INSERT INTO Users (@UserName, @Password,@FirstName, @LastName,@PhoneNumber,@Email)";
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@firstName", firstName);
                    cmd.Parameters.AddWithValue("@lastName", lastName);
                    cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    cmd.Parameters.AddWithValue("@email", email);
                    success = cmd.ExecuteNonQuery() == 1;
                    
                }

            }
            Response.Write(success?"success":"failure");
        }
        void login()
        {
            string userName = Request["userName"];
            string password = Request["password"];
            if (userName == null || password == null)
                return;
            bool valid = false;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                valid = validateUser(userName,password,conn);
            }
            Response.Write(valid ? "success" : "failure");
        }

        bool validateUser(string userName, string password, SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("SELECT Password FROM Uers WHERE UserName=@UserName");
            cmd.Parameters.AddWithValue("@UserName",userName);
            bool valid = false;
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    valid = password == dr.GetString(0);
                }
            }
            return valid;
        }

    }
}