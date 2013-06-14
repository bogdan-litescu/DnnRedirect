using System;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using DotNetNuke;
using DotNetNuke.Common.Utilities;
using avt;
using Microsoft.ApplicationBlocks.Data;

namespace avt.Redirect
{

    public class SqlDataProvider : DataProvider
    {

        #region "Private Members"

        private const string ProviderType = "data";

        private DotNetNuke.Framework.Providers.ProviderConfiguration _providerConfiguration = DotNetNuke.Framework.Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType);
        private string _connectionString;
        private string _providerPath;
        private string _objectQualifier;
        private string _databaseOwner;

        #endregion

        #region "Constructors"

        public SqlDataProvider()
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

        #endregion

        #region "Properties"

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        public string ProviderPath
        {
            get { return _providerPath; }
        }

        public string ObjectQualifier
        {
            get { return _objectQualifier; }
        }

        public string DatabaseOwner
        {
            get { return _databaseOwner; }
        }

        #endregion


        public override IDataReader GetRedirects(int portalId, int moduleId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "avtRedirect_GetRedirects", portalId, moduleId);
        }

        public override IDataReader GetRedirectsRef(int moduleId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "avtRedirect_GetRedirectsRef", moduleId);
        }

        public override IDataReader GetRedirectParamById(int redirectParamId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "avtRedirect_GetRedirectParamById", redirectParamId);
        }

        public override IDataReader GetRedirectsParam(int moduleId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "avtRedirect_GetRedirectsParam", moduleId);
        }

        public override void UpdateRedirect(int roleId, bool allUsers, bool unregisteredUsers, string url, bool logoutUser, int moduleId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "avtRedirect_UpdateRedirect", roleId, allUsers, unregisteredUsers, url, logoutUser, moduleId);
        }

        public override void UpdateRedirectRef(string referrer, string url, bool matchDomainonly, int moduleId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "avtRedirect_UpdateRedirectRef", referrer, url, matchDomainonly, moduleId);
        }

        public override void UpdateRedirectParam(int moduleId, string paramName, int paramType, int operation, string value, string url, int priority, bool logoutUser)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "avtRedirect_UpdateRedirectParam", moduleId, paramName, paramType, operation, value, url, priority, logoutUser);
        }

        public override void RemoveRedirect(int roleId, bool allUsers, bool unregisteredUsers)
        {
            SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "avtRedirect_RemoveRedirect", roleId, allUsers, unregisteredUsers);
        }

        public override void RemoveRedirectRef(string referrer)
        {
            SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "avtRedirect_RemoveRedirectRef", referrer);
        }

        public override void RemoveRedirect(int redirectId)
        {
            SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "avtRedirect_RemoveRedirectById", redirectId);
        }

        public override void RemoveRedirectRef(int redirectRefId)
        {
            SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "avtRedirect_RemoveRedirectRefById", redirectRefId);
        }

        public override void RemoveRedirectParam(int redirectParamId)
        {
            SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "avtRedirect_RemoveRedirectParamById", redirectParamId);
        }
        

        public override void IncreasePriority(int redirectId)
        {
            SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "avtRedirect_IncreasePriority", redirectId);
        }

        public override void DecreasePriority(int redirectId)
        {
            SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "avtRedirect_DecreasePriority", redirectId);
        }



    }
}
