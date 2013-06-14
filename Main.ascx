<%@ Control Language="C#" AutoEventWireup="true" Inherits="avt.Redirect.Main" EnableViewState = "true" CodeBehind="Main.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<div runat = "server" id = "pnlAdmin" visible = "false">
    Use Module Actions to <a href="<%= EditUrl("Manage") %>">manage redirects</a>...<br />
    <b>Important: </b> Make sure users you want to redirect have view rights for this module.<br /><br />
    <div style = "color: #828282; font-style: italic; font-size: 10px; border-top: 1px solid #cccccc;">
        Text above is only visible for Administrators in Edit Mode...
    </div>
</div>

<div runat = "server" id = "pnlRedirectUser" visible = "false"></div>
