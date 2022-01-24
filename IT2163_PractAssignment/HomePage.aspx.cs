using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IT2163_PractAssignment
{

    public partial class HomePage : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASSIGNMENTDB"].ConnectionString;
        byte[] Key;
        byte[] IV;
        byte[] cardno = null;
        string userID = null;
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



        private void BindGrid()
        {
            using (SqlConnection con = new SqlConnection(MYDBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Date, Level, IP_Address, Message FROM [dbo].[Log] WHERE UserID=@USERID"))
                {
                    //cmd.Parameters.AddWithValue("@RESETPWTOKEN", userToken);
                    cmd.Parameters.AddWithValue("@USERID", (string)Session["userID"]);
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            GridView1.DataSource = dt;
                            GridView1.DataBind();
                        }
                    }
                }
            }
        }


        protected string decryptData(byte[] cipherText)
        {
            string plainText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                // Create the streams used for decryption
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return plainText;
        }



        protected void displayUserProfile(string userid)
        {
            string userRole = null;

            bool emailVerified = false;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT * FROM [dbo].[User] WHERE Email=@userId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@userId", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        if (reader["Email"] != DBNull.Value)
                        {
                            lbl_user.InnerText = reader["Email"].ToString();
                        }

                        if (reader["EmailVerified"] != DBNull.Value)
                        {
                            emailVerified = (bool)reader["EmailVerified"];
                        }


                        if (reader["PhotoPath"] != DBNull.Value)
                        {
                            avatarImg.Src = reader["PhotoPath"].ToString();
                        }





                        if (reader["LastPwSet"] != DBNull.Value)
                        {
                            lastPwSet = (DateTime)reader["LastPwSet"];
                        }

                        else
                        {

                            lastPwSet = Convert.ToDateTime("1900-01-01 00:00:00");

                        }

                        if (reader["IV"] != DBNull.Value)
                        {
                            IV = Convert.FromBase64String(reader["IV"].ToString());
                        }
                        if (reader["Key"] != DBNull.Value)
                        {
                            Key = Convert.FromBase64String(reader["Key"].ToString());
                        }
                        if (reader["CardNo"] != DBNull.Value)
                        {
                            // Convert base64 in db to byte[]
                            cardno = Convert.FromBase64String(reader["CardNo"].ToString());
                        }

                        if (reader["Role"] != DBNull.Value)
                        {
                            userRole = (reader["Role"].ToString());
                        }
                    }

                    

                    lbl_cardno.Text = HttpUtility.HtmlEncode($"Credit Card No: {decryptData(cardno)}");


                }
            }//try
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }


            if (emailVerified != true && userRole != "Staff") Response.Redirect("VerifyEmail.aspx", false);

            // Maximum password age

            if (lastPwSet != Convert.ToDateTime("1900-01-01 00:00:00"))
            {
                if (DateTime.Now.Subtract(lastPwSet).TotalDays > 30) Response.Redirect("PasswordReset.aspx", false);
            }


            if (userRole.Trim() != "Guest")
            {
                staffNav.Style.Add("display", "inline");
                guestNav.Style.Add("display", "none");
            }



        }



        protected void Page_Load(object sender, EventArgs e)
        {








            // Check their session


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


                        if (!IsPostBack)
                        {


                            try
                            {
                                this.BindGrid();

                            }
                            catch (WebException ex)
                            {
                                throw ex;
                            }



                        }


                        userID = (string)Session["userID"];
                        displayUserProfile(userID);




                    }

                }
            }
            else
            {






                Response.Redirect("Login.aspx", false);
            }



        }


        //Logout button
        protected void LinkButton1_Click(object sender, EventArgs e)
        {


            log4net.ThreadContext.Properties["ClientIp"] = GetIPAddress();
            log4net.ThreadContext.Properties["UserId"] = userID;
            log.Info("Account logged out");

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



        }

    }
}