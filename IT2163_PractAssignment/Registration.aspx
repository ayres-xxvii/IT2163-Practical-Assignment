<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="IT2163_PractAssignment.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">



<head runat="server">

            <script src="https://www.google.com/recaptcha/api.js"></script>
    <script src="https://www.google.com/recaptcha/api.js?render=6LfZBjkdAAAAAGknUsX869nYU5dl257TXvI1wfUt"></script>

    <script type="text/javascript">
        {

            // Image validation


            function validation(event) {
                validate(event.target.value)
            }

            function validate(file) {
                var ext = file.split(".");
                ext = ext[ext.length - 1].toLowerCase();
                var arrayExtensions = ["jpg", "jpeg", "png", "bmp", "gif"];

                if (arrayExtensions.lastIndexOf(ext) == -1) {
                    alert("Only files with following extensions are allowed: .png, .jpg, .jpeg");
                    document.getElementById("avatarx").value = null;
                }

                else {
                    previewImg(event)
                }

            }


            function previewImg(event) {
                var reader = new FileReader();
                reader.onload = function () {
                    var output = document.getElementById('avatarImg');
                    output.src = reader.result;


                };

                let img = new Image()
                img.src = window.URL.createObjectURL(event.target.files[0])
                img.onload = () => {
                    if (img.width <= 1000 & img.height <= 1000) {
                        reader.readAsDataURL(event.target.files[0]);
                        fileChanged = true;
                    }
                    else {
                        alert("Image too large; try using a smaller image ( 110 x 110 pixels )");

                        document.getElementById('avatarx').value = null;

                    }



                }





            }



            function validatePass() {

            var str = document.getElementById('<%=tb_password.ClientID %>').value.trim();

            if (str.length < 12) {
                document.getElementById("lbl_passwordError").innerHTML = "Password length must be at least 12 characters";
                document.getElementById("lbl_passwordError").style.display = "inline-block";
                document.getElementById("lbl_passwordError").style.color = "Red";
                return false;

            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_passwordError").innerHTML = "Password requires at least 1 number";
                document.getElementById("lbl_passwordError").style.display = "inline-block";
                document.getElementById("lbl_passwordError").style.color = "Red";
                return false;
            }

            //Check for uppercase
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_passwordError").innerHTML = "Password requires at a lower case";
                document.getElementById("lbl_passwordError").style.display = "inline-block";
                document.getElementById("lbl_passwordError").style.color = "Red";
                return false;
            }

            //Check for lower case
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_passwordError").innerHTML = "Password requires an upper case";
                document.getElementById("lbl_passwordError").style.display = "inline-block";
                document.getElementById("lbl_passwordError").style.color = "Red";
                return false;
            }

            //Check for special char
            else if (str.search(/[^a-zA-Z0-9]/) == -1) {

                document.getElementById("lbl_passwordError").innerHTML = "Password requires at least a special character";
                document.getElementById("lbl_passwordError").style.display = "inline-block";
                document.getElementById("lbl_passwordError").style.color = "Red";
                return false;
            }

            else {
                document.getElementById("lbl_passwordError").innerHTML = "Excellent";
                document.getElementById("lbl_passwordError").style.color = "blue";
                return true;

            }



        }



        function fnameValidate() {

            var fname = document.getElementById('<%=tb_firstname.ClientID %>').value



            var regExPattern = "^[a-zA-Z][a-zA-Z ]{5,30}$";// the whitelist

            if (!fname.match(regExPattern))
            {
                document.getElementById("lbl_fnameError").innerHTML = "First name must contain only characters, at least 5 characters";
                document.getElementById("lbl_fnameError").style.display = "inline-block";
                document.getElementById("lbl_fnameError").style.color = "Red";
                return false;
            }

            else {
                document.getElementById("lbl_fnameError").innerHTML = "Excellent";
                document.getElementById("lbl_fnameError").style.color = "blue";
                return true;
            }

        }


        function lnameValidate() {

            var lname = document.getElementById('<%=tb_lastname.ClientID %>').value

            var regExPattern = "^[a-zA-Z][a-zA-Z ]{5,30}$";// the whitelist

            if (!lname.match(regExPattern)) {
                document.getElementById("lbl_lnameError").innerHTML = "Last name must contain only characters, at least 5 characters";
                document.getElementById("lbl_lnameError").style.display = "inline-block";
                document.getElementById("lbl_lnameError").style.color = "Red";
                return false;
            }


            else {
                document.getElementById("lbl_lnameError").innerHTML = "Excellent";
                document.getElementById("lbl_lnameError").style.color = "blue";
                return true;

            }

        }


        function emailValidate() {


            var email = document.getElementById('<%=tb_email.ClientID %>').value.trim();



            if (email.search(/^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i) == -1) {

                document.getElementById("lbl_emailError").innerHTML = "Please enter a valid email address";
                document.getElementById("lbl_emailError").style.display = "inline-block";
                document.getElementById("lbl_emailError").style.color = "Red";
                return false;
            }

            else {

                document.getElementById("lbl_emailError").innerHTML = "Excellent";
                document.getElementById("lbl_emailError").style.color = "blue";
                return true;

            }

        }











                 function cardValidate() {


                var cardNo = document.getElementById('<%=tb_card.ClientID %>').value.trim()


                            if (!cardNo.match("^4[0-9]{12}(?:[0-9]{3})?$")) {
                                document.getElementById("lbl_cardError").innerHTML = "Please enter a valid credit card number";
                                document.getElementById("lbl_cardError").style.display = "inline-block";
                                document.getElementById("lbl_cardError").style.color = "Red";
                                return false;

                            }

                            else {
                                document.getElementById("lbl_cardError").innerHTML = "Excellent";
                                document.getElementById("lbl_cardError").style.color = "blue";
                                return true;
                            }

                        }
            


            function dateValidate() {


                // YoB from user

                var useryob = document.getElementById('<%=tb_date.ClientID %>').value.substring(0, 4);

                // Current year

                var currentYear = new Date().getFullYear()


                if (currentYear - useryob < 6) {
                    document.getElementById("lbl_dateError").innerHTML = "You are not eligible to sign up at this time";
                    document.getElementById("lbl_dateError").style.display = "inline-block";
                    document.getElementById("lbl_dateError").style.color = "Red";
                    return false;

                }


                return true;
            }


        }






    </script>

    <style>

h1 {
    text-align: center;
}

        .g-recaptcha {
margin: 0 auto;display: table
}

body {
    margin-bottom: 70px;

    /*background-color: #f5f5f5*/
}

.registration-form {
    padding: 10px 0;
}

    .registration-form form {
        background-color: #fff;
        max-width: 600px;
        margin: auto;
        padding: 50px 70px;
        border-top-left-radius: 30px;
        border-top-right-radius: 30px;
        box-shadow: 0px 2px 10px rgba(0, 0, 0, 0.075);
    }

    .registration-form .form-icon {
        text-align: center;
        background-color: #5891ff;
        border-radius: 50%;
        font-size: 40px;
        color: white;
        width: 100px;
        height: 100px;
        margin: auto;
        margin-bottom: 50px;
        line-height: 100px;
    }

    .registration-form .item {
        border-radius: 20px;
        margin-bottom: 25px;
        padding: 10px 20px;
    }

    .registration-form .create-account {
        border-radius: 30px;
        padding: 10px 20px;
        font-size: 18px;
        font-weight: bold;
        background-color: #5791ff;
        border: none;
        color: white;
        margin-top: 20px;
    }

    .registration-form .social-media {
        max-width: 600px;
        background-color: #fff;
        margin: auto;
        padding: 35px 0;
        text-align: center;
        border-bottom-left-radius: 30px;
        border-bottom-right-radius: 30px;
        color: #9fadca;
        box-shadow: 0px 2px 10px rgba(0, 0, 0, 0.075);
    }

    .registration-form .social-icons {
        margin-top: 30px;
        margin-bottom: 16px;
    }

        .registration-form .social-icons a {
            font-size: 23px;
            margin: 0 3px;
            color: #5691ff;
            border: 1px solid;
            border-radius: 50%;
            width: 45px;
            display: inline-block;
            height: 45px;
            text-align: center;
            background-color: #fff;
            line-height: 45px;
        }

            .registration-form .social-icons a:hover {
                text-decoration: none;
                opacity: 0.6;
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

    <title>Registration Page</title>

        <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">


</head>
<body style="background-color:#f5f5f5; margin-top:5px;">

    <div class="registration-form">



       <form id="form2" runat="server" onsubmit="return validatexx()">

    <%--<form id="form1" runat="server" >--%>
        <h4 class="text-center">Registration</h4>
        <hr />

        <asp:Label ID="lbl_error" runat="server" Text=""></asp:Label>

              <img id="avatarImg" style="
width: 100px;
  display: block;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 7px;" src="https://i.pinimg.com/originals/51/f6/fb/51f6fb256629fc755b8870c801092942.png"
        class="avatar" alt="Avatar">


            <div class="form-group">
      <label>Profile Image</label>
        <%--<input id ="avatarx" class="form-control" type="file"   onChange="validation(event)" accept=""   name="avatar"onChange="validate(this.value)" required  />--%>
        <asp:FileUpload ID="avatarx" class="form-control"  onChange="validate(this.value)" runat="server"  required />
  </div>  

        <div class="form-group">
            <label asp-for="Name" class="control-label">First Name: </label>
            <asp:TextBox ID="tb_firstname" class="form-control" runat="server"  AutoCompleteType="Disabled" required ></asp:TextBox>
            <asp:Label ID="lbl_fnameError" runat="server" Text="" style="display:none"></asp:Label>

        </div>

        <%--lbl_passwordError--%>

        <div class="form-group">
            <label asp-for="Name" class="control-label">Last Name: </label>
            <asp:TextBox ID="tb_lastname" class="form-control"  runat="server" AutoCompleteType="Disabled" required ></asp:TextBox>
            <asp:Label ID="lbl_lnameError" runat="server" Text="" style="display:none"></asp:Label>


        </div>
        <div class="form-group">
            <label asp-for="Name" class="control-label" >Email: </label>
            <asp:TextBox ID="tb_email" class="form-control" runat="server"  AutoCompleteType="Disabled" required ></asp:TextBox>
            <asp:Label ID="lbl_emailError" runat="server" Text="" style="display:none"></asp:Label>

        </div>



        <div class="form-group">
            <label asp-for="Name" class="control-label" >Password: </label>
            <asp:TextBox TextMode="Password" ID="tb_password" type class="form-control" runat="server" AutoCompleteType="Disabled"  required ></asp:TextBox>
            <asp:Label ID="lbl_passwordError" runat="server" Text="" style="display:none"></asp:Label>
        </div>





                <div class="form-group">
            <label asp-for="Name" class="control-label" >Date of Birth: </label>
            <asp:TextBox TextMode="Date"  ID="tb_date" class="form-control" AutoCompleteType="Disabled" runat="server" required ></asp:TextBox>
            <asp:Label ID="lbl_dateError" runat="server" Text="" style="display:none"></asp:Label>
        </div>

        <br />
        <h4 style="text-align:center">Credit / Debit Card</h4>
        <hr />

                <div class="form-group">
            <label asp-for="Name" class="control-label" >Card Number: </label>
            <asp:TextBox ID="tb_card" class="form-control" AutoCompleteType="Disabled" runat="server" required ></asp:TextBox>
            <asp:Label ID="lbl_cardError" runat="server" Text="" style="display:none"></asp:Label>
        </div>
        <br />

<input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>


        <div class="form-group">
            <asp:Button type="submit" ID="submitBtn" runat="server"  Text="Create Account" class="btn btn-primary btn-block" OnClick="submitBtn_Click"/>
            
        </div>



    </form>
            <div class="social-media">
    </div>
        </div>

    <script>


        function validatexx() {



            // here


            //card validation
            

            if (!validatePass()) return false;


            if (!cardValidate()) return false;


            if (!dateValidate()) return false;

            if (!emailValidate()) return false;

            if (!fnameValidate()) return false;

            if (!lnameValidate()) return false;








            // date validation












        }

        grecaptcha.ready(function () {
            grecaptcha.execute('6LfZBjkdAAAAAGknUsX869nYU5dl257TXvI1wfUt', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });




    </script>


    

     <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js" integrity="sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1" crossorigin="anonymous"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js" integrity="sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM" crossorigin="anonymous"></script>



</body>

<script>




</script>


</html>
