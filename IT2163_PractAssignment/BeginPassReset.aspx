<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BeginPassReset.aspx.cs" Inherits="IT2163_PractAssignment.BeginPassReset" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SITConnect | Reset Password</title>
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
.submitBtn {
    color: white;
    background-color:#007bff;
    width: 100%;   border: 0;
                border-radius: 2rem;
                box-shadow: 0 0.5rem 1rem 0 rgba(0, 0, 0, 0.1);
                overflow: hidden;   font-size: 80%;
  border-radius: 5rem;
  letter-spacing: .1rem;
  font-weight: bold;
  padding: 1rem;
  transition: all 0.2s;
}
</style>
</head>
<body>

    <div class="container h-100">
  <div class="row h-100">
  <div class="col-sm-10 col-md-8 col-lg-6 mx-auto d-table h-100">
    <div class="d-table-cell align-middle">

      <div class="text-center mt-4">
        <h1 class="h2">Reset password</h1>
        <p class="lead">
          Enter your email to reset your password.
        </p>
      </div>

      <div class="card">
        <div class="card-body">
          <div class="m-sm-4">
            <form id="subscribeForm" method="POST" action="" runat="server">
              <div class="form-group">
                <label>Email</label>



                <input runat="server" class="form-control form-control-lg"  type="email" id="emailInput" placeholder="Enter your email"/>



              </div>
              <div class="text-center mt-3">
                  <asp:Button ID="submitBtn" runat="server" Text="Reset Password" CssClass="submitBtn" type="submit" class="btn btn-primary btn-lg text-uppercase" OnClick="submitBtn_Click"/>

              </div>
            </form>
          </div>
        </div>
      </div>

    </div>
  </div>
</div>
</div>
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js" integrity="sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1" crossorigin="anonymous"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js" integrity="sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM" crossorigin="anonymous"></script>
</body>
</html>
