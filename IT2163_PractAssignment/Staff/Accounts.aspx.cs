using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IT2163_PractAssignment.Staff
{
    public partial class Accounts : System.Web.UI.Page
    {
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



                        var userID = (string)Session["userID"];




                    }

                }
            }
            else
            {
                throw new HttpException(403, "Access denied");
            }


        }
    }
}