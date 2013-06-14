using DotNetNuke.Entities.Modules;
using DotNetNuke.Security.Roles;
using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace avt.Redirect
{
    public partial class Manage : PortalModuleBase
    {
        private DotNetNuke.Framework.Providers.ProviderConfiguration _providerConfiguration = DotNetNuke.Framework.Providers.ProviderConfiguration.GetProviderConfiguration("data");
        private string _connectionString;
        private string _providerPath;
        private string _objectQualifier;
        private string _databaseOwner;


        /////////////////////////////////////////////////////////////////////////////////
        // EVENT HANDLERS

        protected void Page_Init(object sender, EventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false) {
                BindData();
            }
        }


        private void BindData()
        {
            if (!TokenReplacer.IsMyTokensInstalled()) {
                lblMyTokensRefUrl.InnerHtml = lblMyTokensParamVal.InnerHtml = lblMyTokensParam.InnerHtml = lblMyTokensRef.InnerHtml = lblMyTokens.InnerHtml = "can contain MyTokens (get it <a href = 'http://www.avatar-soft.ro/Products/MyTokens/tabid/148/Default.aspx'>here</a>)";
            } else {
                lblMyTokensRefUrl.InnerHtml = lblMyTokensParam.InnerHtml = lblMyTokensRef.InnerHtml = lblMyTokens.InnerHtml = "can contain MyTokens (installed)";
                lblMyTokensParamVal.InnerHtml = lblMyTokens.InnerHtml = "both parameter name and value can contain MyTokens (installed)";
            }

            // clear form
            txtUrl.Text = "";
            txtUrlRef.Text = "";
            cbKeepOnPage.Checked = false;
            cbKeepOnPageRef.Checked = false;
            cbByRoleLogout.Checked = false;
            reqUrl.IsValid = true;

            tbReferrer.Text = "";
            txtUrlRef.Text = "";
            reqUrlRef.IsValid = true;
            cbUrlRefMathDomain.Checked = false;
            cbKeepOnPageRef.Checked = false;

            cbParamRed_Logout.Checked = false;
            cbParamRed_KeepOnPage.Checked = false;
            tbParamRed_Url.Text = "";
            tbParamRed_Name.Text = "";
            tbParamRed_Value.Text = "";
            reqUrlParam.IsValid = true;
            ddParamOp.ClearSelection();
            ddParamOp.SelectedIndex = 0;
            ddParamType.ClearSelection();
            ddParamType.SelectedIndex = 0;

            // bind settings
            ModuleController modCtrl = new ModuleController();
            try {
                txtGetParam.Text = modCtrl.GetModuleSettings(ModuleId)["GetParam"].ToString();
            } catch {
                txtGetParam.Text = "";
            }
            try {
                txtGetParamRefferer.Text = modCtrl.GetModuleSettings(ModuleId)["GetParamRef"].ToString();
            } catch {
                txtGetParamRefferer.Text = "";
            }
            try {
                txtDefaultUrl.Text = modCtrl.GetModuleSettings(ModuleId)["DefaultUrl"].ToString();
            } catch {
                txtDefaultUrl.Text = "";
            }
            try {
                cbLogout.Checked = Convert.ToBoolean(modCtrl.GetModuleSettings(ModuleId)["LogoutUser"].ToString());
            } catch {
                cbLogout.Checked = false;
            }


            // bind roles DD
            ddRoles.ClearSelection();
            ddRoles.Items.Clear();

            RoleController roleCtrl = new RoleController();
            ArrayList roles = roleCtrl.GetPortalRoles(PortalId);

            // remove admin role
            foreach (RoleInfo rInfo in roles) {
                if (rInfo.RoleID == PortalSettings.AdministratorRoleId) {
                    roles.Remove(rInfo);
                    break;
                }
            }

            // now, add All Users and Unregistered Users
            roles.Insert(0, new RoleInfo() { RoleID = 0, RoleName = "Unregistered Users" });
            roles.Insert(0, new RoleInfo() { RoleID = -1, RoleName = "All Users" });

            ddRoles.DataTextField = "RoleName";
            ddRoles.DataValueField = "RoleID";
            ddRoles.DataSource = roles;
            ddRoles.DataBind();

            // bind redirects table
            GetDbConfig();
            sqlDataSource.SelectCommand = _databaseOwner + _objectQualifier + "avtRedirect_GetRedirects";
            sqlDataSource.SelectParameters.Clear();
            sqlDataSource.SelectParameters.Add(new Parameter() { Name = "portalId", DefaultValue = PortalId.ToString() });
            sqlDataSource.SelectParameters.Add(new Parameter() { Name = "moduleId", DefaultValue = ModuleId.ToString() });
            sqlDataSource.DataBind();
            vwRedirects.DataBind();

            // bind redirects ref table
            sqlDataSourceRef.SelectCommand = _databaseOwner + _objectQualifier + "avtRedirect_GetRedirectsRef";
            sqlDataSourceRef.SelectParameters.Clear();
            sqlDataSourceRef.SelectParameters.Add(new Parameter() { Name = "moduleId", DefaultValue = ModuleId.ToString() });
            sqlDataSourceRef.DataBind();
            vwRedirectsRef.DataBind();

            // bind redirects param table
            sqlDataSourceParam.SelectCommand = _databaseOwner + _objectQualifier + "avtRedirect_GetRedirectsParam";
            sqlDataSourceParam.SelectParameters.Clear();
            sqlDataSourceParam.SelectParameters.Add(new Parameter() { Name = "moduleId", DefaultValue = ModuleId.ToString() });
            sqlDataSourceParam.DataBind();
            vwRedirectsParam.DataBind();


            RedirectController redirCtrl = new RedirectController();

            cbKeepOnPage.Attributes["onclick"] = "ValidatorEnable(document.getElementById('" + reqUrl.ClientID + "'), !this.checked);";
            cbKeepOnPageRef.Attributes["onclick"] = "ValidatorEnable(document.getElementById('" + reqUrlRef.ClientID + "'), !this.checked);";
            cbParamRed_KeepOnPage.Attributes["onclick"] = "ValidatorEnable(document.getElementById('" + reqUrlParam.ClientID + "'), !this.checked);";
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
                cReferrer.InnerHtml = "Current Referrer: " + Request.UrlReferrer.ToString();
            else
                cReferrer.InnerHtml = "Current Referrer: <i>none</i>";


            // set validation groups
            txtGetParam.ValidationGroup = ModuleId.ToString() + "_avtValidRedirectSettings";
            txtGetParamRefferer.ValidationGroup = ModuleId.ToString() + "_avtValidRedirectSettings";
            txtDefaultUrl.ValidationGroup = ModuleId.ToString() + "_avtValidRedirectSettings";
            cbLogout.ValidationGroup = ModuleId.ToString() + "_avtValidRedirectSettings";
            cmdSaveSettings.ValidationGroup = ModuleId.ToString() + "_avtValidRedirectSettings";

            tbParamRed_Url.ValidationGroup = ModuleId.ToString() + "avtValidRedirectParam";
            cbParamRed_Logout.ValidationGroup = ModuleId.ToString() + "avtValidRedirectParam";
            cmdSaveParamAddEditUrl.ValidationGroup = ModuleId.ToString() + "avtValidRedirectParam";
            reqUrlParam.ValidationGroup = ModuleId.ToString() + "avtValidRedirectParam";

            txtUrlRef.ValidationGroup = ModuleId.ToString() + "avtValidRedirectRef";
            cmdUpdateRef.ValidationGroup = ModuleId.ToString() + "avtValidRedirectRef";
            reqUrlRef.ValidationGroup = ModuleId.ToString() + "avtValidRedirectRef";

            txtUrl.ValidationGroup = ModuleId.ToString() + "avtValidRedirect";
            cbByRoleLogout.ValidationGroup = ModuleId.ToString() + "avtValidRedirect";
            cmdUpdate.ValidationGroup = ModuleId.ToString() + "avtValidRedirect";
            reqUrl.ValidationGroup = ModuleId.ToString() + "avtValidRedirect";
        }

        protected void OnRowCmd(Object Sender, GridViewCommandEventArgs args)
        {

            RedirectController redirCtrl = new RedirectController();
            switch (args.CommandName) {
                case "del":
                    redirCtrl.RemoveRedirect(Convert.ToInt32(args.CommandArgument));
                    break;
                case "priority_up":
                    redirCtrl.IncreasePriority(Convert.ToInt32(args.CommandArgument));
                    break;
                case "priority_down":
                    redirCtrl.DecreasePriority(Convert.ToInt32(args.CommandArgument));
                    break;
            }

            BindData();
        }

        protected void OnRowCmdRef(Object Sender, GridViewCommandEventArgs args)
        {

            RedirectController redirCtrl = new RedirectController();
            switch (args.CommandName) {
                case "del":
                    redirCtrl.RemoveRedirectRef(Convert.ToInt32(args.CommandArgument));
                    break;
            }

            BindData();
        }

        protected void OnRowCmdParams(Object Sender, GridViewCommandEventArgs args)
        {
            RedirectController redirCtrl = new RedirectController();
            switch (args.CommandName) {
                case "del":
                    redirCtrl.RemoveRedirectParam(Convert.ToInt32(args.CommandArgument));
                    break;
                case "priority_up":
                    RedirectInfoParam rinc = redirCtrl.GetRedirectParamById(Convert.ToInt32(args.CommandArgument));
                    rinc.RedirectPriority++;
                    rinc.Save();
                    break;
                case "priority_down":
                    RedirectInfoParam rdec = redirCtrl.GetRedirectParamById(Convert.ToInt32(args.CommandArgument));
                    rdec.RedirectPriority--;
                    rdec.Save();
                    break;
            }

            BindData();
        }

        protected void OnSaveAddEditUrl(Object Sender, EventArgs args)
        {
            //try {

            RedirectController rc = new RedirectController();
            int roleId = Convert.ToInt32(ddRoles.SelectedValue);
            bool bAllUsers = false;
            bool bUnregisteredUsers = false;

            if (roleId == 0) {
                bUnregisteredUsers = true;
            } else if (roleId == -1) {
                bAllUsers = true;
            }

            rc.UpdateRedirect(roleId, bAllUsers, bUnregisteredUsers, cbKeepOnPageRef.Checked ? null : txtUrl.Text.Trim(), cbByRoleLogout.Checked, ModuleId);
            BindData();

            //} catch (Exception) {
            //    lblMsg.Visible = true;
            //    lblMsg.InnerHtml = "Error saving redirect!";
            //    lblMsg.Style["background-color"] = "#FF9999";
            //    Page.ClientScript.RegisterStartupScript(GetType(), "timeoutSuccessMsg", "<script type='text/javascript'>setTimeout(function() { document.getElementById('" + lblMsg.ClientID + "').style.display = 'none'; }, 6000);</script>");
            //}
        }


        protected void OnSaveRefAddEditUrl(Object Sender, EventArgs args)
        {
            //try {

            RedirectController rc = new RedirectController();
            rc.UpdateRedirectRef(tbReferrer.Text.Trim(), cbKeepOnPageRef.Checked ? null : txtUrlRef.Text.Trim(), cbUrlRefMathDomain.Checked, ModuleId);
            BindData();

            //} catch (Exception) {
            //    lblMsg.Visible = true;
            //    lblMsg.InnerHtml = "Error saving redirect!";
            //    lblMsg.Style["background-color"] = "#FF9999";
            //    Page.ClientScript.RegisterStartupScript(GetType(), "timeoutSuccessMsg", "<script type='text/javascript'>setTimeout(function() { document.getElementById('" + lblMsg.ClientID + "').style.display = 'none'; }, 6000);</script>");
            //}
        }

        protected void OnSaveParamAddEditUrl(Object Sender, EventArgs args)
        {
            //try {

            RedirectInfoParam rp = new RedirectInfoParam() {
                ModuleId = ModuleId,
                Param = tbParamRed_Name.Text,
                ParamType = (RedirectInfoParam.eParamType)Enum.Parse(typeof(RedirectInfoParam.eParamType), ddParamType.SelectedValue, false),
                Operation = (RedirectInfoParam.eOperation)Enum.Parse(typeof(RedirectInfoParam.eOperation), ddParamOp.SelectedValue, false),
                Value = tbParamRed_Value.Text,
                RedirectUrl = cbParamRed_KeepOnPage.Checked ? null : tbParamRed_Url.Text,
                LogoutUser = cbParamRed_Logout.Checked
            };

            if (rp.Operation == RedirectInfoParam.eOperation.Exists || rp.Operation == RedirectInfoParam.eOperation.NotExists) {
                rp.Value = ""; // unary operation
            }

            rp.Save();
            BindData();

            //} catch (Exception) {
            //    lblMsg.Visible = true;
            //    lblMsg.InnerHtml = "Error saving redirect!";
            //    lblMsg.Style["background-color"] = "#FF9999";
            //    Page.ClientScript.RegisterStartupScript(GetType(), "timeoutSuccessMsg", "<script type='text/javascript'>setTimeout(function() { document.getElementById('" + lblMsg.ClientID + "').style.display = 'none'; }, 6000);</script>");
            //}
        }

        protected void OnSaveSettings(Object Sender, EventArgs args)
        {
            ModuleController modCtrl = new ModuleController();
            modCtrl.UpdateModuleSetting(ModuleId, "GetParam", txtGetParam.Text.Trim());
            modCtrl.UpdateModuleSetting(ModuleId, "GetParamRef", txtGetParamRefferer.Text.Trim());
            modCtrl.UpdateModuleSetting(ModuleId, "DefaultUrl", txtDefaultUrl.Text.Trim());
            modCtrl.UpdateModuleSetting(ModuleId, "LogoutUser", cbLogout.Checked.ToString());
        }

        private void GetDbConfig()
        {
            // Read the configuration specific information for this provider
            DotNetNuke.Framework.Providers.Provider objProvider = (DotNetNuke.Framework.Providers.Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider];

            // Read the attributes for this provider
            //Get Connection string from web.config
            _connectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString();

            if (_connectionString == "") {
                // Use connection string specified in provider
                _connectionString = objProvider.Attributes["connectionString"];
            }

            _providerPath = objProvider.Attributes["providerPath"];

            _objectQualifier = objProvider.Attributes["objectQualifier"];
            if (_objectQualifier != "" & _objectQualifier.EndsWith("_") == false) {
                _objectQualifier += "_";
            }

            _databaseOwner = objProvider.Attributes["databaseOwner"];
            if (_databaseOwner != "" & _databaseOwner.EndsWith(".") == false) {
                _databaseOwner += ".";
            }
        }

        protected string Tokenize(string content)
        {
            return TokenReplacer.Tokenize(content, ModuleConfiguration, UserInfo, false, false);
        }

    }

}
