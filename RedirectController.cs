
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;


namespace avt.Redirect
{

    public class RedirectInfo
    {
        private int _RedirectId;
        private int _RoleId;
        private bool _RoleAllUsers;
        private bool _RoleUnregisteredUsers;
        private string _RoleName;
        private string _RedirectUrl;
        private int _RedirectPriority;
        private bool _LogoutUser;

        public int RedirectId
        {
            get { return _RedirectId; }
            set { _RedirectId = value; }
        }

        public int RoleId
        {
            get { return _RoleId; }
            set { _RoleId = value; }
        }

        public bool RoleAllUsers
        {
            get { return _RoleAllUsers; }
            set { _RoleAllUsers = value; }
        }

        public bool RoleUnregisteredUsers
        {
            get { return _RoleUnregisteredUsers; }
            set { _RoleUnregisteredUsers = value; }
        }

        public string RoleName
        {
            get { return _RoleName; }
            set { _RoleName = value; }
        }

        public string RedirectUrl
        {
            get { return _RedirectUrl; }
            set { _RedirectUrl = value; }
        }

        public int RedirectPriority
        {
            get { return _RedirectPriority; }
            set { this._RedirectPriority = value; }
        }

        public bool LogoutUser
        {
            get { return _LogoutUser; }
            set { this._LogoutUser = value; }
        }
    }


    public class RedirectRefInfo
    {
        private int _RedirectRefId;
        private string _Referrer;
        bool _MatchDomainOnly;
        private string _RedirectUrl;

        public int RedirectRefId
        {
            get { return _RedirectRefId; }
            set { _RedirectRefId = value; }
        }

        public string Referrer
        {
            get { return _Referrer; }
            set { _Referrer = value; }
        }

        public bool MatchDomainOnly
        {
            get { return _MatchDomainOnly; }
            set { _MatchDomainOnly = value; }
        }

        public string RedirectUrl
        {
            get { return _RedirectUrl; }
            set { _RedirectUrl = value; }
        }
    }

    public class RedirectInfoParam
    {
        public enum eParamType
        {
            GET_POST,
            GET,
            POST
        }

        public enum eOperation
        {
            Equals,
            NotEquals,
            Contains,
            NotContains,
            Exists,
            NotExists
        }


        int _RedirectParamId;
        int _ModuleId;
        string _Param;
        eParamType _ParamType;
        eOperation _Operation;
        string _Value;
        int _RedirectPriority;
        private bool _LogoutUser;
        private string _RedirectUrl;

        public RedirectInfoParam()
        {
            RedirectPriority = 0;
            _LogoutUser = false;
        }

        public int RedirectParamId
        {
            get { return _RedirectParamId; }
            set { _RedirectParamId = value; }
        }

        public int ModuleId
        {
            get { return _ModuleId; }
            set { _ModuleId = value; }
        }

        public string Param
        {
            get { return _Param; }
            set { _Param = value; }
        }

        public eParamType ParamType
        {
            get { return _ParamType; }
            set { _ParamType = value; }
        }

        public eOperation Operation
        {
            get { return _Operation; }
            set { _Operation = value; }
        }

        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public int RedirectPriority
        {
            get { return _RedirectPriority; }
            set { this._RedirectPriority = value; }
        }

        public bool LogoutUser
        {
            get { return _LogoutUser; }
            set { this._LogoutUser = value; }
        }

        public string RedirectUrl
        {
            get { return _RedirectUrl; }
            set { _RedirectUrl = value; }
        }

        public bool Save()
        {
            RedirectController redirCtrl = new RedirectController();
            redirCtrl.UpdateRedirectParam(ModuleId, Param, ParamType, Operation, Value, RedirectUrl, RedirectPriority, LogoutUser);
            return true;
        }
    }

 
    public class RedirectController
    {
        private string _objectQualifier;
        private string _databaseOwner;

        public RedirectController()
        {
            // Read the configuration specific information for this provider
            DotNetNuke.Framework.Providers.ProviderConfiguration _providerConfiguration = DotNetNuke.Framework.Providers.ProviderConfiguration.GetProviderConfiguration("data");
            DotNetNuke.Framework.Providers.Provider objProvider = (DotNetNuke.Framework.Providers.Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider];

            // Read the attributes for this provider
            //Get Connection string from web.config
            string _connectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString();

            if (_connectionString == "") {
                // Use connection string specified in provider
                _connectionString = objProvider.Attributes["connectionString"];
            }

            string _providerPath = objProvider.Attributes["providerPath"];

            _objectQualifier = objProvider.Attributes["objectQualifier"];
            if (_objectQualifier != "" & _objectQualifier.EndsWith("_") == false) {
                _objectQualifier += "_";
            }

            _databaseOwner = objProvider.Attributes["databaseOwner"];
            if (_databaseOwner != "" & _databaseOwner.EndsWith(".") == false) {
                _databaseOwner += ".";
            }
        }

        public ArrayList GetRedirects(int portalId, int moduleId)
        {
            return DotNetNuke.Common.Utilities.CBO.FillCollection(DataProvider.Instance().GetRedirects(portalId, moduleId), typeof(RedirectInfo));
        }

        public ArrayList GetRedirectsRef(int moduleId)
        {
            return DotNetNuke.Common.Utilities.CBO.FillCollection(DataProvider.Instance().GetRedirectsRef(moduleId), typeof(RedirectRefInfo));
        }

        public RedirectInfoParam GetRedirectParamById(int redirectParamId)
        {
            return DotNetNuke.Common.Utilities.CBO.FillObject<RedirectInfoParam>(DataProvider.Instance().GetRedirectParamById(redirectParamId));
        }

        public List<RedirectInfoParam> GetRedirectsParam(int moduleId)
        {
            return DotNetNuke.Common.Utilities.CBO.FillCollection<RedirectInfoParam>(DataProvider.Instance().GetRedirectsParam(moduleId));
        }

        public void UpdateRedirect(int roleId, bool allUsers, bool unregisteredUsers, string url, bool logoutUser, int moduleId)
        {
            DataProvider.Instance().UpdateRedirect(roleId, allUsers, unregisteredUsers, url, logoutUser, moduleId);
        }

        public void UpdateRedirectRef(string referrer, string url, bool matchDomainonly, int moduleId)
        {
            DataProvider.Instance().UpdateRedirectRef(referrer, url, matchDomainonly, moduleId);
        }

        public void UpdateRedirectParam(int moduleId, string paramName, RedirectInfoParam.eParamType paramType, RedirectInfoParam.eOperation operation, string value, string url, int priority, bool logoutUser)
        {
            DataProvider.Instance().UpdateRedirectParam(moduleId, paramName, (int)paramType, (int)operation, value, url, priority, logoutUser);
        }

        public void RemoveRedirect(int roleId, bool allUsers, bool unregisteredUsers)
        {
            DataProvider.Instance().RemoveRedirect(roleId, allUsers, unregisteredUsers);
        }

        public void RemoveRedirectRef(string referrer)
        {
            DataProvider.Instance().RemoveRedirectRef(referrer);
        }

        public void RemoveRedirectParam(int redirectParamId)
        {
            DataProvider.Instance().RemoveRedirectParam(redirectParamId);
        }

        public void RemoveRedirect(int redirectId)
        {
            DataProvider.Instance().RemoveRedirect(redirectId);
        }

        public void RemoveRedirectRef(int redirectRefId)
        {
            DataProvider.Instance().RemoveRedirectRef(redirectRefId);
        }

        public void IncreasePriority(int redirectId)
        {
            DataProvider.Instance().IncreasePriority(redirectId);
        }

        public void DecreasePriority(int redirectId)
        {
            DataProvider.Instance().DecreasePriority(redirectId);
        }

        static public string RegSrv = "http://www.avatar-soft.ro/DesktopModules/avt.RegCore4/Api.aspx";
        static public string ProductCode = "RDRCT";
        static public string Version = "1.6";
        static public string VersionAll = "1.6.0";
        static public string DocSrv = RegSrv + "?cmd=doc&product=" + ProductCode + "&version=" + Version;
        //static public string BuyLink = RegSrv + "?cmd=buy&product=" + ProductCode + "&version=" + Version;
        //static public string ProductKey = "<RSAKeyValue><Modulus>pPcQaveK/Va9zfdMAG3h0LSFkBuZS1Hj41uh+vjofhF7pNzMURjvxwiJXydpW9089EZsUNShB1NYtwNmzFEYmCKpohqsRFa3mcJbj3I9fgOOb8Sz8WRIEqjB3iWp1CiXUtn9nQNt1yqryj03sggSOBA8oM0CiMHEhRH9N7DJvEk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

    }
}

