<%@ Control Language="C#" AutoEventWireup="True" Inherits="avt.Redirect.Manage" EnableViewState = "true" CodeBehind="Manage.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<div>
    <div class = "avtRedirectBox" style = "padding-top: 10px;">
        <p><span style="font-size: 18px;"><strong>Why is DNN Redirect <span style="font-size: 18px; color: #ff0000;">FREE</span>?</strong></span></p>
        <p style="font-size: 14px;">
            DNN Redirect was developed in 2009. It was a success and people started asking for other redirect types.
            This was a challange on current architecture, and we've decided to start a new module called Redirect Toolkit, which comes with tons of new redirect types plus configurable additional actions to execute.
        </p>
        <p style="font-size: 14px;">
            And instead of killing DNN Redirect, we're giving it to the Open Source community, because it still has some use in it, especially for simple user or role based redirects.
            If you need more, make sure to:
        </p>
        <p style="float: left; margin-left: 50px; margin-top: -8px;">
             <a href="http://www.dnnsharp.com/dotnetnuke/modules/dnn-redirect/redirect-toolkit"><img border="0" src="http://static.dnnsharp.com/logo/dnn-modules/redirect-toolkit-100t.png" title="Redirect Toolkit" /></a>
        </p>
        <ol style="float: left; margin: 6px 0 0 20px;">
                <li style="margin: 4px;"><a style="font-weight: normal; font-size: 12px; text-decoration: none;" href="http://www.dnnsharp.com/upgrade-benefits/dnn-redirect">read what <strong style="color: rgb(226, 48, 13); ">Redirect Toolkit</strong> brings to the table</a></li>
                <li style="margin: 4px;"><a style="font-weight: normal; font-size: 12px; text-decoration: none;" href="http://www.dnnsharp.com/dotnetnuke/modules/dnn-redirect/redirect-toolkit/download"><strong style="color: rgb(226, 48, 13); ">download</strong> Redirect Toolkit free trial</a></li>
                <li style="margin: 4px;"><a style="font-weight: normal; font-size: 12px; text-decoration: none;" href="http://twitter.com/dnnsharp">follow us on <strong style="color: rgb(226, 48, 13); ">twitter</strong> to stay connected</a></li>
            </ol>
        <div style="clear: both;"></div>
    </div>


    <a style = "float: right; background-image: url(<%=TemplateSourceDirectory%>/res/icon_help.png); background-repeat: no-repeat; background-position: center top; display: block; padding-top: 20px; margin: 4px 10px 10px 0; font-size: 11px;" href = "https://github.com/bogdan-litescu/DnnRedirect">Help</a>

    <div class = "avtRedirectBox" style = "background-color: #fafafa;">
        <div style = "border-bottom: 1px solid #929292; font-size: 14px; font-weight: bold; margin: 16px 0 2px 0; padding-left: 4px; ">Redirection Defaults</div>
        <div class = "grayedDesc">These settings apply when none of the rules (based on roles or referrers) match.</div>
        <div>
            If no rule matches, redirect to value of parameter 
            <asp:TextBox runat = "server" ID = "txtGetParam" style = "width: 100px;" ValidationGroup = "avtValidRedirectSettings"></asp:TextBox> from GET
        </div>
        
        <div>
            If no rule matches, redirect to value of parameter 
            <asp:TextBox runat = "server" ID = "txtGetParamRefferer" style = "width: 100px;" ValidationGroup = "avtValidRedirectSettings"></asp:TextBox> from GET of the refferer (previous page)
        </div>
        
        <div>
            If no rule matches (including parameter above), redirect to
            <asp:TextBox ID="txtDefaultUrl" runat = "server"  style = "width: 220px;" ValidationGroup = "avtValidRedirectSettings"></asp:TextBox>
        </div>
        
        <div>
            <asp:CheckBox ID="cbLogout" runat = "server"  Text = "Also Logout user before redirect" ValidationGroup = "avtValidRedirectSettings" />
        </div>
        
        <div style = "text-align: center; margin-top: 12px;">
            <asp:LinkButton id="cmdSaveSettings" runat="server"  CausesValidation="True" Text = "Save Settings" OnClick = "OnSaveSettings" ValidationGroup = "avtValidRedirectSettings" style = "display: inline-block; font-weight: bold; padding: 8px; margin-left: 4px; border: 1px solid #929292; background-color: #efefef;" />
        </div>
    </div>


    <%-- Paremeter Redirections --%>
    
    <div class = "avtRedirectBox" style = "background-color: #f0fff0;">
        <div style = "border-bottom: 1px solid #929292; font-size: 14px; font-weight: bold; margin: 16px 0 2px 0; padding-left: 4px; ">PARAMETER Redirections</div>
        <div class = "grayedDesc">This section allows adding new redirects based on parameters from GET or POST.</div>
        <div style = "padding-left: 10px;">
            <div style = "float: left;">
                <b>IF</b> 
                <asp:DropDownList runat = "server" id = "ddParamType">
                    <asp:ListItem Value = "GET_POST">GET or POST</asp:ListItem>
                    <asp:ListItem Value = "GET">GET</asp:ListItem>
                    <asp:ListItem Value = "POST">POST</asp:ListItem>
                </asp:DropDownList> 
                PARAMETER NAMED <asp:TextBox runat = "server" ID = "tbParamRed_Name" style = "width: 100px"></asp:TextBox>

                <asp:DropDownList runat = "server" id = "ddParamOp">
                    <asp:ListItem Value = "Equals">Equals</asp:ListItem>
                    <asp:ListItem Value = "NotEquals">Doesn't Equal</asp:ListItem>
                    <asp:ListItem Value = "Contains">Contains</asp:ListItem>
                    <asp:ListItem Value = "NotContains">Doesn't Contain</asp:ListItem>
                    <asp:ListItem Value = "Exists">Exists</asp:ListItem>
                    <asp:ListItem Value = "NotExists">Doesn't Exist</asp:ListItem>
                </asp:DropDownList> 
                
                VALUE
                <asp:TextBox runat = "server" ID = "tbParamRed_Value" style = "width: 200px;"></asp:TextBox>
                <br />
                <div runat = "server" id = "lblMyTokensParamVal" style = "color: #626262; font-size: 11px; margin-left: 50px; font-weight: normal; font-style: italic;"></div>
                <br />
                <b>THEN</b>

                <div style = "margin-left: 10px;">
                    Redirect to URL <asp:TextBox runat = "server" ID = "tbParamRed_Url"  style = "width: 240px; border: 1px solid #929292;" ValidationGroup = "avtValidRedirectParam"></asp:TextBox>
                    <div runat = "server" id = "lblMyTokensParam" style = "color: #626262; font-size: 11px; margin-left: 50px; font-weight: normal; font-style: italic;"></div>
                    or <asp:CheckBox runat = "server" id = "cbParamRed_KeepOnPage" Text = "Keep on current page" /><br /><br />
                    <asp:CheckBox ID="cbParamRed_Logout" runat = "server"  Text = "Also Logout user before redirect" ValidationGroup = "avtValidRedirectParam" />
                </div>
            </div>
            <div style = "float: left; height: 30px; margin-left: 20px; margin-top: 30px; ">
                <asp:LinkButton runat="server" CausesValidation="True" Text = "Add/Update" ID = "cmdSaveParamAddEditUrl" OnClick = "OnSaveParamAddEditUrl" ValidationGroup = "avtValidRedirectParam" style = "display: inline-block; font-weight: bold; padding: 8px; margin-left: 4px; border: 1px solid #929292; background-color: #efefef;" />
            </div>
            <div style = "clear: both;"></div>
            <br />
            <asp:RequiredFieldValidator ID = "reqUrlParam" runat = "server" ControlToValidate = "tbParamRed_Url" Text = "Please provide the URL for the page to be redirect!" Display = "Dynamic" ValidationGroup = "avtValidRedirectParam"></asp:RequiredFieldValidator>
            <div id = "Div3" runat = "server" Visible = "False" style = "padding: 3px;"></div>
            <div style = "color: #828282; font-size: 11px;">
                Make sure the URL does not point to another redirection (like for example this page), that would cause infinite loop.
            </div>
        </div>

        <div class = "">
            <div style = "border-bottom: 1px solid #929292; font-size: 12px; font-weight: bold; margin: 16px 0 2px 0; padding-left: 4px;">Manage Parameter Redirections</div>
            <div class = "grayedDesc">This section lists all redirects based on parameters.</div>
        
            <asp:gridview id="vwRedirectsParam" 
                AutoGenerateColumns = "false"
                datasourceid="sqlDataSourceParam"
                emptydatatext="There are no redirects by parameters." 
                allowpaging="True" 
                AllowSorting="True"
                runat="server"
                Width="100%" 
                DataKeyNames="" 
                HeaderStyle-CssClass = "grid_vw_header"
                RowStyle-CssClass = "grid_vw_row"
                AlternatingRowStyle-CssClass= "grid_vw_row_alt"
                PageSize = "20"
                PagerStyle-CssClass = "grid_vw_pager"
                BorderColor = "#DDDDDD"  OnRowCommand = "OnRowCmdParams">

                <Columns>
                    <asp:TemplateField HeaderText = "">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton2" runat = "server" CommandName = "del" OnClientClick = "return confirm('Are you sure you want to remove this redirect?');" CommandArgument = '<%# DataBinder.Eval(Container.DataItem, "RedirectParamID")%>' Text = "del"></asp:LinkButton>
                    </ItemTemplate>
                    </asp:TemplateField>
                
                    <asp:BoundField DataField = "RedirectParamId" SortExpression = "RedirectParamId" HeaderText = "id" ItemStyle-Width = "30px" ItemStyle-HorizontalAlign = "Right" ItemStyle-Font-Italic = "true" />
                    <asp:TemplateField SortExpression = "Param" HeaderText = "Rule">
                    <ItemTemplate>
                        [<%# ((avt.Redirect.RedirectInfoParam.eParamType)DataBinder.Eval(Container.DataItem, "ParamType")).ToString()%>]
                        <b><%# DataBinder.Eval(Container.DataItem, "Param") %></b>
                        <i><%# ((avt.Redirect.RedirectInfoParam.eOperation)DataBinder.Eval(Container.DataItem, "Operation")).ToString()%></i>
                        <b><%# DataBinder.Eval(Container.DataItem, "Value") %></b>
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression = "LogoutUser" HeaderText = "Logout?">
                    <ItemTemplate>
                        <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "LogoutUser")) ? "true" : "false" %>
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression = "RedirectUrl" HeaderText = "RedirectUrl">
                    <ItemTemplate>
                        <a href = "<%# string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "RedirectUrl").ToString()) ? "" : Tokenize(DataBinder.Eval(Container.DataItem, "RedirectUrl").ToString()) %>"><%# string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "RedirectUrl").ToString()) ? "keep on page" : Tokenize(DataBinder.Eval(Container.DataItem, "RedirectUrl").ToString()) %></a>
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression = "RedirectPriority" HeaderText = "Priority">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "RedirectPriority")%>
                        <asp:LinkButton runat = "server" ID = "cmdPriorityUp" CommandName = "priority_up" CommandArgument = '<%# DataBinder.Eval(Container.DataItem, "RedirectParamId")%>'>
                            <img src = "<%=TemplateSourceDirectory %>/res/up.gif" border = "0" />
                        </asp:LinkButton>
                        <asp:LinkButton runat = "server" ID = "cmdPriorityDown" CommandName = "priority_down" CommandArgument = '<%# DataBinder.Eval(Container.DataItem, "RedirectParamId")%>'>
                            <img src = "<%=TemplateSourceDirectory %>/res/down.gif" border = "0" />
                        </asp:LinkButton>
                    </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:gridview>

            <asp:SqlDataSource ID="sqlDataSourceParam" runat="server" ConnectionString="<%$ ConnectionStrings:SiteSqlServer %>" SelectCommandType="StoredProcedure" ></asp:SqlDataSource>
        </div>

    </div>

    <div style = "clear: both"></div>

    <%-- Referrer Redirections --%>
    
    <div class = "avtRedirectBox" style = "background-color: #fafafa;">
        <div style = "border-bottom: 1px solid #929292; font-size: 14px; font-weight: bold; margin: 16px 0 2px 0; padding-left: 4px; ">REFERRER Redirections</div>
        <div class = "grayedDesc">This section allows adding new redirects based on referrer or edit existing ones.</div>
        <div style = "padding-left: 10px;">
            <div id = "cReferrer" runat = "server" style = "margin-bottom: 6px;"></div>
            <div style = "float: left;">
                Redirect users referred by 
                    <asp:TextBox runat = "server" id = "tbReferrer" style = "width: 240px; border: 1px solid #929292;"></asp:TextBox>
                    <div class = "grayedDesc" style = "margin: 0;">(add blank referrer to handle when no referrer is passed)</div>
                    <div runat = "server" id = "lblMyTokensRefUrl" style = "color: #626262; font-size: 11px; margin-left: 50px; font-weight: normal; font-style: italic;"></div>
                    <div style = "margin-left: 170px">
                        <asp:CheckBox runat = "server" ID = "cbUrlRefMathDomain" Text = "Match domain only" />
                    </div>
                <br/>
                <div style = "margin-left: 10px;">
                    to URL <asp:TextBox runat = "server" ID = "txtUrlRef"  style = "width: 240px; border: 1px solid #929292;" ValidationGroup = "avtValidRedirectRef"></asp:TextBox>
                    <div runat = "server" id = "lblMyTokensRef" style = "color: #626262; font-size: 11px; margin-left: 50px; font-weight: normal; font-style: italic;"></div>
                    or <asp:CheckBox runat = "server" id = "cbKeepOnPageRef" Text = "Keep on current page" />
                </div>
            </div>
            <div style = "float: left; height: 30px; margin-left: 20px;">
                <asp:LinkButton id="cmdUpdateRef" runat="server"  CausesValidation="True" Text = "Add/Update" OnClick = "OnSaveRefAddEditUrl" ValidationGroup = "avtValidRedirectRef" style = "display: inline-block; font-weight: bold; padding: 8px; margin-left: 4px; margin-top: 30px; border: 1px solid #929292; background-color: #efefef;" />
            </div>
            <div style = "clear: both;"></div>
            <br />
            <asp:RequiredFieldValidator ID="reqUrlRef" runat = "server" ControlToValidate = "txtUrlRef" Text = "Please provide the URL for the page to be redirect!" Display = "Dynamic" ValidationGroup = "avtValidRedirectRef"></asp:RequiredFieldValidator>
            <div id = "lblMsgRef" runat = "server" Visible = "False" style = "padding: 3px;"></div>
            <div style = "color: #828282; font-size: 11px; clear: both;">
                Make sure the URL does not point to another redirection (like for example this page), that would cause infinite loop.
            </div>
        </div>

        <div class = "">
            <div style = "border-bottom: 1px solid #929292; font-size: 12px; font-weight: bold; margin: 16px 0 2px 0; padding-left: 4px;">Manage Referrer Redirections</div>
            <div class = "grayedDesc">This section lists all redirects based on referrers.</div>
        
            <asp:gridview id="vwRedirectsRef" 
                AutoGenerateColumns = "false"
                datasourceid="sqlDataSourceRef"
                emptydatatext="There are no redirects by referrer." 
                allowpaging="True" 
                AllowSorting="True"
                runat="server"
                Width="100%" 
                DataKeyNames="" 
                HeaderStyle-CssClass = "grid_vw_header"
                RowStyle-CssClass = "grid_vw_row"
                AlternatingRowStyle-CssClass= "grid_vw_row_alt"
                PageSize = "20"
                PagerStyle-CssClass = "grid_vw_pager"
                BorderColor = "#DDDDDD"  OnRowCommand = "OnRowCmdRef">

                <Columns>
                    <asp:TemplateField HeaderText = "">
                    <ItemTemplate>
                        <asp:LinkButton runat = "server" CommandName = "del" OnClientClick = "return confirm('Are you sure you want to remove this redirect?');" CommandArgument = '<%# DataBinder.Eval(Container.DataItem, "RedirectRefID")%>' Text = "del"></asp:LinkButton>
                    
                    </ItemTemplate>
                    </asp:TemplateField>
                
                    <asp:BoundField DataField = "RedirectRefId" SortExpression = "RedirectRefId" HeaderText = "id" ItemStyle-Width = "30px" ItemStyle-HorizontalAlign = "Right" ItemStyle-Font-Italic = "true" />
                    <asp:BoundField DataField = "Referrer" SortExpression = "Referrer" HeaderText = "Referrer" ItemStyle-Width = "200px" ItemStyle-HorizontalAlign = "Left" ItemStyle-Font-Italic = "true" />
                    <asp:BoundField DataField = "MatchDomainOnly" SortExpression = "MatchDomainOnly" HeaderText = "MatchDomainOnly" />
                    <asp:TemplateField SortExpression = "RedirectUrl" HeaderText = "RedirectUrl">
                    <ItemTemplate>
                        <a href = "<%# string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "RedirectUrl").ToString()) ? "" : Tokenize(DataBinder.Eval(Container.DataItem, "RedirectUrl").ToString()) %>"><%# string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "RedirectUrl").ToString()) ? "keep on page" : Tokenize(DataBinder.Eval(Container.DataItem, "RedirectUrl").ToString()) %></a>
                    </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:gridview>

            <asp:SqlDataSource ID="sqlDataSourceRef" runat="server" ConnectionString="<%$ ConnectionStrings:SiteSqlServer %>" SelectCommandType="StoredProcedure" ></asp:SqlDataSource>
        </div>

    </div>

    <div style = "clear: both"></div>

    <%-- Role Redirections --%>

    <div class = "avtRedirectBox" style = "background-color: #f0fff0;">
        <div style = "border-bottom: 1px solid #929292; font-size: 14px; font-weight: bold; margin: 16px 0 2px 0; padding-left: 4px; ">ROLE Redirections</div>
        <div class = "grayedDesc">This section allows adding new redirects based on roles or edit existing ones.</div>
        <div style = "padding-left: 10px;">
            <div style = "float: left;">
                Redirect users in role 
                    <asp:DropDownList runat = "server" id = "ddRoles"></asp:DropDownList> 
                <br/>
                <div style = "margin-left: 10px;">
                    to URL <asp:TextBox runat = "server" ID = "txtUrl"  style = "width: 240px; border: 1px solid #929292;" ValidationGroup = "avtValidRedirect"></asp:TextBox>
                    <div runat = "server" id = "lblMyTokens" style = "color: #626262; font-size: 11px; margin-left: 50px; font-weight: normal; font-style: italic;"></div>
                    or <asp:CheckBox runat = "server" id = "cbKeepOnPage" Text = "Keep on current page" /><br /><br />
                    <asp:CheckBox ID="cbByRoleLogout" runat = "server"  Text = "Also Logout user before redirect" ValidationGroup = "avtValidRedirect" />
                </div>
            </div>
            <div style = "float: left; height: 30px; margin-left: 20px;">
                <asp:LinkButton id="cmdUpdate" runat="server"  CausesValidation="True" Text = "Add/Update" OnClick = "OnSaveAddEditUrl" ValidationGroup = "avtValidRedirect" style = "display: inline-block; font-weight: bold; padding: 8px; margin-left: 4px; border: 1px solid #929292; background-color: #efefef;" />
            </div>
            <div style = "clear: both;"></div>
            <br />
            <asp:RequiredFieldValidator ID="reqUrl" runat = "server" ControlToValidate = "txtUrl" Text = "Please provide the URL for the page to be redirect!" Display = "Dynamic" ValidationGroup = "avtValidRedirect"></asp:RequiredFieldValidator>
            <div id = "lblMsg" runat = "server" Visible = "False" style = "padding: 3px;"></div>
            <div style = "color: #828282; font-size: 11px; clear: both;">
                Make sure the URL does not point to another redirection (like for example this page), that would cause infinite loop.
            </div>
        </div>

        <div class = "">
            <div style = "border-bottom: 1px solid #929292; font-size: 12px; font-weight: bold; margin: 16px 0 2px 0; padding-left: 4px;">Manage Role Redirections</div>
            <div class = "grayedDesc">This section lists all redirects based on roles.</div>

            <asp:gridview id="vwRedirects" 
                AutoGenerateColumns = "false"
                datasourceid="sqlDataSource"
                emptydatatext="There are no redirects." 
                allowpaging="True" 
                AllowSorting="True"
                runat="server"
                Width="100%" 
                DataKeyNames="" 
                HeaderStyle-CssClass = "grid_vw_header"
                RowStyle-CssClass = "grid_vw_row"
                AlternatingRowStyle-CssClass= "grid_vw_row_alt"
                PageSize = "20"
                PagerStyle-CssClass = "grid_vw_pager"
                BorderColor = "#DDDDDD"  OnRowCommand = "OnRowCmd">

                <Columns>
                    <asp:TemplateField HeaderText = "">
                    <ItemTemplate>
                        <asp:LinkButton runat = "server" CommandName = "del" OnClientClick = "return confirm('Are you sure you want to remove this redirect?');" CommandArgument = '<%# DataBinder.Eval(Container.DataItem, "RedirectID")%>' Text = "del"></asp:LinkButton>
                    </ItemTemplate>
                    </asp:TemplateField>
                
                    <asp:BoundField DataField = "RedirectId" SortExpression = "RedirectId" HeaderText = "id" ItemStyle-Width = "30px" ItemStyle-HorizontalAlign = "Right" ItemStyle-Font-Italic = "true" />
                    <asp:BoundField DataField = "RoleId" SortExpression = "RoleId" HeaderText = "RoleID" ItemStyle-Width = "50px" ItemStyle-HorizontalAlign = "Right" ItemStyle-Font-Italic = "true" />
                    <asp:BoundField DataField = "RoleName" SortExpression = "RoleName" HeaderText = "RoleName" />
                    <asp:TemplateField SortExpression = "LogoutUser" HeaderText = "LogoutUser">
                    <ItemTemplate>
                        <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "LogoutUser")) ? "true" : "false" %>
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression = "RedirectUrl" HeaderText = "RedirectUrl">
                    <ItemTemplate>
                        <a href = "<%# string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "RedirectUrl").ToString()) ? "" : Tokenize(DataBinder.Eval(Container.DataItem, "RedirectUrl").ToString()) %>"><%# string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "RedirectUrl").ToString()) ? "keep on page" : Tokenize(DataBinder.Eval(Container.DataItem, "RedirectUrl").ToString()) %></a>
                    </ItemTemplate>
                    </asp:TemplateField>
                
                    <asp:TemplateField SortExpression = "RedirectPriority" HeaderText = "Priority">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "RedirectPriority")%>
                        <asp:LinkButton runat = "server" ID = "cmdPriorityUp" CommandName = "priority_up" CommandArgument = '<%# DataBinder.Eval(Container.DataItem, "RedirectID")%>'>
                            <img src = "<%=TemplateSourceDirectory %>/res/up.gif" border = "0" />
                        </asp:LinkButton>
                        <asp:LinkButton runat = "server" ID = "cmdPriorityDown" CommandName = "priority_down" CommandArgument = '<%# DataBinder.Eval(Container.DataItem, "RedirectID")%>'>
                            <img src = "<%=TemplateSourceDirectory %>/res/down.gif" border = "0" />
                        </asp:LinkButton>
                    </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:gridview>

            <asp:SqlDataSource ID="sqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SiteSqlServer %>" SelectCommandType="StoredProcedure" ></asp:SqlDataSource>
        </div>

    </div>

    <div style = "clear: both"></div>
    
    <br />
    <a href = "<%= DotNetNuke.Common.Globals.NavigateURL(TabId) %>" style="font-size: 16px; font-weight:bold;">&lt; Back</a>

    <div style="margin-top:40px; padding: 4px 10px; border-top: 1px solid #ccc;">
        Make sure to <a href="http://www.dnnsharp.com/dnn-modules">read about our other modules</a> and <a href ="http://twitter.com/dnnsharp">follow us on twitter</a> 
    </div>

</div>