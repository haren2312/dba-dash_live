﻿using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBADashService
{
    public class MaintenanceJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string connectionString = dataMap.GetString("ConnectionString");
            try
            {
                AddPartitions(connectionString);
            }
            catch(Exception ex)
            {
                logError(connectionString, "AddPartitions", ex.Message);
            }
            try
            {
                PurgeData(connectionString);
            }
            catch(Exception ex)
            {
                logError(connectionString, "PurgeData", ex.Message);
            }
            return Task.CompletedTask;
        }

        public static void AddPartitions(string connectionString)
        {
            var cn = new SqlConnection(connectionString);
            using (cn)
            {
                using (var cmd = new SqlCommand("dbo.Partitions_Add", cn) { CommandType = CommandType.StoredProcedure }) {
                    cn.Open();
                    Console.WriteLine("Maintenance: Creating partitions");
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void PurgeData(string connectionString)
        {
            var cn = new SqlConnection(connectionString);
            using (cn)
            {
                using (var cmd = new SqlCommand("dbo.PurgeData", cn) { CommandType = CommandType.StoredProcedure })
                {
                    cn.Open();
                    Console.WriteLine("Maintenance: PurgeData");
                    cmd.ExecuteNonQuery();
                }
            }
        }


        private void logError(string connectionString, string errorSource, string errorMessage, string errorContext = "Maintenance")
        {
            Console.WriteLine("Error: " + errorContext + " - " + errorSource + " : " + errorMessage);
            try
            {
                var dtErrors = new DataTable("Errors");
                dtErrors.Columns.Add("ErrorSource");
                dtErrors.Columns.Add("ErrorMessage");
                dtErrors.Columns.Add("ErrorContext");
                var rError = dtErrors.NewRow();
                rError["ErrorSource"] = errorSource;
                rError["ErrorMessage"] = errorMessage;
                rError["ErrorContext"] = errorContext;
                dtErrors.Rows.Add(rError);
                DataSet ds = new DataSet();
                ds.Tables.Add(dtErrors);
                DBADash.DBImporter.InsertErrors(connectionString, null, DateTime.UtcNow, ds);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error logging errors from MaintenanceJob: " + ex.Message);
            }
        }
        
    }
}