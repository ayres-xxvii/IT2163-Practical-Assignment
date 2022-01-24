using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IT2163_PractAssignment
{

    public partial class BeginPassReset : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASSIGNMENTDB"].ConnectionString;
        static string resetPwToken;

        protected void Page_Load(object sender, EventArgs e)
        {

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


        protected void submitBtn_Click(object sender, EventArgs e)
        {
            // Generate ResetPwToken





            string to = "ktykuang@gmail.com"; //To address    
            string from = "contact.breadington.official@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);
            string mailbody = "<h1>Forgot your password?</h1><br> You requested a link to change your password. (If you didn't request this, you can ignore this email.)<br>reset your password by clicking on the link below <br>" +  "https://localhost:44399/EndPassReset.aspx?token=" + HttpUtility.UrlEncode(GeneratePwToken(emailInput.Value.Trim()));
            message.Subject = "SITConnect Password Reset ✔ ";
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
                Response.Redirect("BeginPassConfirm.aspx", false);
            }

            //}

            // Append to url & send email to user




        }
    }
}