using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPX_Login_Signup
{
    public partial class index : System.Web.UI.Page
    {
        const string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ASPX-DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        void changeToHide()
        {
            txtPassword.Visible = false;
            txtUserName.Visible = false;
            btnLogin.Visible = false;
            btnSignup.Visible = false;
            usrLabel.Visible = false;
            pswLabel.Visible = false;
            btnLogout.Visible = true;
        }
        void changeToShow()
        {
            txtPassword.Visible = true;
            txtUserName.Visible = true;
            btnLogin.Visible = true;
            btnSignup.Visible = true;
            usrLabel.Visible = true;
            pswLabel.Visible = true;
            btnLogout.Visible = false;
        }

        bool Login (string userName, string password)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Password FROM Users WHERE UserName = @UserName", conn);
                cmd.Parameters.AddWithValue("@UserName", userName);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        if (dr.GetString(0)== password)
                        {
                            txtLabel.Text = "Hello " +userName+", you are now loged in";
                            txtLabel.ForeColor = System.Drawing.Color.Green;
                            changeToHide();
                            return true;
                        }
                        else
                        {
                            txtLabel.Text = "Password Wrong";
                            txtLabel.ForeColor = System.Drawing.Color.Red;
                            txtPassword.Text = "";
                            txtUserName.Text = userName;
                            return false;
                        }
                    }
                    txtLabel.Text = "User not exists - please try again";
                    txtLabel.ForeColor = System.Drawing.Color.Red;
                    txtPassword.Text = "";
                    txtUserName.Text = "";
                    return false;
                }
            }


        }

        protected void btnSignup_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string password = txtPassword.Text;
            if (userName.Length < 2 || password.Length < 2)
            {
                txtLabel.Text = "User Name or Password are too short";
                txtPassword.Text = "";
                txtUserName.Text = "";
                return;
            }
            if (Login(userName,password))
            {
                //txtLabel.Text = "Hello " + userName + ", you are now loged in";
                //txtLabel.ForeColor = System.Drawing.Color.Green;
                //changeToHide();

            }
            else if (txtUserName.Text==userName)
            {
                txtLabel.Text = "User Exists";
                txtLabel.ForeColor = System.Drawing.Color.Red;
                return;
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Users VALUES(@UserName, @Password)",conn);
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@Password", password);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected==1)
                    {
                        Login(userName, password);
                    }
                    else
                    {
                        txtLabel.Text = "Failed - Please try again";
                        txtLabel.ForeColor = System.Drawing.Color.Red;

                    }
                    
                }
            }
        }
        
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string password = txtPassword.Text;
            
            if (Login(userName,password))
            {
                txtLabel.Text = "Hello " + userName + ", you are now loged in";
                changeToHide();
            }
            else
            {
                txtLabel.Text = "User or Password Wrong";
                txtLabel.ForeColor = System.Drawing.Color.Red;
                txtPassword.Text = "";
                txtUserName.Text = "";
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            
            txtLabel.Text = "Welcome";
            txtLabel.ForeColor = System.Drawing.Color.Black;
            txtPassword.Text = "";
            txtUserName.Text = "";
            changeToShow();
        }
    }
}