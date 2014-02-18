<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gallery.Default" ViewStateMode="Disabled" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Galleri</title>
</head>
<body>

    <h1>Galleri</h1>

    <form id="form" runat="server">
    <div>
        <%-- Meddelande vid lyckad uppladdning --%>
        <asp:Panel ID="successPanel" runat="server" CssClass="" Visible="false">

            <asp:Label ID="successLabel" runat="server" Text="Bilden har laddats upp!"></asp:Label>
            <asp:ImageButton ID="closeMessage" runat="server" ImageUrl="Content/delete.jpg" CausesValidation="False" OnClick="closeMessage_Click" />

        </asp:Panel>
        
        <%-- Bild i större format --%>
        <div>
            <asp:Image ID="FullImage" runat="server" ImageUrl="~/Content/Images/delete.jpg" />
        </div>
        
        <%-- Repeater för tumnagelbilderna --%>
        <div>
            <asp:Repeater ID="Repeater" runat="server" ItemType="Gallery.Model.Thumbnail" SelectMethod="Repeater_GetData" OnItemDataBound="Repeater_ItemDataBound" >
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink" runat="server" NavigateUrl='<%# Item.ThumbNavUrl %>' CssClass="">
                        <asp:Image ID="ThumbNail" runat="server" ImageUrl='<%# Item.ThumbNailUrl %>' />
                    </asp:HyperLink>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div>
            <%-- Kontroll för uppladdning av fil med validering --%>
            <asp:FileUpload ID="FileUpload" runat="server" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="En fil måste väljas" ControlToValidate="FileUpload" Display="None"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Endast bilder av typerna gif, jpg eller png är tillåtna" ControlToValidate="FileUpload" Display="None" ValidationExpression=".*.(gif|GIF|jpg|JPG|png|PNG)" ></asp:RegularExpressionValidator>

            <%-- Knapp för uppladdning --%>
            <asp:Button ID="Button" runat="server" Text="Button" OnClick="Button_Click" />

            <%-- Validationsummary --%>
            <asp:validationsummary ID="Validationsummary1" runat="server" HeaderText="Fel inträffade! Korrigera felet och försök igen."></asp:validationsummary>
        </div>
    </div>
    </form>
</body>
</html>

