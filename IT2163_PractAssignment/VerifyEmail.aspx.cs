using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IT2163_PractAssignment
{
    public partial class VerifyEmail : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASSIGNMENTDB"].ConnectionString;
        string userID = null;
        string token = null;

        // Open db & check whether the hash is valid ( not expired & matches )

        protected void verifyUserEmail(string userID)
            
        {
            if (Request.QueryString["token"] != null)
            {

                string userToken = HttpUtility.UrlDecode(Request.QueryString["token"]).Replace(" ", "+");
                string dbToken = null;
                DateTime dbTokenExpiry = new DateTime();

                // 

                SqlConnection connection = new SqlConnection(MYDBConnectionString);
                string sql = "SELECT * FROM [dbo].[User] WHERE Email=@userId";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@userId", userID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            if (reader["Token"] != DBNull.Value)
                            {
                                dbToken = reader["Token"].ToString();
                            }

                            if (reader["TokenExpiry"] != DBNull.Value)
                            {
                                dbTokenExpiry = (DateTime)reader["TokenExpiry"];
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }


                // This checks whether the time validity of token has been expired, as well as whether it matches.


                if (DateTime.Compare(DateTime.Now, dbTokenExpiry) <= 0 && userToken.Equals(dbToken))
                {

                    try
                    {
                        string sqlx = "UPDATE [dbo].[User] SET EmailVerified = 1, Token = @Token, TokenExpiry = @TokenExpiry WHERE [Email]=@USERID; ";
                        using (var cmd = new SqlCommand(sqlx, connection))
                        {
                            cmd.Parameters.AddWithValue("@USERID", (string)Session["userID"]);
                            cmd.Parameters.AddWithValue("@Token", DBNull.Value);
                            cmd.Parameters.AddWithValue("@TokenExpiry", DBNull.Value);
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



                    Response.Redirect("HomePage.aspx", false);



                }

                else
                {
                    Response.Redirect("VerificationFailed.aspx", false);

                }

            }
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

                    if (Session["UserID"] != null)
                    {
                        userID = (string)Session["userID"];
                        verifyUserEmail(userID);

                    }

                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }


        }


        // This function generates a new token with new expiry date, and stores them into the database before passing token to URL
        public string GenerateNewToken()
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            connection.Open();

            SHA512Managed hashing = new SHA512Managed();
            string token = userID + DateTime.Now.ToString();
            byte[] hashedToken = hashing.ComputeHash(Encoding.UTF8.GetBytes(token));
            token = Convert.ToBase64String(hashedToken);

            try
            {
                string sql = "UPDATE [dbo].[User] SET Token = @TOKEN, TokenExpiry = @TOKENEXPIRY WHERE [Email]=@USERID; ";
                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@USERID", (string)Session["userID"]);
                    cmd.Parameters.AddWithValue("@TOKEN", token);
                    cmd.Parameters.AddWithValue("@TOKENEXPIRY", DateTime.Now.AddMinutes(30));
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
            return token;
        }


        protected void verifyEmailBtn_Click(object sender, EventArgs e)
        {

            // Implement countdown
            // Generate new set of tokens with new expiry


            string to = "ktykuang@gmail.com"; //To address    
            string from = "contact.breadington.official@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = "<b>Thank you for your registration! <br> please verify your account here:</b> https://localhost:44399/VerifyEmail.aspx?token=" + HttpUtility.UrlEncode(GenerateNewToken());
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




        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {

            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login.aspx", false);

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Request.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Request.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);

            }


            if (Request.Cookies["AuthToken"] != null)
            {
                Request.Cookies["AuthToken"].Value = string.Empty;
                Request.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);

            }

            log4net.ThreadContext.Properties["ClientIp"] = GetIPAddress();
            log4net.ThreadContext.Properties["UserId"] = userID;
            log.Info("Account logged out");




            Response.Redirect("Login.aspx", false);
        }
    }
}