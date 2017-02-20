using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace WebTicTacToe
{
    public partial class api : System.Web.UI.Page
    {
        const string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TicTacToeDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        const long numberOfTenthOfSecondsInSingelDay = 24L * 60L * 60L * 10000000L;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            string action = Request["action"];
            if (action == null)
                return;
            string username;
            string password;
            bool success;

            username = Request["username"];
            password = Request["password"];
            if (username == null || password == null || username.Length < 2 || password.Length < 2)
                return;
            switch (action)
            {
                case "signup":
                    success = signup(username, password);
                    Response.Write(success ? "ok" : "failure");
                    break;
                case "login":
                    success = login(username, password);
                    Response.Write(success ? "ok" : "failure");

                    break;
                case "choosePartner":
                    choosePartner(username, password);
                    break;
                case "getPartners":
                    getPartners(username, password);
                    break;
                case "makeMove":
                    makeMove(username,password);
                    break;
                case "checkMove":
                    checkMove(username, password);
                    break;
                case "endGame":
                    endGame(username, password);
                    break;
                case "resetGame":
                    resetGame(username, password);
                    break;

            }

        }

        void getPartners(string username, string password)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                if (!validateUser(username, password, conn))
                    return;

                int now =(int)((DateTime.Now.Ticks / 1000000L) % numberOfTenthOfSecondsInSingelDay);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE Users SET GetPartnersTime=@now, Partner=NULL WHERE UserName=@UserName AND Partner IS NULL";
                cmd.Parameters.AddWithValue("@now", now);
                cmd.Parameters.AddWithValue("@UserName", username);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    stringBuilder.Append("chosen");
                    cmd.CommandText = "SELECT Partner FROM Users WHERE UserName=@userName";
                    cmd.Parameters.RemoveAt(0);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            stringBuilder.Append(dr.GetString(0));
                        }

                    }

                }
                else
                {
                    cmd.CommandText = "SELECT UserName FROM Users WHERE GetPartnersTime + 30 >@now AND UserName<>@UserName";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            stringBuilder.Append(dr.GetSqlString(0) + "&");
                        }
                    }
                    if (stringBuilder.Length > 0)
                        stringBuilder.Remove(stringBuilder.Length - 1, 1);
                }
                
            }
            Response.Write(stringBuilder.ToString());
        }

        void makeMove(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                if (!validateUser(username, password, conn))
                    return;

            }
        }
 
        void checkMove(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                if (!validateUser(username, password, conn))
                    return;

            }
        }

        void endGame(string username, string password)
        {
            int rowsAffected;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                if (!validateUser(username, password, conn))
                    return;
                SqlCommand cmd = new SqlCommand("SELECT Partner FROM Users WHERE UserName=@UserName AND Partner IS NOT NULL", conn);
                cmd.Parameters.AddWithValue("@UserName", username);
                string partner = null;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        partner = dr.GetString(0);
                    }
                }
                cmd.CommandText = "UPDATE Users SET Partner=NULL WHERE UserName=@UserName";
                rowsAffected = cmd.ExecuteNonQuery();
                cmd.Parameters["@UserName"].Value = partner;
                rowsAffected += cmd.ExecuteNonQuery();
            }
            Response.Write(rowsAffected == 2 ? "ok" : "failure");
        }

        void choosePartner(string username, string password)
        {
            string partner = Request["partner"];
            if (partner == null)
                return;
            bool success = false;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                if (!validateUser(username, password, conn))
                    return;
                
                SqlCommand cmd = new SqlCommand("UPDATE Users SET GetPartnersTime=NULL, Partner=@UserName WHERE UserName=@Partner AND GetPartnersTime IS NOT NULL",conn);
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@Partner", partner);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected ==1)
                {
                    cmd.CommandText = "UPDATE Users SET GetPartnersTime=NULL, Partner=@Partner WHERE UserName=@UserName AND GetPartnersTime IS NOT NULL";
                    rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 1)
                        success = true;
                }

            }
            Response.Write(success ? "ok" : "failure");
        }

        void resetGame(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                if (!validateUser(username, password, conn))
                    return;

            }
        }

        bool login(string username, string password)
        {
            bool valid = false;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                valid = validateUser(username, password,conn);
            }
            return valid;
        }

        bool validateUser(string username, string password, SqlConnection conn)
        {
            bool valid = false;
            SqlCommand cmd = new SqlCommand("SELECT Password FROM Users WHERE UserName=@UserName", conn);
            cmd.Parameters.AddWithValue("@UserName", username);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    if (dr.GetString(0) == password)
                        valid = true;
                }
            }
            return valid;
        }
        
        bool signup(string username, string password)
        {
            bool success = false;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SignupProcedure",conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@Password", password);
                SqlParameter countParameter = new SqlParameter("@count", SqlDbType.Int);
                countParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(countParameter);
                cmd.ExecuteNonQuery();
                int count = (int)countParameter.Value;
                if (count==0)
                {
                    success = true;
                }

            }
            return success;
        }
    }
}