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
    public partial class EndPassReset : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASSIGNMENTDB"].ConnectionString;
        static string salt;
        static string finalHash;
        static string userToken;

        static string currentHashedPass;


        static string oldPasswordHash;
        static string oldPasswordSalt;

        static string oldHashedPass1;
        static string oldHashedPass2;

        static string oldPassSalt1;
        static string oldPassSalt2;

        string userID;

        string userEmail;

        DateTime lastPwSet = new DateTime();



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


        protected void verifyPwTokenOrSession()
        {

            if (Request.QueryString["token"] != null)
            {

                userToken = HttpUtility.UrlDecode(Request.QueryString["token"]).Replace(" ", "+");
                string dbPwToken = null;
                DateTime dbPwTokenExpiry = new DateTime();

                // 

                SqlConnection connection = new SqlConnection(MYDBConnectionString);
                string sql = "SELECT * FROM [dbo].[User] WHERE ResetPwToken=@RESETPWTOKEN";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@RESETPWTOKEN", userToken);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            if (reader["ResetPwToken"] != DBNull.Value)
                            {
                                dbPwToken = reader["ResetPwToken"].ToString();
                            }

                            if (reader["ResetPwTokenExpiry"] != DBNull.Value)
                            {
                                dbPwTokenExpiry = (DateTime)reader["ResetPwTokenExpiry"];
                            }

                            if (reader["Id"] != DBNull.Value)
                            {
                                userID = reader["Id"].ToString();
                            }

                            if (reader["Email"] != DBNull.Value)
                            {
                                userEmail = reader["Email"].ToString();
                            }
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                oldPasswordHash = reader["PasswordHash"].ToString();
                            }
                            if (reader["PasswordSalt"] != DBNull.Value)
                            {
                                oldPasswordSalt = reader["PasswordSalt"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }

                finally
                {
                    connection.Close();
                }


                // This checks whether the time validity of token has been expired, as well as whether it matches.
                // If doesn't match, redirect back to login page.

                // Todo check?

                if (DateTime.Compare(DateTime.Now, dbPwTokenExpiry) > 0 || !(userToken.Equals(dbPwToken)))

                    Response.Redirect("VerificationFailed.aspx");
            }


                  



        }









        public bool firstTimeChangingPass(string userID)
        {

            using (SqlConnection con = new SqlConnection(MYDBConnectionString))
            {

                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Password_History] WHERE [userID]=@USERID", con))
                {
                    cmd.Parameters.AddWithValue("@USERID", userID);


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                if (reader["FirstPasswordHash"] == DBNull.Value)
                                {
                                    return true;

                                }

                            }
                        }
                    }

                }
                con.Close();
            }

            return false;

        }




        protected void Page_Load(object sender, EventArgs e)
        {
            // Retrieve the token, search through database, if match, change that person's credentials
            verifyPwTokenOrSession();
        }



        protected void submitBtn_Click(object sender, EventArgs e)
        {
            //Ensure password is valid according to regex.


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










                //Todo: Search through database
                //Todo: Change user credentials
                //Todo: Redirect back to login page.

                SqlConnection connection = new SqlConnection(MYDBConnectionString);
                connection.Open();
                try
                {

                    // Make a random salt
                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                    byte[] saltByte = new byte[8];

                    //Fills array of bytes with a cryptographically strong sequence of random values.
                    rng.GetBytes(saltByte);
                    salt = Convert.ToBase64String(saltByte);

                    SHA512Managed hashing = new SHA512Managed();
                    string passWithSalt = passwordTB.Value + salt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(passWithSalt));
                    finalHash = Convert.ToBase64String(hashWithSalt);










                    using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                    {

                        con.Open();

                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User] WHERE ResetPwToken=@RESETPWTOKEN", con))
                        {
                            cmd.Parameters.AddWithValue("@RESETPWTOKEN", userToken);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader != null)
                                {
                                    while (reader.Read())
                                    {

                                        if (reader["PasswordHash"] != DBNull.Value)
                                        {
                                            currentHashedPass = reader["PasswordHash"].ToString();

                                        }

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




                    // Minimum password age


                    if (firstTimeChangingPass(userID) || DateTime.Now.Subtract(lastPwSet).TotalDays > 1)
                    {




                        using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                        {

                            con.Open();

                            using (SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Password_History] WHERE [userID]=@USERID", con))
                            {
                                cmd.Parameters.AddWithValue("@USERID", userID);

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


                        // Store previous password into password history table

                        if ((!userHash1.Equals(currentHashedPass)) && (!userHash2.Equals(oldHashedPass1)) && (!userHash3.Equals(oldHashedPass2)))
                        {

                            using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                            {

                                con.Open();

                                using (SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Password_History] WHERE [userID]=@USERID", con))
                                {
                                    cmd.Parameters.AddWithValue("@USERID", userID);

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
                                                        cmdf.Parameters.AddWithValue("@OLDPASSWORD", oldPasswordHash);
                                                        cmdf.Parameters.AddWithValue("@OLDSALT", oldPasswordSalt);
                                                        cmdf.Parameters.AddWithValue("@USERID", userID);
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
                                                        cmdxx.Parameters.AddWithValue("@USERID", userID);
                                                        cmdxx.ExecuteNonQuery();
                                                    }

                                                    string sql4 = "UPDATE [dbo].[User_Password_History] SET [FirstPasswordHash] = @OLDPASSWORD, [FirstPasswordSalt] = @OLDSALT WHERE [userID]=@USERID; ";
                                                    using (var cmd2 = new SqlCommand(sql4, connection))
                                                    {
                                                        cmd2.Parameters.AddWithValue("@OLDPASSWORD", oldPasswordHash);
                                                        cmd2.Parameters.AddWithValue("@OLDSALT", oldPasswordSalt);
                                                        cmd2.Parameters.AddWithValue("@USERID", userID);
                                                        cmd2.ExecuteNonQuery();
                                                    }


                                                }



                                            }
                                        }
                                    } // reader closed and disposed up here

                                } // command disposed here

                            }

                            // Changing user password

                            try
                            {

                                string sql2 = "UPDATE [dbo].[User] SET [PasswordHash] = @PASSWORDHASH, [PasswordSalt] = @PASSWORDSALT ,[ResetPwTokenExpiry] = @RESETPWTOKENEXPIRY WHERE [ResetPwToken]=@RESETPWTOKEN; ";
                                using (var cmdx = new SqlCommand(sql2, connection))
                                {
                                    var meow = HttpUtility.UrlDecode(Request.QueryString["token"]);
                                    var meow2 = salt;
                                    var meow3 = finalHash;

                                    cmdx.Parameters.AddWithValue("@RESETPWTOKEN", HttpUtility.UrlDecode(Request.QueryString["token"]).Replace(" ", "+"));
                                    cmdx.Parameters.AddWithValue("@PASSWORDSALT", salt);
                                    cmdx.Parameters.AddWithValue("@PASSWORDHASH", finalHash);
                                    cmdx.Parameters.AddWithValue("@RESETPWTOKENEXPIRY", DBNull.Value);

                                    var update = cmdx.ExecuteNonQuery();
                                }
                            }

                            catch (Exception ex)
                            {
                                throw new Exception(ex.ToString());
                            }

                            finally
                            {
                                // Void the last token & update pw last set timestamp

                                string sql = "UPDATE [dbo].[User] SET [ResetPwToken] = @PWTOKEN, [LastPwSet] = @LASTPWSET, [FailedAttempts] = 0, [LastFailed] = @LASTFAILED WHERE [ResetPwToken]=@RESETPWTOKEN; ";
                                using (var cmdz = new SqlCommand(sql, connection))
                                {
                                    cmdz.Parameters.AddWithValue("@RESETPWTOKEN", HttpUtility.UrlDecode(Request.QueryString["token"]).Replace(" ", "+"));
                                    cmdz.Parameters.AddWithValue("@LASTPWSET", DateTime.Now);
                                    cmdz.Parameters.AddWithValue("@PWTOKEN", DBNull.Value);
                                    cmdz.Parameters.AddWithValue("@LASTFAILED", DBNull.Value);
                                    var update = cmdz.ExecuteNonQuery();
                                }

                                connection.Close();


                                log4net.ThreadContext.Properties["ClientIp"] = GetIPAddress();
                                log4net.ThreadContext.Properties["UserId"] = userEmail;
                                log.Warn("Password manually changed");


                                Response.Redirect("/Login.aspx?passUpdated=true", false);


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
                        lbl_passwordError.Text = "You must wait longer to change your password";
                        lbl_passwordError.Attributes["style"] = "display: inline-block";
                        lbl_passwordError.ForeColor = Color.Red;
                    }

                }

                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
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