﻿using DBADashGUI.Changes;
using DBADashGUI.CustomReports;
using DBADashGUI.Theme;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBADashGUI.Performance
{
    public partial class AzureSummary : UserControl, ISetContext, IRefreshData
    {
        public AzureSummary()
        {
            InitializeComponent();
            dgv.RegisterClearFilter(tsClearFilter);
            dgvPool.RegisterClearFilter(tsClearFilterPool);
        }

        private List<int> InstanceIDs;

        public void SetContext(DBADashContext _context)
        {
            InstanceIDs = _context.AzureInstanceIDs.ToList();
            RefreshData();
        }

        public void RefreshData()
        {
            if (Common.ConnectionString == null) return;
            RefreshDB();
            RefreshPool();
        }

        private DataTable GetAzureDBPerformanceSummary()
        {
            using var cn = new SqlConnection(Common.ConnectionString);
            using SqlCommand cmd = new("dbo.AzureDBPerformanceSummary_Get", cn) { CommandType = CommandType.StoredProcedure };
            cn.Open();

            if (InstanceIDs.Count > 0)
            {
                cmd.Parameters.AddWithValue("InstanceIDs", string.Join(",", InstanceIDs));
            }
            var histogram = HasHistograms(ref dgv);
            cmd.Parameters.AddWithValue("CPUHist", histogram);
            cmd.Parameters.AddWithValue("DataHist", histogram);
            cmd.Parameters.AddWithValue("LogHist", histogram);
            cmd.Parameters.AddWithValue("DTUHist", histogram);
            cmd.Parameters.AddWithValue("FromDate", DateRange.FromUTC);
            cmd.Parameters.AddWithValue("ToDate", DateRange.ToUTC);
            cmd.Parameters.AddWithValue("ShowHidden", InstanceIDs.Count == 1 || Common.ShowHidden);
            cmd.CommandTimeout = Config.DefaultCommandTimeout;
            SqlDataAdapter da = new(cmd);
            DataTable dt = new();
            da.Fill(dt);
            return dt;
        }

        private void RefreshDB()
        {
            var dt = GetAzureDBPerformanceSummary();
            dgv.AutoGenerateColumns = false;
            dgv.DataSource = new DataView(dt);
            GenerateHistogram(dgv);
            dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgv.ApplyTheme(DBADashUser.SelectedTheme);
        }

        private void RefreshPool()
        {
            var dt = GetAzureDBPoolSummary();
            dgvPool.AutoGenerateColumns = false;
            dgvPool.DataSource = new DataView(dt);
            GenerateHistogram(dgvPool);
            dgvPool.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgv.ApplyTheme(DBADashUser.SelectedTheme);
        }

        private DataTable GetAzureDBPoolSummary()
        {
            using var cn = new SqlConnection(Common.ConnectionString);
            using SqlCommand cmd = new("dbo.AzureDBPoolSummary_Get", cn) { CommandType = CommandType.StoredProcedure };
            cn.Open();

            if (InstanceIDs.Count > 0)
            {
                cmd.Parameters.AddWithValue("InstanceIDs", string.Join(",", InstanceIDs));
            }
            var histogram = HasHistograms(ref dgvPool);
            cmd.Parameters.AddWithValue("CPUHist", histogram);
            cmd.Parameters.AddWithValue("DataHist", histogram);
            cmd.Parameters.AddWithValue("LogHist", histogram);
            cmd.Parameters.AddWithValue("DTUHist", histogram);
            cmd.Parameters.AddWithValue("FromDate", DateRange.FromUTC);
            cmd.Parameters.AddWithValue("ToDate", DateRange.ToUTC);
            cmd.Parameters.AddWithValue("ShowHidden", InstanceIDs.Count == 1 || Common.ShowHidden);
            cmd.CommandTimeout = Config.DefaultCommandTimeout;
            SqlDataAdapter da = new(cmd);
            DataTable dt = new();
            da.Fill(dt);
            return dt;
        }

        private readonly string[] histograms = new[] { "DTU", "CPU", "Data", "Log" };

        private void GenerateHistogram(DataGridView gv)
        {
            var colPrefix = gv.Name + "_";
            foreach (var histogram in histograms)
            {
                var colName = colPrefix + histogram + "Histogram";
                if (gv.Rows.Count > 0 && gv.Rows[0].Cells[colName].Visible && gv.Rows[0].Cells[colName].Value == null)
                {
                    foreach (DataGridViewRow r in gv.Rows)
                    {
                        var row = (DataRowView)r.DataBoundItem;
                        StringBuilder sbToolTip = new();
                        var hist = new List<double>();
                        double total = 0;

                        for (var i = 10; i <= 100; i += 10)
                        {
                            var v = Convert.ToDouble(row[histogram + i]);
                            hist.Add(v);
                            total += v;
                        }

                        if (total == 0)
                        {
                            r.Cells[colName].Value = new Bitmap(1, 1);
                        }
                        else
                        {
                            for (var i = 10; i <= 100; i += 10)
                            {
                                var v = Convert.ToDouble(row[histogram + i]);
                                sbToolTip.AppendLine((i - 10) + " to " + i + "% | " + v.ToString("N0") + " (" + (v / total).ToString("P2") + ")");
                            }
                            r.Cells[colName].Value = Histogram.GetHistogram(hist, 200, 100, true);
                            r.Height = 100;
                            r.Cells[colName].ToolTipText = sbToolTip.ToString();
                        }
                    }
                }
            }
        }

        private void AzureSummary_Load(object sender, EventArgs e)
        {
            dgv.ApplyTheme();
            dgvPool.ApplyTheme();
            AddHistCols(dgv);
            AddHistCols(dgvPool);
        }

        private void AddHistCols(DataGridView gv)
        {
            foreach (var histogram in histograms)
            {
                for (var i = 10; i <= 100; i += 10)
                {
                    var col = new DataGridViewTextBoxColumn()
                    {
                        Name = gv.Name + "_" + histogram + "Histogram_" + i,
                        DataPropertyName = histogram + i,
                        Visible = false,
                        HeaderText = histogram + " Histogram " + (i - 10) + " to " + i + "%"
                    };
                    gv.Columns.Add(col);
                }
            }
        }

        private void TsRefresh_Click(object sender, EventArgs e)
        {
            RefreshDB();
        }

        private void TsCopy_Click(object sender, EventArgs e)
        {
            ExportDGV(ExportTarget.Clipboard);
        }

        private void TsCopyPool_Click(object sender, EventArgs e)
        {
            ExportDGVPool(ExportTarget.Clipboard);
        }

        private void TsRefreshPool_Click(object sender, EventArgs e)
        {
            RefreshPool();
        }

        private void Dgv_Sorted(object sender, EventArgs e)
        {
            GenerateHistogram(dgv);
        }

        private void DgvPool_Sorted(object sender, EventArgs e)
        {
            GenerateHistogram(dgvPool);
        }

        private void Dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var col = dgv.Columns[e.ColumnIndex];
            if (e.RowIndex >= 0 && (col == colDB || col == colElasticPool))
            {
                var row = (DataRowView)dgv.Rows[e.RowIndex].DataBoundItem;
                var instance = (string)row["Instance"];
                var db = (string)row["DB"];

                var frm = new AzureDBResourceStatsView
                {
                    FromDate = DateRange.FromUTC,
                    ToDate = DateRange.ToUTC,
                    InstanceID = (int)row["InstanceID"]
                };
                if (col == colElasticPool)
                {
                    var pool = (string)row["elastic_pool_name"];
                    frm.ElasticPoolName = pool;
                    frm.Text = instance + " | " + pool;
                }
                else
                {
                    frm.Text = instance + " | " + db;
                }
                frm.ApplyTheme();
                frm.Show();
            }
            else if (e.RowIndex >= 0 && col == colServiceObjective)
            {
                var row = (DataRowView)dgv.Rows[e.RowIndex].DataBoundItem;
                var frm = new ResourceGovernanceViewer() { InstanceID = (int)row["InstanceID"], DatabaseName = (string)row["DB"] };
                frm.ApplyTheme();
                frm.Show();
            }
        }

        private void DgvPool_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvPool.Columns[e.ColumnIndex] == colPoolName)
            {
                var row = (DataRowView)dgvPool.Rows[e.RowIndex].DataBoundItem;
                var instance = (string)row["Instance"];
                var pool = (string)row["elastic_pool_name"];
                var frm = new AzureDBResourceStatsView
                {
                    FromDate = DateRange.FromUTC,
                    ToDate = DateRange.ToUTC,
                    InstanceID = (int)row["InstanceID"],
                    ElasticPoolName = pool,
                    Text = instance + " | " + pool
                };
                frm.ApplyTheme();
                frm.Show();
            }
        }

        private void DgvPol_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (var idx = e.RowIndex; idx < e.RowIndex + e.RowCount; idx += 1)
            {
                var row = (DataRowView)dgvPool.Rows[idx].DataBoundItem;
                var poolStorageStatus = (DBADashStatus.DBADashStatusEnum)row["ElasticPoolStorageStatus"];
                dgvPool.Rows[idx].Cells[colAllocatedStoragePct.Index].SetStatusColor(poolStorageStatus);
            }
        }

        private void TsExcel_Click(object sender, EventArgs e)
        {
            ExportDGV(ExportTarget.Excel);
        }

        private void TsExcelPool_Click(object sender, EventArgs e)
        {
            ExportDGVPool(ExportTarget.Excel);
        }

        private enum ExportTarget
        {
            Clipboard,
            Excel
        }

        private void ExportDGV(ExportTarget target)
        {
            var visibleCols = (new DataGridViewColumn[] { dgv_CPUHistogram, dgv_DataHistogram, dgv_DTUHistogram, dgv_LogHistogram }).Where(col => col.Visible).ToList();

            foreach (var c in visibleCols)
            {
                c.Visible = false;
            }
            if (target == ExportTarget.Clipboard)
            {
                dgv.CopyGrid();
            }
            else if (target == ExportTarget.Excel)
            {
                dgv.ExportToExcel();
            }
            foreach (var c in visibleCols)
            {
                c.Visible = true;
            }
        }

        private void ExportDGVPool(ExportTarget target)
        {
            var visibleCols = (new DataGridViewColumn[] { dgvPool_CPUHistogram, dgvPool_DataHistogram, dgvPool_DTUHistogram, dgvPool_LogHistogram }).Where(col => col.Visible).ToList();

            foreach (var c in visibleCols)
            {
                c.Visible = false;
            }
            if (target == ExportTarget.Clipboard)
            {
                dgvPool.CopyGrid();
            }
            else if (target == ExportTarget.Excel)
            {
                dgvPool.ExportToExcel();
            }
            foreach (var c in visibleCols)
            {
                c.Visible = true;
            }
        }

        private static DBADashStatus.DBADashStatusEnum GetStatus(object value)
        {
            return (DBADashStatus.DBADashStatusEnum)Convert.ToInt32(value == DBNull.Value ? 3 : value);
        }

        private void Dgv_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (var idx = e.RowIndex; idx < e.RowIndex + e.RowCount; idx += 1)
            {
                var row = (DataRowView)dgv.Rows[idx].DataBoundItem;
                var pctMaxSizeStatusColor = GetStatus(row["PctMaxSizeStatus"]);
                var snapshotStatusColor = GetStatus(row["FileSnapshotStatus"]);
                dgv.Rows[idx].Cells["colAllocatedPctMax"].SetStatusColor(pctMaxSizeStatusColor);
                dgv.Rows[idx].Cells["colFileSnapshotAge"].SetStatusColor(snapshotStatusColor);
            }
        }

        private static bool HasHistograms(ref DBADashDataGridView gv)
        {
            return gv.Columns.Cast<DataGridViewColumn>().Any(col => col.Name.Contains("Histogram") && col.Visible);
        }

        private void UpdateHistogramsIfNeeded(ref DBADashDataGridView gv)
        {
            var dv = (DataView)gv.DataSource;

            if (!HasHistograms(ref gv)) return;
            foreach (var histogram in histograms)
            {
                if (dv.Table.Columns.Contains(histogram + "10")) continue;
                if (gv == dgvPool)
                {
                    RefreshPool();
                }
                else
                {
                    RefreshDB();
                }

                return;
            }
            GenerateHistogram(gv);
        }

        private void PromptColumnSelection(ref DBADashDataGridView gv)
        {
            if (dgv.PromptColumnSelection() == DialogResult.OK)
            {
                UpdateHistogramsIfNeeded(ref gv);
                gv.AutoResizeColumns();
                gv.AutoResizeRows();
            }
        }

        private void TsCols_Click(object sender, EventArgs e)
        {
            PromptColumnSelection(ref dgv);
        }

        private void TsPoolCols_Click(object sender, EventArgs e)
        {
            PromptColumnSelection(ref dgvPool);
        }
    }
}