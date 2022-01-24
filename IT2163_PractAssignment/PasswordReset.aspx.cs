using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IT2163_PractAssignment
{
    public partial class PasswordReset : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASSIGNMENTDB"].ConnectionString;

        static string salt;
        static string finalHash;

        DateTime lastPwSet = new DateTime();

        string userID = null;


        string useridForHistory;

        static string currentHashedPass;


        static string oldPasswordHash;
        static string oldPasswordSalt;

        static string oldHashedPass1;
        static string oldHashedPass2;

        static string oldPassSalt1;
        static string oldPassSalt2;

        public bool verifyPassAge(string userId)
        {


            using (SqlConnection con = new SqlConnection(MYDBConnectionString))
            {

                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User] WHERE [Email] =@USERID", con))
                {
                    cmd.Parameters.AddWithValue("@USERID", userId);


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                if (reader["LastPwSet"] != DBNull.Value)
                                {
                                    lastPwSet = (DateTime)reader["LastPwSet"];

                                }

                            }
                        }
                    }

                }

                con.Close();

            }
            // Maximum password age; if it has exceeded 30 days, then allow reset of password

            if (DateTime.Now.Subtract(lastPwSet).TotalDays > 3)
            {
                return true;
            }
                return false;

        }



        protected void Page_Load(object sender, EventArgs e)
        {





            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {

                    if (Session["LoggedIn"] != null)
                    {
                        userID = (string)Session["LoggedIn"];


                        if (!verifyPassAge(userID))
                        {

                            Response.Redirect("HomePage.aspx", false);

                        }




                    }

                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }



        private int CheckPassword(string password)
        {
            int score = 0;

            //todo: implementation here


            //score 1

            if (password.Length < 8)
            {
                return 1;
            }

            else
            {
                score = 1;
            }

            //score 2

            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }

            //score 3


            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }

            //score 4


            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }


            //score 5


            if (Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                score++;
            }



            return score;

        }


        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        protected void submitBtn_Click(object sender, EventArgs e)
        {

            //changeUserPass

            // Password server side validation

            int scores = CheckPassword(passwordTB.Value);
            string status = "";
            switch (scores)
            {
                case 1:
                    status = "Very Weak";
                    break;
                case 2:
                    status = "Weak";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Excellent";
                    break;
                default:
                    break;
            }

            lbl_passwordError.Text = "Status : " + status;
            if (scores < 4)
            {
                lbl_passwordError.Attributes["style"] = "display: inline-block";
                lbl_passwordError.ForeColor = Color.Red;
                return;
            }
            lbl_passwordError.ForeColor = Color.Green;








            if (status == "Excellent")
            {

                SqlConnection connection = new SqlConnection(MYDBConnectionString);
                connection.Open();


                // Make a random salt
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];

                //Fills array of bytes with a cryptographically strong sequence of random values.
                rng.GetBytes(saltByte);
                salt = Convert.ToBase64String(saltByte);

                SHA512Managed hashing = new SHA512Managed();
                string passWithSalt = passwordTB.Value + salt;
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(passWithSalt));

                // To-be password converted into hash with the salt.

                finalHash = Convert.ToBase64String(hashWithSalt);



                // Obtain current password hash

                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {

                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User] WHERE [Email] =@USERID", con))
                    {
                        cmd.Parameters.AddWithValue("@USERID", userID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {

                                    if (reader["PasswordHash"] != DBNull.Value)
                                    {
                                        // Should be OldPasswordHash
                                        currentHashedPass = reader["PasswordHash"].ToString();

                                    }

                                    if (reader["Id"] != DBNull.Value)
                                    {
                                        useridForHistory = reader["Id"].ToString();
                                    }

                                    if (reader["PasswordSalt"] != DBNull.Value)
                                    {
                                        oldPasswordSalt = reader["PasswordSalt"].ToString();
                                    }

                                }
                            }
                        }

                    }


                }


                // obtain the old passwords & salts to do comparison


                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {

                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Password_History] WHERE [userID]=@USERID", con))
                    {
                        cmd.Parameters.AddWithValue("@USERID", useridForHistory);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {

                                    if (reader["FirstPasswordHash"] != DBNull.Value)
                                    {
                                        oldHashedPass1 = reader["FirstPasswordHash"].ToString();

                                    }

                                    if (reader["FirstPasswordSalt"] != DBNull.Value)
                                    {
                                        oldPassSalt1 = reader["FirstPasswordSalt"].ToString();

                                    }

                                    if (reader["SecondPasswordHash"] != DBNull.Value)
                                    {
                                        oldHashedPass2 = reader["SecondPasswordHash"].ToString();

                                    }

                                    if (reader["SecondPasswordSalt"] != DBNull.Value)
                                    {
                                        oldPassSalt2 = reader["SecondPasswordSalt"].ToString();

                                    }

                                }
                            }
                        }

                    }

                    con.Close();

                }


                // Comparison with current password

                string currentPwdWithSalt = passwordTB.Value + oldPasswordSalt;
                byte[] currentHashWithSalt = hashing.ComputeHash(System.Text.Encoding.UTF8.GetBytes(currentPwdWithSalt));
                string userHash1 = Convert.ToBase64String(currentHashWithSalt);


                // Comparsion with first old password


                string olderPwdWithSalt = passwordTB.Value + oldPassSalt1;
                byte[] olderHashWithSalt = hashing.ComputeHash(System.Text.Encoding.UTF8.GetBytes(olderPwdWithSalt));
                string userHash2 = Convert.ToBase64String(olderHashWithSalt);


                // Comparison with oldest password stored


                string oldestPwdWithSalt = passwordTB.Value + oldHashedPass2;
                byte[] oldestHashWithSalt = hashing.ComputeHash(System.Text.Encoding.UTF8.GetBytes(oldestPwdWithSalt));
                string userHash3 = Convert.ToBase64String(oldestHashWithSalt);

                // After comparison, if the to-be password is not equal to current password, nor the last 2 password, then proceed

                if ((!userHash1.Equals(currentHashedPass)) && (!userHash2.Equals(oldHashedPass1)) && (!userHash3.Equals(oldHashedPass2)))

                {

                    // To change the password.


                    // But first, store previous password into password history table


                    using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                    {

                        con.Open();

                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Password_History] WHERE [userID]=@USERID", con))
                        {
                            cmd.Parameters.AddWithValue("@USERID", useridForHistory);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader != null)
                                {
                                    while (reader.Read())
                                    {

                                        if (reader["FirstPasswordHash"] == DBNull.Value)
                                        {


                                            string sql3 = "UPDATE [dbo].[User_Password_History] SET [FirstPasswordHash] = @OLDPASSWORD, [FirstPasswordSalt] = @OLDSALT WHERE [userID]=@USERID; ";
                                            using (var cmdf = new SqlCommand(sql3, connection))
                                            {
                                                cmdf.Parameters.AddWithValue("@OLDPASSWORD", currentHashedPass);
                                                cmdf.Parameters.AddWithValue("@OLDSALT", oldPasswordSalt);
                                                cmdf.Parameters.AddWithValue("@USERID", useridForHistory);
                                                var update = cmdf.ExecuteNonQuery();
                                            }
                                        }

                                        else
                                        {

                                            string sql3 = "UPDATE [dbo].[User_Password_History] SET [SecondPasswordHash] = @FIRSTOLDPASSWORD, [SecondPasswordSalt] = @FIRSTOLDSALT WHERE [userID]=@USERID; ";
                                            using (var cmdxx = new SqlCommand(sql3, connection))
                                            {
                                                cmdxx.Parameters.AddWithValue("@FIRSTOLDPASSWORD", reader["FirstPasswordHash"].ToString());
                                                cmdxx.Parameters.AddWithValue("@FIRSTOLDSALT", reader["FirstPasswordSalt"].ToString());
                                                cmdxx.Parameters.AddWithValue("@USERID", useridForHistory);
                                                cmdxx.ExecuteNonQuery();
                                            }

                                            string sql4 = "UPDATE [dbo].[User_Password_History] SET [FirstPasswordHash] = @OLDPASSWORD, [FirstPasswordSalt] = @OLDSALT WHERE [userID]=@USERID; ";
                                            using (var cmd2 = new SqlCommand(sql4, connection))
                                            {
                                                cmd2.Parameters.AddWithValue("@OLDPASSWORD", currentHashedPass);
                                                cmd2.Parameters.AddWithValue("@OLDSALT", oldPasswordSalt);
                                                cmd2.Parameters.AddWithValue("@USERID", useridForHistory);
                                                cmd2.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }


                    // Finally, change password !!



                    try
                    {

                        string sql2 = "UPDATE [dbo].[User] SET [PasswordHash] = @PASSWORDHASH, [PasswordSalt] = @PASSWORDSALT WHERE [Email]=@USERID; ";
                        using (var cmdx = new SqlCommand(sql2, connection))
                        {

                            cmdx.Parameters.AddWithValue("@PASSWORDSALT", salt);
                            cmdx.Parameters.AddWithValue("@PASSWORDHASH", finalHash);
                            cmdx.Parameters.AddWithValue("@USERID", userID);
                            var update = cmdx.ExecuteNonQuery();
                        }
                    }

                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }

                    finally
                    {
                        // update pw last set timestamp

                        // No need void passwork token; for it is mandatory to change password this time.

                        string sql = "UPDATE [dbo].[User] SET [LastPwSet] = @LASTPWSET WHERE [Email]=@USERID; ";
                        using (var cmdz = new SqlCommand(sql, connection))
                        {
                            cmdz.Parameters.AddWithValue("@LASTPWSET", DateTime.Now);
                            cmdz.Parameters.AddWithValue("@USERID", userID);
                            var update = cmdz.ExecuteNonQuery();
                        }

                        connection.Close();


                        log4net.ThreadContext.Properties["ClientIp"] = GetIPAddress();
                        log4net.ThreadContext.Properties["UserId"] = userID;
                        log.Warn("Password changed due to policy");

                        Response.Redirect("/HomePage.aspx?passUpdated=true", false);
                    }
                }

                else
                {
                    lbl_passwordError.Text = "Your password cannot be the same as any of your recent passwords.";
                    lbl_passwordError.Attributes["style"] = "display: inline-block";
                    lbl_passwordError.ForeColor = Color.Red;
                }






            }

            else
            {
                // Change error message todo

                lbl_passwordError.Text = "Please change to a decent password.";
                lbl_passwordError.Attributes["style"] = "display: inline-block";
                lbl_passwordError.ForeColor = Color.Red;
            }

        }
        }
}