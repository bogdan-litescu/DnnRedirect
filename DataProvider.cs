
using System;
using DotNetNuke;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

using avt;
using DotNetNuke.Framework;

namespace avt.Redirect
{
    public abstract class DataProvider
    {

        #region "Shared/Static Methods"

        // singleton reference to the instantiated object 
        private static DataProvider objProvider = null;

        // constructor
        static DataProvider()
        {
            CreateProvider();
        }

        // dynamically create provider
        private static void CreateProvider()
        {
            //objProvider = (DataProvider)Reflection.CreateObject("data", "avt.DotNetNuke.Modules.UserManagement", "");
            objProvider = (DataProvider)Reflection.CreateObject("data", "avt.Redirect", "");
        }

        // return the provider
        public static DataProvider Instance()
        {
            return objProvider;
        }

        #endregion


        public abstract IDataReader GetRedirects(int portalId, int moduleId);
        public abstract IDataReader GetRedirectsRef(int moduleId);
        public abstract IDataReader GetRedirectParamById(int redirectParamId);
        public abstract IDataReader GetRedirectsParam(int moduleId);

        public abstract void UpdateRedirect(int roleId, bool allUsers, bool unregisteredUsers, string url, bool logoutUser, int moduleId);
        public abstract void UpdateRedirectRef(string referrer, string url, bool matchDomainonly, int moduleId);
        public abstract void UpdateRedirectParam(int moduleId, string paramName, int paramType, int operation, string value, string url, int priority, bool logoutUser);

        public abstract void RemoveRedirect(int roleId, bool allUsers, bool unregisteredUsers);
        public abstract void RemoveRedirectRef(string referrer);
        public abstract void RemoveRedirectParam(int redirectParamId);
        //public abstract void RemoveRedirectParam(string paramName, int paramType);
        public abstract void RemoveRedirect(int redirectId);
        public abstract void RemoveRedirectRef(int redirectRefId);
        //public abstract void RemoveRedirectParam(int redirectParamId);
        public abstract void IncreasePriority(int redirectId);
        public abstract void DecreasePriority(int redirectId);
    }
}
