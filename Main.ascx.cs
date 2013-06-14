using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using DotNetNuke.Security.Permissions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;


namespace avt.Redirect
{
    public partial class Main : PortalModuleBase, IActionable
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
            if (ModuleConfiguration.DisplayTitle) {
                ModuleConfiguration.DisplayTitle = false;
                if (ModuleConfiguration.EndDate == DateTime.MaxValue)
                    ModuleConfiguration.EndDate = Null.NullDate;
                ModuleController modCtrl = new ModuleController();
                modCtrl.UpdateModule(ModuleConfiguration);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // dont't redirect admins, instead show them the module so they can manage redirects
            pnlAdmin.Visible = ModulePermissionController.CanEditModuleContent(ModuleConfiguration);
            if (!pnlAdmin.Visible) {
                Redirect();
            }
        }

        protected string Tokenize(string content)
        {
            return TokenReplacer.Tokenize(content, ModuleConfiguration, UserInfo, false, false);
        }

        protected void Redirect()
        {
            RedirectController redirCtrl = new RedirectController();
            ArrayList redirectsRef = redirCtrl.GetRedirectsRef(ModuleId);
            ArrayList redirects = redirCtrl.GetRedirects(PortalId, ModuleId);
            List<RedirectInfoParam> redirectsParam = redirCtrl.GetRedirectsParam(ModuleId);

            ModuleController modCtrl = new ModuleController();
            Hashtable modSettings = modCtrl.GetModuleSettings(ModuleId);

            string redirUrl = null;

            bool bLogout;
            try {
                bLogout = Convert.ToBoolean(modCtrl.GetModuleSettings(ModuleId)["LogoutUser"].ToString());
            } catch { bLogout = false; }

            // check parameters
            foreach (RedirectInfoParam redInfo in redirectsParam) {

                NameValueCollection collection = null;
                switch (redInfo.ParamType) {
                    case RedirectInfoParam.eParamType.GET:
                        collection = Request.QueryString;
                        break;
                    case RedirectInfoParam.eParamType.POST:
                        collection = Request.Form;
                        break;
                    case RedirectInfoParam.eParamType.GET_POST:
                        collection = Request.Params;
                        break;
                }

                if (collection == null)
                    continue;

                // check if matches

                bool bMatch = false;
                string paramName = Tokenize(redInfo.Param);
                string paramValue =Tokenize(redInfo.Value);

                if (collection[paramName] == null) {
                    if (redInfo.Operation == RedirectInfoParam.eOperation.NotExists) {
                        bMatch = true;
                    } else { continue; }
                } else {
                    if (redInfo.Operation == RedirectInfoParam.eOperation.Exists) {
                        bMatch = true;
                    }
                }

                if (bMatch == false) {
                    switch (redInfo.Operation) {
                        case RedirectInfoParam.eOperation.Equals:
                            if (paramValue == collection[paramName])
                                bMatch = true;
                            break;
                        case RedirectInfoParam.eOperation.NotEquals:
                            if (paramValue != collection[paramName])
                                bMatch = true;
                            break;
                        case RedirectInfoParam.eOperation.Contains:
                            if (collection[paramName].Contains(paramValue))
                                bMatch = true;
                            break;
                        case RedirectInfoParam.eOperation.NotContains:
                            if (!collection[paramName].Contains(paramValue))
                                bMatch = true;
                            break;
                    }
                }

                if (bMatch) {
                    redirUrl = redInfo.RedirectUrl;
                    bLogout = redInfo.LogoutUser;
                    if (string.IsNullOrEmpty(redirUrl)) {
                        CheckLogout(bLogout);
                        return; // keep on page
                    }
                    break;
                }
            }

            // check referrers first
            if (redirUrl == null) {
                string urlReferrer = "";
                if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString())) {
                    urlReferrer = Request.UrlReferrer.ToString();
                }

                foreach (RedirectRefInfo redInfo in redirectsRef) {

                    string checkRef = Tokenize(redInfo.Referrer);

                    if (redInfo.MatchDomainOnly) {
                        if (urlReferrer.IndexOf(checkRef) == 0) {
                            redirUrl = redInfo.RedirectUrl;
                            if (string.IsNullOrEmpty(redirUrl)) {
                                CheckLogout(bLogout);
                                return; // keep on page
                            }
                            break;
                        }
                    } else {
                        if (checkRef == urlReferrer) {
                            redirUrl = redInfo.RedirectUrl;
                            if (string.IsNullOrEmpty(redirUrl)) {
                                CheckLogout(bLogout);
                                return; // keep on page
                            }
                            break;
                        }
                    }
                }
            }

            // check which one matches
            if (redirUrl == null) {
                foreach (RedirectInfo redInfo in redirects) {
                    if (redInfo.RoleId > 0) {
                        if (UserInfo.IsInRole(redInfo.RoleName)) {
                            redirUrl = redInfo.RedirectUrl;
                            bLogout = redInfo.LogoutUser;
                            if (string.IsNullOrEmpty(redirUrl)) {
                                CheckLogout(bLogout);
                                return; // keep on page
                            }
                            break;
                        }
                    } else {
                        if (redInfo.RoleUnregisteredUsers && UserInfo.UserID <= 0) {
                            redirUrl = redInfo.RedirectUrl;
                            bLogout = redInfo.LogoutUser;
                            if (string.IsNullOrEmpty(redirUrl)) {
                                CheckLogout(bLogout);
                                return; // keep on page
                            }
                            break;
                        } else if (redInfo.RoleAllUsers) {
                            redirUrl = redInfo.RedirectUrl;
                            bLogout = redInfo.LogoutUser;
                            if (string.IsNullOrEmpty(redirUrl)) {
                                CheckLogout(bLogout);
                                return; // keep on page
                            }
                            break;
                        }
                    }
                }
            }

            // see if we have get param
            if (redirUrl == null) {
                if (modSettings.ContainsKey("GetParam") && modSettings["GetParam"].ToString().Length > 0) {
                    if (!String.IsNullOrEmpty(Request.QueryString[modSettings["GetParam"].ToString()])) {
                        redirUrl = Request.QueryString[modSettings["GetParam"].ToString()];
                    }
                }
            }

            if (redirUrl == null) {
                if (modSettings.ContainsKey("GetParamRef") && modSettings["GetParamRef"].ToString().Length > 0) {
                    try {
                        Match m = Regex.Match(Request.UrlReferrer.ToString(), modSettings["GetParamRef"].ToString() + "=([^&]+)");
                        if (!String.IsNullOrEmpty(m.Groups[1].Captures[0].Value)) {
                            redirUrl = m.Groups[1].Captures[0].Value;
                        }
                    } catch {
                    }
                }
            }

            // we're up to default url
            if (redirUrl == null) {
                if (modSettings.ContainsKey("DefaultUrl") && modSettings["DefaultUrl"].ToString().Length > 0) {
                    redirUrl = modSettings["DefaultUrl"].ToString();
                }
            }

            if (redirUrl == null) {
                // redirect failed
                //pnlRedirectUser.Visible = true;
                //pnlRedirectUser.InnerHtml = "<b>Redirect Failed!<b/><br/><br/>There is no redirect defined for current role.";
                CheckLogout(bLogout);
                return;
            }

            redirUrl = redirUrl.Trim();

            // apply tokens
            redirUrl = Tokenize(redirUrl);


            CheckLogout(bLogout);

            Response.Redirect(redirUrl);
        }

        private void CheckLogout(bool bLogout)
        {
            if (bLogout && UserInfo != null && UserInfo.UserID > 0) {
                DataCache.ClearUserCache(PortalSettings.PortalId, Context.User.Identity.Name);

                PortalSecurity objPortalSecurity = new PortalSecurity();
                objPortalSecurity.SignOut();
            }
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




        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection Actions = new ModuleActionCollection();
                Actions.Add(GetNextActionID(), "Manage Redirects", DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "icon_sitesettings_16px.gif", EditUrl("Manage"), false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);

                return Actions;
            }
        }
    }

}
