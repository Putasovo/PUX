<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="FileChecker.Main" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Label ID="Label1" runat="server" Text="Ready"></asp:Label>
    <asp:Label ID="Label2" runat="server"></asp:Label>
    <br />
    <asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged">Select Directory</asp:TextBox>
    <asp:Button ID="Button1" runat="server" Text="Analyze" OnClick="Button1_Click" />

    <asp:GridView runat="server" ID="GridDeleted" Caption="Deleted Files"
        ItemType="FileChecker.FileModel" DataKeyNames="FileID" 
        AutoGenerateColumns="false">
        <Columns>
            <asp:DynamicField DataField="Name" />
            <asp:DynamicField DataField="Size" />          
            <asp:DynamicField DataField="WriteTime" />
        </Columns>
    </asp:GridView>

     <asp:GridView runat="server" ID="GridModed"  Caption="Modified Files"
        ItemType="FileChecker.FileModel" DataKeyNames="FileID" 
        AutoGenerateColumns="false">
        <Columns>
            <asp:DynamicField DataField="Version" />
            <asp:DynamicField DataField="Name" />
            <asp:DynamicField DataField="Size" />          
            <asp:DynamicField DataField="WriteTime" />
        </Columns>
    </asp:GridView>

    <asp:GridView runat="server" ID="GridNewFiles" Caption="New Files"
        ItemType="FileChecker.FileModel"
        AutoGenerateColumns="false">
        <Columns>
            <asp:DynamicField DataField="Name" />
            <asp:DynamicField DataField="Size" />          
            <asp:DynamicField DataField="WriteTime" />
        </Columns>
    </asp:GridView>

<%--    Files: <%= currentState.Count %>
    Total Size: <%= TotalSize %>--%>
</asp:Content>
