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
        const long numberOfTenthOfSecondsInSingleDay = 24L * 60L * 60L * 10000000L;
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
            if (username == null || password == null
                || username.Length < 2 || password.Length < 2)
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
                    makeMove(username, password);
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
                //TODO
                int now = (int)((DateTime.Now.Ticks / 1000000L) % numberOfTenthOfSecondsInSingleDay);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE Users SET GetPartnersTime=@now WHERE UserName=@UserName AND Partner IS NULL";
                cmd.Parameters.AddWithValue("@now", now);
                cmd.Parameters.AddWithValue("@UserName", username);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    stringBuilder.Append("chosen");
                    cmd.CommandText = "SELECT Partner FROM Users WHERE UserName=@UserName";
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
                    //cmd.Parameters.Clear();
                    //cmd.Parameters.Remove("@UserName");
                    cmd.CommandText = "SELECT UserName FROM Users WHERE GetPartnersTime + 20 > @now AND UserName <> @UserName";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            stringBuilder.Append(dr.GetString(0) + "&");
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
            string cellString = Request["cell"];
            if (cellString == null)
                return;
            int cell = -1;
            try
            {
                cell = int.Parse(cellString);
            }
            catch
            {
                return;
            }
            if (cell < 0 || cell > 8)
                return;
            bool ok = false;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                if (!validateUser(username, password, conn))
                    return;
                SqlCommand cmd = new SqlCommand("SELECT BoardId FROM Users WHERE UserName=@UserName AND BoardId IS NOT NULL", conn);
                cmd.Parameters.AddWithValue("@UserName", username);
                int boardId = -1;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        boardId = dr.GetInt32(0);
                    }
                }
                if (boardId == -1)
                    return;
                string cellsString = "";
                for (int i = 1; i <= 9; i++)
                {
                    cellsString += "Cell" + i + ",";
                }
                cmd.CommandText = "SELECT PlayerX, PlayerO, MoveCount," + cellsString + " IsXturn FROM Boards WHERE Id=@BoardId";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@BoardId", boardId);

                string playerX;
                string playerO;
                int moveCount;
                bool isXturn = true;
                bool userIsX;
                int[] cells;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        playerX = dr.GetString(0);
                        playerO = dr.GetString(1);
                        moveCount = dr.GetInt32(2);
                        isXturn = dr.GetBoolean(12);
                        userIsX = username == playerX;
                        cells = new int[9];
                        for (int i = 0; i < 9; i++)
                        {
                            cells[i] = dr.GetByte(3 + i);
                        }
                        if (userIsX == isXturn)
                        {
                            if (cells[cell] == 0)
                            {
                                ok = true;
                            }
                        }
                    }
                }
                if (ok)
                {
                    cmd.CommandText = "UPDATE Boards SET Cell" + (cell + 1) + "=" + (isXturn ? "1" : "2") + ",MoveCount=MoveCount+1,IsXturn=" + (isXturn ? "0" : "1") + " WHERE Id=@BoardId";
                    cmd.ExecuteNonQuery();
                }
            }
            Response.Write(ok ? "ok" : "failure");
        }

        void checkMove(string username, string password)
        {
            string s = "";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                if (!validateUser(username, password, conn))
                    return;
                SqlCommand cmd = new SqlCommand("SELECT Cell1,Cell2,Cell3,Cell4,Cell5,Cell6,Cell7,Cell8,Cell9,IsXturn,MoveCount FROM Boards INNER JOIN Users ON Boards.Id=Users.BoardId WHERE Users.UserName=@UserName", conn);
                cmd.Parameters.AddWithValue("@UserName", username);
                int[] cells = new int[9];
                int moveCount = -1;
                bool isXturn = false;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            cells[i] = dr.GetByte(i);
                        }
                        isXturn = dr.GetBoolean(9);
                        moveCount = dr.GetInt32(10);
                        for (int i = 0; i < 9; i++)
                        {
                            s += cells[i] + "&";
                        }
                        s += isXturn + "&" + moveCount;

                    }
                    else
                    {
                        s = "endgame";
                    }
                }


            }
            Response.Write(s);
        }

        void endGame(string username, string password)
        {
            int rowsAffected = 0;
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
                if (partner != null)
                {
                    cmd.CommandText = "UPDATE Users SET Partner=NULL, BoardId=NULL WHERE UserName=@UserName";
                    rowsAffected = cmd.ExecuteNonQuery();
                    cmd.Parameters["@UserName"].Value = partner;
                    rowsAffected += cmd.ExecuteNonQuery();
                }
                //SqlCommand cmd = new SqlCommand("UPDATE Users SET Partner=NULL WHERE UserName=@UserName", conn);
            }
            Response.Write(rowsAffected == 2 ? "ok" : "failure");
        }

        void resetGame(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                if (!validateUser(username, password, conn))
                    return;
                //TODO
            }
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
                SqlCommand cmd = new SqlCommand("UPDATE Users SET GetPartnersTime=NULL,Partner=@UserName WHERE UserName=@Partner AND GetPartnersTime IS NOT NULL", conn);
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@Partner", partner);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 1)
                {
                    cmd.CommandText = "UPDATE Users SET GetPartnersTime=NULL,Partner=@Partner WHERE UserName=@UserName AND GetPartnersTime IS NOT NULL";
                    rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 1)
                    {

                        cmd.CommandText = "INSERT INTO Boards(PlayerX, PlayerO) OUTPUT INSERTED.ID VALUES (@UserName, @Partner)";
                        int boardId = (int)cmd.ExecuteScalar();
                        cmd.CommandText = "UPDATE Users SET BoardId=@BoardId WHERE UserName IN (@UserName,@Partner)";
                        cmd.Parameters.AddWithValue("@BoardId", boardId);
                        rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 2)
                            success = true;
                    }
                    else
                    {

                    }
                }
            }
            Response.Write(success ? "ok" : "failure");
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


        bool login(string username, string password)
        {
            bool valid = false;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                valid = validateUser(username, password, conn);

            }
            return valid;
        }


        bool signup(string username, string password)
        {
            bool success = false;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SignupProcedure", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                SqlParameter countParameter =
                    new SqlParameter("@count", SqlDbType.Int);
                countParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(countParameter);
                cmd.ExecuteNonQuery();
                //int count = int.Parse(countParameter.Value.ToString());
                int count = (int)countParameter.Value;
                if (count == 0)
                    success = true;

                //SqlCommand cmd = new SqlCommand("INSERT INTO Users VALUES(@UserName, @Password, 1)", conn);
                ////cmd.Parameters.Add("@UserName", SqlDbType.VarChar);
                ////cmd.Parameters["@UserName"].Value = username;

                //rowsAffected = cmd.ExecuteNonQuery();
            }
            return success;
        }
    }
}