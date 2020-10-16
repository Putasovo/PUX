<%@ Page Title="Popis" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="FileChecker.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <h3>Sledování změn souborů v adresářích</h3>
    <p>Při prvním spuštění musíte vybrat adresář. Poté můžete spustit analýzu vedlejším tlačítkem.<br />
        Soubory ve vybraném adresáři nalezené během prvního běhu budou zobrazeny jako nové.<br />
        Při každé další analýze stejného adresáře budou zobrazeny už pouze změny.<br />
    </p>
    <p>Jakmile vyberete jiný adresář, začne proces znovu stejným průběhem jako při prvním výběru adresáře.<br />
        Informace o předchozím adresáři budou ztraceny.<br />
    </p>
</asp:Content>
