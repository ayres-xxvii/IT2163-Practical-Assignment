<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PasswordReset.aspx.cs" Inherits="IT2163_PractAssignment.PasswordReset" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SITConnect | Password Reset </title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">
    <style>
         body{
                  margin-top: 120px;
    background-color: #f2f3f8;
}
.card {
    margin-bottom: 1.5rem;
    box-shadow: 0 1px 15px 1px rgba(52,40,104,.08);
}
.card {
    position: relative;
    display: -ms-flexbox;
    display: flex;
    -ms-flex-direction: column;
    flex-direction: column;
    min-width: 0;
    word-wrap: break-word;
    background-color: #fff;
    background-clip: border-box;
    border: 1px solid #e5e9f2;
    border-radius: .2rem;
}
    </style>




        <script>
            function validate() {
            

            var str = document.getElementById('password').value;

            if (str.length < 12) {
                document.getElementById("lbl_passwordError").innerHTML = "Password length must be at least 12 characters";
            document.getElementById("lbl_passwordError").style.display = "inline-block";
            document.getElementById("lbl_passwordError").style.color = "Red";

               }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_passwordError").innerHTML = "Password requires at least 1 number";
            document.getElementById("lbl_passwordError").style.display = "inline-block";
            document.getElementById("lbl_passwordError").style.color = "Red";
            return ("no_number");
               }

            //Check for uppercase
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_passwordError").innerHTML = "Password requires at a lower case";
            document.getElementById("lbl_passwordError").style.display = "inline-block";
            document.getElementById("lbl_passwordError").style.color = "Red";
            return ("no_number");
               }

            //Check for lower case
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_passwordError").innerHTML = "Password requires an upper case";
            document.getElementById("lbl_passwordError").style.display = "inline-block";
            document.getElementById("lbl_passwordError").style.color = "Red";
            return ("no_number");
               }

            //Check for special char
            else if (str.search(/[^a-zA-Z0-9]/) == -1) {

                document.getElementById("lbl_passwordError").innerHTML = "Password requires at least a special character";
            document.getElementById("lbl_passwordError").style.display = "inline-block";
            document.getElementById("lbl_passwordError").style.color = "Red";
            return ("no_number");
               }

            else {
                document.getElementById("lbl_passwordError").innerHTML = "Excellent";
            document.getElementById("lbl_passwordError").style.color = "blue";

               }
            



           }
    </script>



</head>
<body>

    
    <div class="container h-100">
  <div class="row h-100">
  <div class="col-sm-10 col-md-8 col-lg-6 mx-auto d-table h-100">
    <div class="d-table-cell align-middle">

      <div class="text-center mt-4">
        <p class="lead">
          Enter your new password.
        </p>
      </div>

      <div class="card">
        <div class="card-body">
          <div class="m-sm-4">
            <form id="subscribeForm" method="POST" action="">
    <div class="form-group">
      <label for="password">Password</label>
      <input type="password" onkeyup="validate()" runat="server" name="password" class="form-control" minlength="4" id="passwordTB"
        required>

    </div>
    <div class="form-group">
      <label for="password2">Confirm Password</label>
      <input type="password" runat="server" name="password2" class="form-control" minlength="4"
        id="password_confirmTB" oninput="check(this)" required>
             <asp:Label ID="lbl_passwordError" runat="server" Text="" style="display:none"></asp:Label>
    </div>
              <div class="text-center mt-3">
<%--                <input runat="server" class="btn btn-primary btn-lg" type="submit" value="Submit">--%>
                              <form id="resetPwForm" method="POST" runat="server">

                  <asp:Button ID="submitBtn" runat="server" class="btn btn-primary btn-lg" type="button" Text="Reset Password" value="submit" Width="100%" OnClick="submitBtn_Click"  />
                                  </form>

                <!-- <button type="submit" class="btn btn-lg btn-primary">Reset password</button> -->
              </div>
            </form>
          </div>
        </div>
      </div>

    </div>
  </div>
</div>
</div>

<script>

    function check(input) {
        if (input.value != document.getElementById('password').value) {
            input.setCustomValidity('Password Must be Matching.');
        } else {
            // input is valid -- reset the error message
            input.setCustomValidity('');
        }
    }
</script>
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js" integrity="sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1" crossorigin="anonymous"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js" integrity="sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM" crossorigin="anonymous"></script>


</body>
</html>
