using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;


using System.Text.RegularExpressions; // for Regular expression
using System.Drawing; // for change of color
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using static IT2163_PractAssignment.Login;
using System.Net.Mail;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Globalization;

namespace IT2163_PractAssignment
{
    public partial class Registration : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASSIGNMENTDB"].ConnectionString;
        static string finalHash;
        static string salt;
        static string token;
        string userId;
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0,
               plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }



        // Function to encompass the SQL insertion query when creating account.
        public void createAccount()
        {
            string fileName = avatarx.PostedFile.FileName;
            string filePath = "Images/" + avatarx.FileName;
            avatarx.PostedFile.SaveAs(Server.MapPath("~/Images/") + fileName);



            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    //using (SqlCommand cmd = new SqlCommand("INSERT INTO User VALUES(@FirstName @LastName, @CardNo, @Email, @PasswordSalt, @PasswordHash, @BirthDate, @PhotoPath)"))
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[User] VALUES(@FirstName, @LastName, @CardNo, @Email, @PasswordSalt, @PasswordHash, @BirthDate, @PhotoPath, @IV, @Key, @FailedAttempts, @LastFailed, @LastLogin, @EmailVerified, @Token, @TokenExpiry, @ResetPwToken, @ResetPwTokenExpiry, @LastPwSet, @Role)"))

                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {

                            // Date Created, Mobile Verified, Email Verified - potential needs ltr

                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", tb_firstname.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", tb_lastname.Text.Trim());
                            cmd.Parameters.AddWithValue("@CardNo", Convert.ToBase64String(encryptData(tb_card.Text.Trim())));
                            cmd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@BirthDate", tb_date.Text.Trim());
                            cmd.Parameters.AddWithValue("@PhotoPath", filePath);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));

                            cmd.Parameters.AddWithValue("@FailedAttempts", 0);
                            cmd.Parameters.AddWithValue("@LastFailed", DBNull.Value);
                            cmd.Parameters.AddWithValue("@LastLogin", DBNull.Value);

                            cmd.Parameters.AddWithValue("@EmailVerified", 0);
                            cmd.Parameters.AddWithValue("@Token", token);
                            cmd.Parameters.AddWithValue("@TokenExpiry", DateTime.Now.AddMinutes(30));


                            cmd.Parameters.AddWithValue("@ResetPwToken", DBNull.Value);
                            cmd.Parameters.AddWithValue("@ResetPwTokenExpiry", DBNull.Value);

                            cmd.Parameters.AddWithValue("@LastPwSet", DateTime.Now);

                            cmd.Parameters.AddWithValue("@Role", "Guest");

                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }


                    SqlConnection connection = new SqlConnection(MYDBConnectionString);
                    connection.Open();
                    string sql2 = "SELECT * FROM [dbo].[User] WHERE [Email]=@Email";
                    SqlCommand command = new SqlCommand(sql2, connection);
                    command.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                if (reader["Id"] != DBNull.Value)
                                {
                                    userId = reader["Id"].ToString();
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





                    using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[User_Password_History] VALUES(@userID, @FirstPasswordHash, @FirstPasswordSalt, @SecondPasswordHash, @SecondPasswordSalt)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@userID", userId);
                            cmd.Parameters.AddWithValue("@FirstPasswordHash", DBNull.Value);
                            cmd.Parameters.AddWithValue("@SecondPasswordHash", DBNull.Value);
                            cmd.Parameters.AddWithValue("@FirstPasswordSalt", DBNull.Value);
                            cmd.Parameters.AddWithValue("@SecondPasswordSalt", DBNull.Value);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        public bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];


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




        private int CheckPassword(string password)
        {
            int score = 0;
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


        // Verify file




        public static bool verifyName(string name)
        {
            // if it is null -> false.

            if (name == null)
            {
                return false;
            }

            // Check that it is of certain length, and only contains word characters, and space.


            if (!Regex.IsMatch(name, @"^[a-zA-Z][a-zA-Z ]{5,30}$"))
            {
                return false;
            }



            return true;

        }


        public static bool checkEmail(string email) {


            // If email is empty.

            if (email == null)
            {
                return false;
            }

            // Regex to ensure email is correct.

            var pattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";


            if (!Regex.IsMatch(email, pattern)) {
            return false;
            }

            // if email is semantically invalid -- fake email.

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }



            return true;


        }

        public static bool checkCard(string cardNo)
        {

            if (cardNo == null)
            {
                return false;
            }

            if (!Regex.IsMatch(cardNo, @"^4[0-9]{12}(?:[0-9]{3})?$")) {
                return false;
            }


            return true;


        }

        public static bool verifyDate(string dateInput)
        {


            if (dateInput == null)
            {
                return false;
            }

            DateTime d;

            // if user is less than 14 years old or more than 99 years old, deny entry.





            // Age verification

            if ((2021 - Convert.ToInt32(dateInput.Substring(0, 4)) < 14) || (2021 - Convert.ToInt32(dateInput.Substring(0, 4)) > 99))
            {
                return false;
            }

            // Format checking

            if (!DateTime.TryParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
            {
                return false;
            }


            return true;
        }




        // Generating Token based on email
        public static string GenerateToken(string email)
        {
            SHA512Managed hashing = new SHA512Managed();
            string token = email + DateTime.Now.ToString();
            byte[] hashedToken = hashing.ComputeHash(Encoding.UTF8.GetBytes(token));
            token = Convert.ToBase64String(hashedToken);
            return token;
        }


        // Determine whether email has been used. Wardialing is prevented via reCaptcha.

        public bool isUniqueEmail(string email)
        {

            int emailExists = 0;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            connection.Open();


            try
            {
                SqlCommand check_unique_email = new SqlCommand("SELECT COUNT(*) FROM [dbo].[User] WHERE ([Email] = @userId)", connection);
                check_unique_email.Parameters.AddWithValue("@userId", email);
                emailExists = (int)check_unique_email.ExecuteScalar();

                if (emailExists > 0)
                {

                    return false;

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


            return true;


        }


        public string GenerateNewToken()
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            connection.Open();

            SHA512Managed hashing = new SHA512Managed();
            string token = tb_email.Text.Trim() + DateTime.Now.ToString();
            byte[] hashedToken = hashing.ComputeHash(Encoding.UTF8.GetBytes(token));
            token = Convert.ToBase64String(hashedToken);

            try
            {
                string sql = "UPDATE [dbo].[User] SET Token = @TOKEN, TokenExpiry = @TOKENEXPIRY WHERE [Email]=@USERID; ";
                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@USERID", tb_email.Text.Trim());
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



        // Upon clicking on registering button

        protected void submitBtn_Click(object sender, EventArgs e)
        {
            string MsgText = "";
            int rsltCount = 0;

            // Server side validation


            // 


            bool fnameValStatus = verifyName(tb_firstname.Text.Trim());

            if (fnameValStatus == false)
            {
                rsltCount += 1;
                MsgText += "&#8226; Please enter a valid First Name.</br>";
            }


            bool lnameValStatus = verifyName(tb_lastname.Text.Trim());

            if (lnameValStatus == false)
            {
                rsltCount += 1;
                MsgText += "&#8226; Please enter a valid Last Name.</br>";
            }


            bool emailValStatus = checkEmail(tb_email.Text.Trim());

            if (emailValStatus == false)
            {
                rsltCount += 1;
                MsgText += "&#8226; Please enter a valid email.</br>";
            }



            bool cardValStatus = checkCard(tb_card.Text.Trim());

            if (cardValStatus == false)
            {
                rsltCount += 1;
                MsgText += "&#8226; Please enter a valid card number.</br>";
            }

            bool dateVal = verifyDate(tb_date.Text.Trim());

            if (dateVal == false)
            {
                rsltCount += 1;
                MsgText += "&#8226; Please enter a valid date of birth.</br>";
            }




            // file extension verification (only allowed extensions )

            // file size verification

            string filename = Path.GetFileName(avatarx.PostedFile.FileName);
            string extension = Path.GetExtension(filename);

            string contentType = avatarx.PostedFile.ContentType;
            HttpPostedFile file = avatarx.PostedFile;
            var filesize = file.ContentLength;
            byte[] document = new byte[file.ContentLength];
            file.InputStream.Read(document, 0, file.ContentLength);

            // Check for sizing as well.

            if ((extension != ".png" && extension != ".jpg" && extension != "jpeg"))
            {
                //  file is Invalid  
                rsltCount += 1;
                MsgText += "&#8226; Please enter a valid file type (.png, .jpg, .jpeg)</br>";
            }

            if ((filesize / 12100.0) > 10)
            {
                rsltCount += 1;
                MsgText += "&#8226; Image too large; try using a smaller image ( 110 x 110 pixels )</br>";
            }





            bool isUniqueEmailorNot = isUniqueEmail(tb_email.Text.Trim());

            if (isUniqueEmailorNot == false)
            {
                rsltCount += 1;
                MsgText += "&#8226; Email already registered.</br>";
            }



            int scores = CheckPassword(tb_password.Text.Trim());
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

            if (scores < 4)
            {
                rsltCount += 1;
                MsgText += "&#8226; Please enter a stronger password.";
            }

            MsgText += "</br>";



            if (rsltCount > 0)
            {


                lbl_error.Text = MsgText;
                lbl_error.Font.Bold = false;
                lbl_error.Font.Size = 14;
                lbl_error.ForeColor = System.Drawing.Color.Red;


            }



            else 
            {
                 // if server side validation has no issues   

                if (ValidateCaptcha())
                {

                    // Todo: 
                    // Upon registering, create random salt.
                    // append salt to pass.
                    // Create a hash of them & insert into DB
                    // Save a copy of salt as well.


                    // Retrive password from user input
                    string password = tb_password.Text.ToString().Trim(); 

                    // Make a random salt
                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                    byte[] saltByte = new byte[8];

                    //Fills array of bytes with a cryptographically strong sequence of random values.
                    rng.GetBytes(saltByte);
                    salt = Convert.ToBase64String(saltByte);

                    SHA512Managed hashing = new SHA512Managed();
                    string passWithSalt = password + salt;
                    byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(password));
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(passWithSalt));
                    finalHash = Convert.ToBase64String(hashWithSalt);

                    RijndaelManaged cipher = new RijndaelManaged();
                    cipher.GenerateKey();
                    Key = cipher.Key;
                    IV = cipher.IV;
                    token = GenerateToken(tb_email.Text.Trim());

                    createAccount();

                    //if (Page.IsValid = false)
                    //{

                    //}

                    //Send Email

                    string to = "ktykuang@gmail.com"; //To address    
                    string from = "contact.breadington.official@gmail.com"; //From address    
                    MailMessage message = new MailMessage(from, to);

                    string mailbody = "<b>Thank you for your registration! <br> please verify your account here:</b> https://localhost:44399/VerifyEmail.aspx?token=" + HttpUtility.UrlEncode(GenerateNewToken());
                    message.Subject = "SITConnect Account Registration ✔ ";
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
                        Response.Redirect("Login.aspx", false);
                    }


                }
            }



        }






    }


}