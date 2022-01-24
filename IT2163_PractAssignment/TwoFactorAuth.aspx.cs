using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Google.Authenticator;

namespace IT2163_PractAssignment
{
    public partial class TwoFactorAuth : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASSIGNMENTDB"].ConnectionString;
        string userID = null;
        // secret ingredient is hash of passwordSalt + userID
        string secretIngredient = null;


        protected string retrieveSecretStuff(string userid)
        {
            // Retrieve password salt, append it to userID & hash it. 
            // Then return

            // yeah science, mr white!

            string h = null;
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
                                h = reader["PasswordSalt"].ToString();
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


            SHA512Managed hashing = new SHA512Managed();
            string idWithSalt = userID + h;
            byte[] hashIDWithSalt = hashing.ComputeHash(System.Text.Encoding.UTF8.GetBytes(idWithSalt));
            string secretIngredient = Convert.ToBase64String(hashIDWithSalt);

            return secretIngredient;


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




        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["Authenticated"] == null)
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

                            {
                                secretIngredient = retrieveSecretStuff(userID);

                                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                                var setupInfo = tfa.GenerateSetupCode("IT2163 AppSecProj", userID, secretIngredient, 300, 300);
                                var setupInfo2 = tfa.GetCurrentPIN(secretIngredient);
                                string qrCodeImageUrl = setupInfo.QrCodeSetupImageUrl;
                                string manualEntrySetupCode = setupInfo.ManualEntryKey;
                                bool isCorrectPIN = tfa.ValidateTwoFactorPIN(secretIngredient, setupInfo2);
                                lblCheck.Text = isCorrectPIN.ToString();
                                imgQRcode.ImageUrl = qrCodeImageUrl;
                                imgQRcode.Width = 300;
                                imgQRcode.Height = 300;

                            }
                        }

                    }
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                }
            }

            else
            {
                Response.Redirect("HomePage.aspx", false);
            }
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            string userInput = input1.Value + input2.Value + input3.Value + input4.Value + input5.Value + input6.Value;

            if (tfa.GetCurrentPIN(secretIngredient) == userInput)
            {
                Response.Redirect("HomePage.aspx", false);
                Session["Authenticated"] = true;


                log4net.ThreadContext.Properties["ClientIp"] = GetIPAddress();
                log4net.ThreadContext.Properties["UserId"] = userID;
                log.Info("Account logged in");
            }
            else
            {
                lblCheck.Attributes["style"] = "display: inline-block";
                lblCheck.ForeColor = Color.Red;
                lblCheck.Text = "Invalid OTP. Please try again! :)";
            }
        }
    
        protected void LinkButton1_Click(object sender, EventArgs e)
        {


            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();




            // Todo: redirect to login instead

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

            Response.Redirect("Login.aspx", false);


        }


    }
}