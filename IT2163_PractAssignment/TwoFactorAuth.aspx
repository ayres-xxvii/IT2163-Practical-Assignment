<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TwoFactorAuth.aspx.cs" Inherits="IT2163_PractAssignment.TwoFactorAuth" EnableEventValidation="false" %>

<!DOCTYPE html>
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SITConnect | Login - 2FA </title>

    <style>
        body{
    background:#eee;
}
.card {
    box-shadow: 0 20px 27px 0 rgb(0 0 0 / 5%);
}
.card {
    position: relative;
    display: flex;
    flex-direction: column;
    min-width: 0;
    word-wrap: break-word;
    background-color: #fff;
    background-clip: border-box;
    border: 0 solid rgba(0,0,0,.125);
    border-radius: 1rem;
}
.img-thumbnail {
    padding: .25rem;
    background-color: #ecf2f5;
    border: 1px solid #dee2e6;
    border-radius: .25rem;
    max-width: 100%;
    height: auto;
}
.avatar-lg {
    height: 150px;
    width: 150px;
}

.checking_label {
    display: none;
}

    </style>
</head>
<body>
    <form id="form1" runat="server">


    <div class="container" style="margin-top:80px">
    <br>
    <div class="row">
        <div class="col-lg-5 col-md-7 mx-auto my-auto">
            <div class="card">
                <div class="card-body px-lg-5 py-lg-5 text-center">
<%--                    <img src="https://bootdey.com/img/Content/avatar/avatar7.png" class="rounded-circle avatar-lg img-thumbnail mb-4" alt="profile-image">--%>

                            <asp:Image ID="imgQRcode" runat="server" />

                    <h2 class="text-info">2FA Security</h2>
                    <p class="mb-4">Enter 6-digits code from your authenticator app.</p>
                    <form>
                        <div class="row mb-4">
                            <div class="col-lg-2 col-md-2 col-2 ps-0 ps-md-2">
                                <input id="input1" runat="server"  type="text" class="form-control text-lg text-center" placeholder="_" aria-label="2fa">



                            </div>
                            <div class="col-lg-2 col-md-2 col-2 ps-0 ps-md-2">
                                <input  id="input2" runat="server" type="text" class="form-control text-lg text-center" placeholder="_" aria-label="2fa">
                            </div>
                            <div class="col-lg-2 col-md-2 col-2 ps-0 ps-md-2">
                                <input  id="input3" runat="server" type="text" class="form-control text-lg text-center" placeholder="_" aria-label="2fa">
                            </div>
                            <div class="col-lg-2 col-md-2 col-2 pe-0 pe-md-2">
                                <input  id="input4" runat="server" type="text" class="form-control text-lg text-center" placeholder="_" aria-label="2fa">
                            </div>
                            <div class="col-lg-2 col-md-2 col-2 pe-0 pe-md-2">
                                <input  id="input5" runat="server" type="text" class="form-control text-lg text-center" placeholder="_" aria-label="2fa">
                            </div>
                            <div class="col-lg-2 col-md-2 col-2 pe-0 pe-md-2">
                                <input  id="input6" runat="server" type="text" class="form-control text-lg text-center" placeholder="_" aria-label="2fa">
                            </div>
                        </div>

                        <asp:Label ID="lblCheck" CssClass="checking_label" runat="server"></asp:Label>
                        <br />

                                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Back to login</asp:LinkButton>


                        <br />

                            <asp:Button ID="Button1" type="button" class="btn bg-info btn-lg my-4" runat="server" Text="Continue" OnClick="Button1_Click" />


                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>

            </form>


        <script type ="text/javascript" src="Scripts/fingerprint2.js"></script>
    <script type ="text/javascript">
        var fp = new Fingerprint2();
        fp.get(function (result, components) {
            document.querySelector("#lblFingerprint").textContent = result;
        });
    </script>

    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js" integrity="sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1" crossorigin="anonymous"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js" integrity="sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM" crossorigin="anonymous"></script>

</body>
</html>
