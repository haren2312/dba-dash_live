﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DBAChecksGUI.Changes
{
    public partial class TraceFlagHistory : UserControl
    {
        public TraceFlagHistory()
        {
            InitializeComponent();
        }

        public string ConnectionString;
        public List<Int32> InstanceIDs;


        private void getFlags()
        {
            SqlConnection cn = new SqlConnection(ConnectionString);
            using (cn)
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("dbo.TraceFlags_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InstanceIDs", string.Join(",", InstanceIDs));
                SqlDataReader rdr = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Columns.Add("Instance");
                string instance = "";
                string previousInstance = "";
                DataRow r=null;
                while (rdr.Read())
                {
                    instance = (string)rdr["ConnectionID"];
                    if (instance != previousInstance)
                    {
                        r= dt.NewRow();
                        dt.Rows.Add(r);
                        r["Instance"] = instance;
                    }
                    if(rdr["TraceFlag"] != DBNull.Value)
                    {
                        var flag = (Int16)rdr["TraceFlag"];
                        var colName = "T" + flag.ToString();
                        var validFrom = (DateTime)rdr["ValidFrom"];
                        if (!dt.Columns.Contains(colName))
                        {
                            dt.Columns.Add(colName);
                        }
                        if (validFrom > DateTime.Parse("1900-01-01"))
                        {
                            r[colName] = "Y (" + validFrom.ToLocalTime().ToString("yyyy-MM-dd") + ")";
                        }
                        else
                        {
                            r[colName] = "Y";
                        }
                    }
                    previousInstance = instance;
                }
                
                dgvFlags.DataSource = dt;
            }
        }

        public void RefreshData()
        {
            getFlags();
            SqlConnection cn = new SqlConnection(ConnectionString);
            using (cn)
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("dbo.TraceFlagHistory_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InstanceIDs", string.Join(",", InstanceIDs));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv.AutoGenerateColumns = false;
                dgv.DataSource = dt;
            }
        }
    }


}
