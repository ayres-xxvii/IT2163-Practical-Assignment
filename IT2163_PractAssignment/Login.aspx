<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="IT2163_PractAssignment.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
        <script src="https://www.google.com/recaptcha/api.js"></script>


    <script src="https://www.google.com/recaptcha/api.js?render=6LfZBjkdAAAAAGknUsX869nYU5dl257TXvI1wfUt"></script>

    <title>SITConnect | Login</title>

    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">

    <link href="css/nav.css" rel="stylesheet" />


                                <link href="css/login.css" rel="stylesheet" />

    

    <style>

        .g-recaptcha {
margin: 0 auto;display: table
}
        .errorlbl {
            display: none;
            color:red;
            text-align: center;
            margin-left: 70px;

        }

        #g-recaptcha-response {
    display: block !important;
    position: absolute;
    margin: -78px 0 0 0 !important;
    width: 302px !important;
    height: 76px !important;
    z-index: -999999;
    opacity: 0;
}

    </style>



</head>



<body id="show-page">

    
    <nav>
    <ul>
        <li class="link1">
            <a asp-area="" asp-controller="Home" asp-action="Index">Home</a>
        </li>
        <li id="link2">
            <a asp-area="" asp-controller="Products" asp-action="Index">Menu</a>

        </li>

        <li class="link3">
            <a asp-area="" asp-controller="Dedication" asp-action="Index">About</a>

        </li>

        <li id="link4">
            <a asp-area="" asp-controller="Reviews" asp-action="Index">Testimonial</a>
        </li>


        <li id="link5">
            <a asp-area="" asp-controller="Blog" asp-action="Index">Blog</a>
        </li>



<%--                                                        <li class="link1" style="color:black">
                                                    <a href="/Login.aspx">Home</a>

                                                </li>
                                                <li id="link2">
                                                    <a href="/Login.aspx">Menu</a>

                                                </li>

                                                <li class="link3">
                                                    <a href="/Login.aspx">About</a>

                                                </li>

                                                <li id="link4">
                                                    <a href="/Login.aspx">Testimonial</a>
                                                </li>


                                                <li id="link5">
                                                    <a href="/Profile.aspx"  asp-action="Profile">Profile</a>

                                                </li>


                                                <li id="link6" style="margin-top: -4px; position: absolute; right: 0;">

                                                </li>--%>



    </ul>
    <div class="handle"></div>
</nav>

<div class="container" style="padding-top: 20px;">
    <div class="row">
        <div class="col-lg-10 col-xl-9 mx-auto">
            <div class="card card-signin flex-row my-5">
                <div class="card-img-left d-none d-md-flex" style="        background: scroll center url('https://i.ibb.co/FJ1QfVn/SITConnect.png');
">
                    <!-- Background image for card set in CSS! -->
                    <!-- <a class="prev" onclick="plusSlides(-1)" style="margin-top: 200px; color: white;">&#10094;</a>
                    <a class="next" onclick="plusSlides(1)" style="margin-top: 200px; margin-left: 335px; color: white; size: 200%;">&#10095;</a> -->
                </div>
                <div class="card-body">
                    <h5 class="card-title text-center">
                        Welcome to SITConnect!

                    </h5>

                    <form id="form1" runat="server" class="form-signin" asp-action="ProcessLogin" onsubmit="return validatexx()">
                        <div asp-validation-summary="ModelOnly" class="text-danger"></div>
                        <div class="form-label-group">


                            <asp:TextBox ID="tb_userEmail" runat="server" type="username" name="username" class="form-control" required></asp:TextBox>

                            <label asp-for="UserName" for="inputUserame" class="control-label">Email</label>



                        </div>

                        <div class="form-label-group">
                                                         <asp:TextBox TextMode="Password" ID="tb_password" runat="server" class="form-control" required></asp:TextBox>

                            <label  asp-for="Password" for="inputEmail" class="control-label">Password</label>


                            <asp:HyperLink ID="HyperLink1" class="d-block text-center mt-2 small" href="/BeginPassReset.aspx" runat="server">Forgot password?</asp:HyperLink>

                            <span asp-validation-for="Password" class="text-danger"></span>

                            <br />

                            <%--<div class="g-recaptcha" data-sitekey="6Lc8MQ0dAAAAAClYYc_nZ03NF7B0h2E-VFQ59cGG"></div>--%>
                            <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>

                        </div>
                         <asp:Label ID="lblMessage" CssClass="errorlbl" runat="server" EnableViewState="False">Error message here (lblMessage)</asp:Label>

                        <hr>

                        <asp:Button class="btn btn-lg btn-primary btn-block text-uppercase" ID="btn_login" runat="server" Text="Login" OnClick="LoginMe" />
                        <a class="d-block text-center mt-2 small" href="/Registration.aspx">Register</a>

                        <hr class="my-4">



                    

                    </form>



                </div>
            </div>
        </div>
    </div>
</div>







    <script>


        function emailValidate() {


            var email = document.getElementById('<%=tb_userEmail.ClientID %>').value.trim();

                    if (email.search(/^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i) == -1) {

                        document.getElementById("lblMessage").innerHTML = "Please enter a valid email address.";
                        document.getElementById("lblMessage").style.display = "inline-block";
                        document.getElementById("lblMessage").style.color = "Red";
                        return false;
                    }

                    else {

                        return true;

                    }

        }













        function validatexx() {



            if (!emailValidate()) return false;



        }






        grecaptcha.ready(function () {
            grecaptcha.execute('6LfZBjkdAAAAAGknUsX869nYU5dl257TXvI1wfUt', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>

        
<%--    <script>
        window.onload = function () {
            var $recaptcha = document.querySelector('#g-recaptcha-response');

            if ($recaptcha) {
                $recaptcha.setAttribute("required", "required");
            }
        };
    </script>--%>





    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js" integrity="sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1" crossorigin="anonymous"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js" integrity="sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM" crossorigin="anonymous"></script>


</body>
</html>
