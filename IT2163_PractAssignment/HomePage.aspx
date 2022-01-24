<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="IT2163_PractAssignment.HomePage" %>

<!DOCTYPE html>

<link rel="stylesheet" href="https://pro.fontawesome.com/releases/v5.10.0/css/all.css" integrity="sha384-AYmEC3Yw5cVb3ZcuHtOA93w35dYTsvhLPVnYs9eStHfGJvOvKxVfELGroGkvsg+p" crossorigin="anonymous" />


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SITConnect | Home </title>

    
    <link href="https://fonts.googleapis.com/css?family=Oswald:300,400,700" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:200,300,400,600,700,900,200italic,300italic,400italic,600italic,700italic,900italic" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.js" integrity="sha256-H+K7U5CnXl1h5ywQfKtSj8PCmoN9aaq30gDh27Xc0jk=" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="https://pro.fontawesome.com/releases/v5.10.0/css/all.css" integrity="sha384-AYmEC3Yw5cVb3ZcuHtOA93w35dYTsvhLPVnYs9eStHfGJvOvKxVfELGroGkvsg+p" crossorigin="anonymous" />
    <link href="css/home.css" rel="stylesheet" />
    <link href="css/nav.css" rel="stylesheet" />

    <style>
    img {
    width: 90px;
    height: 90px;
    -webkit-border-radius: 100px;
    -moz-border-radius: 100px;
    border-radius: 100px;
  }

    </style>

</head>




<body id="show-page">


    

    


    
    <nav>
                                    <form id="form1" runat="server" >

                                        <div id="guestNav" runat="server" >
                                            <ul>
                                                <li class="link1">
                                                    <a href="/HomePage.aspx">Home</a>
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
                                                    <a asp-area="" asp-controller="Profile" asp-action="Profile">Blog</a>
                                                </li>


                                                                                                <li class="link1" style="color:black">

                                              <%--  </li>
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

                                                <li id="link5" style="margin-top: -4px; position: absolute; right: 0;">
                                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">
                <i class="fa fa-sign-out" style="font-size:25px"></i>
                                                    </asp:LinkButton>
                                                </li>


                                            </ul>
                                            <div class="handle"></div>

                                        </div>


        <div id="staffNav" runat="server" style="display:none">

                                                <ul>
        <li class="link1">
            <a asp-area="" asp-controller="Home" asp-action="Index">Home</a>
        </li>
        <li id="link2">
            <a asp-area="" asp-controller="Sales" asp-action="Index">Sales</a>

        </li>

        <li class="link3">
            <a asp-area="" asp-controller="Reports" asp-action="Index">Reports</a>

        </li>

        <li id="link4">
            <a asp-area="" asp-controller="Accounts" asp-action="Index">Accounts</a>
        </li>

        
        <li id="link5">
            <a asp-area="" asp-controller="Dashboard" asp-action="Index">Dashboard</a>
        </li>

        
        <li id="link5" style="margin-top:-4px; position:absolute; right:0;">
                    <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton1_Click">
                <i class="fa fa-sign-out" style="font-size:25px"></i>
                    </asp:LinkButton>
        </li>



    </ul>




    <div class="handle"></div>
                                        </div>

</nav>



    <div id="parallax-world-of-ugg" style="margin-top: -50px">


        <section>
            <div class="title">
                <h3>WELCOME TO</h3>
                <h1>SITConnect</h1>
                <br />
                                                <img src="" runat="server" alt="Profile Picture" id="avatarImg">

                <h3 id="lbl_user" style="text-decoration:underline" runat="server"></h3>
<br />

                <asp:Label ID="lbl_cardno" runat="server" Text=""></asp:Label>                <br />


                <h3 style="color:red" id="countDown"></h3>

                <br />

                <h3 style="color:black">~ My Activity ~</h3>


                                        <asp:GridView ID="GridView1" runat="server"   autogeneratecolumns="False"  HorizontalAlign="Center"
>
                                <Columns>
         <asp:BoundField DataField="Date" HeaderText="Date" 
            SortExpression="Date" />

                                             <asp:BoundField DataField="Level" HeaderText="Category" 
            SortExpression="Date" />

                                             <asp:BoundField DataField="Message" HeaderText="Operation" 
            SortExpression="Message" />

                                             <asp:BoundField DataField="IP_Address" HeaderText="IP address" 
            SortExpression="Date" />


    </Columns>

                        </asp:GridView>

                                            </form>


        <div>
            
            

        </div>



            </div>
        </section>

        <section>
            <div class="parallax-one">
                <h2>SOUTHERN CALIFORNIA</h2>
            </div>
        </section>

        <section>
            <div class="block">
                <p><span class="first-character sc">I</span>n 1978, Brian Smith landed in Southern California with a bag of sheepskin boots and hope. He fell in love with the sheepskin experience and was convinced the world would one day share this love. The beaches of Southern California had long been an epicenter of a relaxed, casual lifestyle, a lifestyle that Brian felt was a perfect fit for his brand. So he founded the UGG brand, began selling his sheepskin boots and they were an immediate sensation. By the mid 1980's, the UGG brand became a symbol of relaxed southern California culture, gaining momentum through surf shops and other shops up and down the coast of California, from San Diego to Santa Cruz. UGG boots reached beyond the beach, popping up in big cities and small towns all over, and in every level of society. Girls wore their surfer boyfriend's pair of UGG boots like a letterman jacket. When winter came along, UGG boots were in ski shops and were seen in lodges from Mammoth to Aspen.</p>
                <p class="line-break margin-top-10"></p>
                <p class="margin-top-10">The UGG brand began to symbolize those who embraced sport and a relaxed, active lifestyle. More than that, an emotional connection and a true feeling of love began to grow for UGG boots, just as Brian had envisioned. People didn't just like wearing UGG boots, they fell in love with them and literally could not take them off. By the end of the 90's, celebrities and those in the fashion world took notice of the UGG brand. A cultural shift occurred as well - people were embracing, and feeling empowered, by living a more casual lifestyle and UGG became one of the symbols of this lifestyle. By 2000, a love that began on the beaches had become an icon of casual style. It was at this time that the love for UGG grew in the east, over the Rockies and in Chicago. In 2000, UGG Sheepskin boots were first featured on Oprah's Favorite Things® and Oprah emphatically declared that she "LOOOOOVES her UGG boots." From that point on, the world began to notice.</p>
            </div>
        </section>

        <section>
            <div class="parallax-two">
                <h2>NEW YORK</h2>
            </div>
        </section>

        <section>
            <div class="block">
                <p><span class="first-character ny">B</span>reaking into the New York fashion world is no easy task. But by the early 2000's, UGG Australia began to take it by storm. The evolution of UGG from a brand that made sheepskin boots, slippers, clogs and sandals for an active, outdoor lifestyle to a brand that was now being touted as a symbol of a stylish, casual and luxurious lifestyle was swift. Much of this was due to a brand repositioning effort that transformed UGG into a high-end luxury footwear maker. As a fashion brand, UGG advertisements now graced the pages of Vogue Magazine as well as other fashion books. In the mid 2000's, the desire for premium casual fashion was popping up all over the world and UGG was now perfectly aligned with this movement.</p>
                <p class="line-break margin-top-10"></p>
                <p class="margin-top-10">Fueled by celebrities from coast to coast wearing UGG boots and slippers on their downtime, an entirely new era of fashion was carved out. As a result, the desire and love for UGG increased as people wanted to go deeper into this relaxed UGG experience. UGG began offering numerous color and style variations on their sheepskin boots and slippers. Cold weather boots for women and men and leather casuals were added with great success. What started as a niche item, UGG sheepskin boots were now a must-have staple in everyone's wardrobe. More UGG collections followed, showcasing everything from knit boots to sneakers to wedges, all the while maintaining that luxurious feel UGG is known for all over the world. UGG products were now seen on runways and in fashion shoots from coast to coast. Before long, the love spread even further.</p>
            </div>
        </section>

        <section>
            <div class="parallax-three">
                <h2>ENCHANTED FOREST</h2>
            </div>
        </section>

        <section>
            <div class="block">
                <p><span class="first-character atw">W</span>hen the New York fashion community notices your brand, the world soon follows. The widespread love for UGG extended to Europe in the mid-2000's along with the stylish casual movement and demand for premium casual fashion. UGG boots and shoes were now seen walking the streets of London, Paris and Amsterdam with regularity. To meet the rising demand from new fans, UGG opened flagship stores in the UK and an additional location in Moscow. As the love spread farther East, concept stores were opened in Beijing, Shanghai and Tokyo. UGG Australia is now an international brand that is loved by all. This love is a result of a magical combination of the amazing functional benefits of sheepskin and the heightened emotional feeling you get when you slip them on your feet. In short, you just feel better all over when you wear UGG boots, slippers, and shoes.</p>
                <p class="line-break margin-top-10"></p>
                <p class="margin-top-10">In 2011, UGG will go back to its roots and focus on bringing the active men that brought the brand to life back with new styles allowing them to love the brand again as well. Partnering with Super Bowl champion and NFL MVP Tom Brady, UGG will invite even more men to feel the love the rest of the world knows so well. UGG will also step into the world of high fashion with UGG Collection. The UGG Collection fuses the timeless craft of Italian shoemaking with the reliable magic of sheepskin, bringing the luxurious feel of UGG to high end fashion. As the love for UGG continues to spread across the world, we have continued to offer new and unexpected ways to experience the brand. The UGG journey continues on and the love for UGG continues to spread.</p>
            </div>
        </section>

    </div>

    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css">
    <script src="//netdna.bootstrapcdn.com/bootstrap/3.0.0/js/bootstrap.min.js"></script>
    <script src="//code.jquery.com/jquery-1.11.1.min.js"></script>



    <script>
        var timeout = '<%= Session.Timeout * 60 * 1000 %>';
        var timer = setInterval(function () {
            timeout -= 1000; document.getElementById('countDown').innerHTML = time(timeout);

            ; if (timeout == 0) { clearInterval(timer); alert('Times up PAL!') }
        }, 1000);


        function two(x) { return ((x > 9) ? "" : "0") + x }


        function time(ms) {
            var t = '';
            var sec = Math.floor(ms / 1000);
            ms = ms % 1000


            var min = Math.floor(sec / 60);
            sec = sec % 60;
            t = two(sec);


            var hr = Math.floor(min / 60);
            min = min % 60;
            t = two(min) + ":" + t;


            return "Your session will timeout in " + t + " minutes.";
        }
    </script>






</body>
</html>
