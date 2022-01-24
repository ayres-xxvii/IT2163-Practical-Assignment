using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.IO;
using System.Security.Cryptography;
using System.Data.SqlClient;
using Ninject;
using log4net;
using System.Reflection;
using System.Data;
using System.Configuration;
using System.Net.Mail;
using System.Text;

namespace IT2163_PractAssignment
{

    public class MyObject
    {
        public string success { get; set; }

        public List<String> ErrorMessage { get; set; }
    }


    public partial class Login : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASSIGNMENTDB"].ConnectionString;


        public class MyObject
        {
            public string success { get; set; }

            public List<string> ErrorMessage { get; set; }

        }




        protected void Page_Load(object sender, EventArgs e)
        {

            //var Kernel = new StandardKernel();
            //Kernel.Bind<ILog>().ToMethod(c => LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)).InSingletonScope();
            //var log = Kernel.Get<ILog>();





        }





        // Get user IP

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



        // Server side captcha validation
        public bool ValidateCaptcha()
        {
            bool result = true;

            // When user submits the recaptcha form, the user gets a POST parameter.
            // captchaResponse consist of the user click pattern. Behaviour analytics! AI :)
            string captchaResponse = Request.Form["g-recaptcha-response"];


            // To send a GET request to Google along with the response and secret key.

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
                (" https://www.google.com/recaptcha/api/siteverify?secret=6LfZBjkdAAAAAG0qvB8aXCzaywdsI-j9ejaiBa_l &response=" + captchaResponse
                );

            try
            {
                //Code to receive the Response in JSON format from google server

                using (WebResponse wResponse = req.GetResponse())
                {

                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        //The response in JSON format
                        string jsonResponse = readStream.ReadToEnd();

                        // To show the JSON response string for learning purpsoe

                        //lblMessage.Text = jsonResponse.ToString();


                        JavaScriptSerializer js = new JavaScriptSerializer();

                        // Create jsonObject to handle the response either success or error
                        // Deserialize json

                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        //Convert the string "False" to bool false or "true" to bool true

                        result = Convert.ToBoolean(jsonObject.success);


                    }

                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }


        //Get DB Hash

        protected string getDBHash(string userid)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM [dbo].[User] WHERE [Email]=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }



        //Get DB salt


        protected string getDBSalt(string userid)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordSalt FROM [dbo].[User] WHERE [Email]=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PasswordSalt"] != null)
                        {
                            if (reader["PasswordSalt"] != DBNull.Value)
                            {
                                s = reader["PasswordSalt"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }


        protected string GeneratePwToken(string userID)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            connection.Open();
            SHA512Managed hashing = new SHA512Managed();
            string pwToken = userID + DateTime.Now.ToString();
            byte[] hashedToken = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwToken));
            var hashedpwToken = Convert.ToBase64String(hashedToken);

            try
            {
                string sql = "UPDATE [dbo].[User] SET ResetPwToken = @PWTOKEN, ResetPwTokenExpiry = @PWTOKENEXPIRY WHERE [Email]=@USERID; ";
                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@USERID", userID);
                    cmd.Parameters.AddWithValue("@PWTOKEN", hashedpwToken);
                    cmd.Parameters.AddWithValue("@PWTOKENEXPIRY", DateTime.Now.AddMinutes(30));
                    var update = cmd.ExecuteNonQuery();
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

            return hashedpwToken;
        }



        protected bool verifyLocked(string userid)
        {
            int loginAttempts = 0;
            DateTime lastFailed = new DateTime();

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * FROM [dbo].[User] WHERE [Email]=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["FailedAttempts"] != null)
                        {
                            if (reader["FailedAttempts"] != DBNull.Value)
                            {
                                loginAttempts = (int)reader["FailedAttempts"];
                                //lastFailed = (DateTime)reader["LastFailed"];
                            }

                        }

                        if (reader["LastFailed"] != null)
                        {
                            if (reader["LastFailed"] != DBNull.Value)
                            {
                                lastFailed = (DateTime)reader["LastFailed"];
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }

            if ((loginAttempts < 3) || (DateTime.Now.Subtract(lastFailed).TotalSeconds > 30))
            {
                //lastFailed = DateTime.Now;

                //string sqlx = "UPDATE [dbo].[User] SET LastFailed = @LASTFAILED WHERE [Email]=@USERID";

                //using (var cmd = new SqlCommand(sqlx, connection))
                //{
                //    cmd.Parameters.AddWithValue("@USERID", userid);
                //    cmd.Parameters.AddWithValue("@LASTFAILED", DateTime.Now);
                //    var update = cmd.ExecuteNonQuery();
                //}

                //connection.Close();

                //// Account is locked temporarily
                ///

                return false;
            }
            else
            {
                // Locked!







                return true;

            }







        }


        protected void increaseFailedAttempt(string userEmail)
        {

            int loginAttempts = 0;


            log4net.ThreadContext.Properties["ClientIp"] = GetIPAddress();
            log4net.ThreadContext.Properties["UserId"] = userEmail;
            log.Warn("Login attempt failed from " + GetIPAddress());




            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            connection.Open();
            try
            {
                string sql = "UPDATE [dbo].[User] SET FailedAttempts += 1,  LastFailed = @LASTFAILED WHERE [Email]=@USERID; ";
                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@USERID", userEmail);
                    cmd.Parameters.AddWithValue("@LASTFAILED", DateTime.Now);
                    var update = cmd.ExecuteNonQuery();
                }






            }

            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
            }



            // if failed attempts > 3 send email

            string sql2 = "select * FROM [dbo].[User] WHERE [Email]=@USERID";
            SqlCommand command2 = new SqlCommand(sql2, connection);
            command2.Parameters.AddWithValue("@USERID", userEmail);


            using (SqlDataReader reader = command2.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader["FailedAttempts"] != null)
                    {
                        if (reader["FailedAttempts"] != DBNull.Value)
                        {
                            loginAttempts = (int)reader["FailedAttempts"];
                            //lastFailed = (DateTime)reader["LastFailed"];
                        }

                    }

                }
            }



            if (loginAttempts >= 3)
                {
                string to = "ktykuang@gmail.com"; //To address    
                string from = "contact.breadington.official@gmail.com"; //From address    
                MailMessage message = new MailMessage(from, to);
                string mailbody = "<h1>Forgot your password?</h1><br>Sorry to hear you're having trouble logging into your account. We can help you get straight back into your account. (If you are not trying to recover your login credentials, you can ignore this email.)<br>reset your password by clicking on the link below <br>" + "https://localhost:44399/EndPassReset.aspx?token=" + HttpUtility.UrlEncode(GeneratePwToken(tb_userEmail.Text.Trim()));
                message.Subject = "SITConnect Account Recovery ✔ ";
                message.Body = mailbody;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    




                string strEmail = System.Configuration.ConfigurationManager.AppSettings["Email"];
                string strPassword = System.Configuration.ConfigurationManager.AppSettings["EmailPassword"];


                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential(strEmail, strPassword);


                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;
                try
                {
                    client.Send(message);
                }

                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();

                }
            }



        
        }


        protected void resetLockout(string userEmail)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            connection.Open();
            try
            {
                string sql = "UPDATE [dbo].[User] SET FailedAttempts = 0,  LastFailed = @NULL_VAL WHERE [Email]=@USERID; ";
                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@USERID", userEmail);
                    cmd.Parameters.AddWithValue("@NULL_VAL", DBNull.Value);
                    var update = cmd.ExecuteNonQuery();
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

        }


        protected void LoginMe(object sender, EventArgs e)
        {

            // Verify whether account is locked out

            // Sanitize user input



            // Remove this for earlier practical
            if (ValidateCaptcha())
            {

                // Check for username & password (hardcoded for this demo)

                //if (tb_userEmail.Text.Trim().Equals("ayres") && tb_password.Text.Trim().Equals("ayres"))
                //{

                // Session for User

                Session["LoggedIn"] = tb_userEmail.Text.Trim();

                // Create a new GUID and save it into second session

                string guid = Guid.NewGuid().ToString();

                Session["AuthToken"] = guid;

                // Now create a new cookie with *the* guid value

                Response.Cookies.Add(new HttpCookie("AuthToken", guid));


                // Password decryption


                string password = tb_password.Text.ToString().Trim();
                string userEmail = tb_userEmail.Text.ToString().Trim();
                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(userEmail);
                string dbSalt = getDBSalt(userEmail);


                try
                {
                    //if (!verifyLocked(userEmail))
                    //{

                    //if (verifyLocked(userEmail) == false)
                    //{ 

                    // Obtain encrypted credentials from database & compare.

                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {



                        string pwdWithSalt = password + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);


                        if (userHash.Equals(dbHash))
                        {

                            if (!verifyLocked(userEmail))
                            {




                                // Resets failedAttempt to 0 & lastFailed to null.

                                resetLockout(userEmail);
                                Session["UserID"] = userEmail;

                                // Mandatory 2FA?  yes                            
                                Response.Redirect("TwoFactorAuth.aspx", false);

                            }

                            else
                            {
                                lblMessage.Style.Add("display", "inline-block");
                                lblMessage.Text = "Your Account has been temporarily locked!";

                                log4net.ThreadContext.Properties["ClientIp"] = GetIPAddress();
                                log4net.ThreadContext.Properties["UserId"] = tb_userEmail.Text;
                                log.Warn("Account has been locked!");


                            }


                        }
                        else
                        {
                            // increment failedAttempts & set time in lastFailed
                            increaseFailedAttempt(userEmail);
                            lblMessage.Style.Add("display", "inline-block");
                            lblMessage.Text = "Invalid email or password!";
                        }

                    }

                    else
                    {
                        // Email is wrong.
                        lblMessage.Style.Add("display", "inline-block");
                        lblMessage.Text = "Invalid email or password!";

                    }
                }




                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally { }


            }

            else
            {
                lblMessage.Style.Add("display", "inline-block");
                lblMessage.Text = "Invalid captcha attempt!";
            }
        }
    }
}