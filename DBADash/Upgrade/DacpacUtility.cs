﻿using Microsoft.SqlServer.Dac;
using Serilog;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using static DBADash.DBValidations;

namespace DacpacUtility
{
    public class DacpacService
    {
        public List<string> MessageList { get; set; }

        public DacpacService()
        {
            MessageList = new List<string>();
            _dacDeployOptions = new DacDeployOptions
            {
                IgnorePermissions = true,
                IgnoreTableOptions = true,
                IgnorePartitionSchemes = true,
                IgnoreIndexOptions = true,
                IgnoreUserSettingsObjects = true,
                IgnoreRoleMembership = true,
                DropObjectsNotInSource = false,
                BlockOnPossibleDataLoss = false,
                RegisterDataTierApplication = false,
                BlockWhenDriftDetected = false,
                ExcludeObjectTypes = new[]
                {
                    ObjectType.Users,
                    ObjectType.Logins,
                    ObjectType.RoleMembership,
                    ObjectType.Permissions
                }
            };
        }

        public void ProcessDacPac(string connectionString,
                                    string databaseName,
                                    string dacpacName,
                                    DBVersionStatusEnum status,
                                    int retryCount = 0)
        {
            MessageList.Add("*** Start of processing for " +
                             databaseName);
            // For an existing DB we want to skip this option.  Some users might prefer to use SIMPLE recovery model and this option would set it back to FULL each time the dacpac is deployed
            _dacDeployOptions.ScriptDatabaseOptions = status == DBVersionStatusEnum.CreateDB;
            if (!_dacDeployOptions.ScriptDatabaseOptions)
            {
                MessageList.Add("Skipping deployment of Database Options for existing database");
            }

            var dacServiceInstance = new DacServices(connectionString);
            dacServiceInstance.ProgressChanged +=
              (s, e) =>
              {
                  MessageList.Add(e.Message);
                  Log.Information(e.Message);
              };
            dacServiceInstance.Message +=
              (s, e) =>
              {
                  MessageList.Add(e.Message.Message);
                  Log.Information(e.Message.Message);
              };

            try
            {
                using (var dacpac = DacPackage.Load(dacpacName))
                {
                    if (_dacDeployOptions.SqlCommandVariableValues.ContainsKey("VersionNumber"))
                    {
                        _dacDeployOptions.SqlCommandVariableValues.Remove("VersionNumber");
                    }

                    _dacDeployOptions.SqlCommandVariableValues.Add("VersionNumber", dacpac.Version.ToString());
                    dacServiceInstance.Deploy(dacpac, databaseName,
                        upgradeExisting: true,
                        options: _dacDeployOptions);
                }
            }
            catch (DacServicesException ex) when (ex.Message.Contains("IX_Instances_InstanceDisplayName") && retryCount == 0)
            {
                var msg = "Encountered a known issue processing dacpac.Applying fix & re - running dacpac. ";
                MessageList.Add(msg);
                Log.Warning(ex, msg);
                DropIndexFix_1040(connectionString, databaseName);
                retryCount++;
                ProcessDacPac(connectionString, databaseName, dacpacName, status, retryCount);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error processing dacpac");
                MessageList.Add(ex.Message);
                throw;
            }
        }

        // Fix upgrade issue that occurs with versions 3.3 and older. #1040
        private static void DropIndexFix_1040(string connectionString, string databaseName)
        {
            var builder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = databaseName
            };
            const string sql = "DROP INDEX IX_Instances_InstanceDisplayName ON dbo.Instances;";
            using var cn = new SqlConnection(builder.ConnectionString);
            using var cmd = new SqlCommand(sql, cn);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        private DacDeployOptions _dacDeployOptions;

        public DacDeployOptions DacDeployOpts
        {
            get => _dacDeployOptions;
            set => _dacDeployOptions = value;
        }

        public static Version GetVersion(string dacpacName)
        {
            using (var dacpac = DacPackage.Load(dacpacName))
            {
                return dacpac.Version;
            }
        }

        public string GenerateDeployScript(string connectionString,
                                string databaseName,
                                string dacpacName)
        {
            MessageList.Add("*** Start of processing for " +
                             databaseName);

            var dacServiceInstance = new DacServices(connectionString);
            dacServiceInstance.ProgressChanged +=
              (s, e) =>
                  MessageList.Add(e.Message);
            dacServiceInstance.Message +=
              (s, e) =>
                  MessageList.Add(e.Message.Message);

            try
            {
                using (var dacpac = DacPackage.Load(dacpacName))
                {
                    if (_dacDeployOptions.SqlCommandVariableValues.ContainsKey("VersionNumber"))
                    {
                        _dacDeployOptions.SqlCommandVariableValues.Remove("VersionNumber");
                    }
                    _dacDeployOptions.SqlCommandVariableValues.Add("VersionNumber", dacpac.Version.ToString());
                    return dacServiceInstance.GenerateDeployScript(dacpac, databaseName,
                                            options: _dacDeployOptions);
                }
            }
            catch (Exception ex)
            {
                MessageList.Add(ex.Message);
                return null;
            }
        }
    }
}