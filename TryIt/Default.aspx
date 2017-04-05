<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TryIt._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Required Services</h1>
        <p class="lead">Ellery Leung</p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Service #1 - Find Nearby Venues</h2>
            <p>
                Input a location (zipcode, city):
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            </p>
            <p>
                Input a venue name (specific, general, ie. &quot;Pizza Hut&quot; vs &quot;pizza&quot;):
                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
            </p>
            <p>
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />
            </p>
            <p>
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Service #2 - Get Foursquare Tips (Reviews) of a venue</h2>
            <p>
                Input a venue name (case-sensitive &amp; specific):
                <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
            </p>
            <p>
                <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Submit" />
            </p>
            <p>
                <asp:Label ID="Label2" runat="server"></asp:Label>
            </p>
        </div>
    </div>

</asp:Content>
