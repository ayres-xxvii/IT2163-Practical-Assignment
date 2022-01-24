<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BeginPassConfirm.aspx.cs" Inherits="IT2163_PractAssignment.BeginPassConfirm" %>

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
</style>
</head>
<body>
<div class="container h-100">
  <div class="row h-100">
  <div class="col-sm-10 col-md-8 col-lg-6 mx-auto d-table h-100">
    <div class="d-table-cell align-middle">

      <div class="text-center mt-4">
        <h1 class="h2">Reset password</h1>
      </div>

      <div class="card">
        <div class="card-body">
          <div class="m-sm-4">
            <form id="subscribeForm" method="POST" action="">

              <div class="text-center mt-3">
                <p>Password reset email has been sent! (provided the email exists)</p>
                <p>Please check your email.</p>
                <!-- <button type="submit" class="btn btn-lg btn-primary">Reset password</button> -->
                <a class="btn btn-primary btn-lg" href="/Login.aspx" role="button">Back</a>

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
