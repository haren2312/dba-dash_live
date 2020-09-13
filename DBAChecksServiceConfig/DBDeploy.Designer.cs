﻿namespace DBAChecksServiceConfig
{
    partial class DBDeploy
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.cboDatabase = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bttnGenerate = new System.Windows.Forms.Button();
            this.txtDeployScript = new System.Windows.Forms.TextBox();
            this.lblNotice = new System.Windows.Forms.Label();
            this.bttnCancel = new System.Windows.Forms.Button();
            this.bttnCopy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(16, 33);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(536, 22);
            this.txtConnectionString.TabIndex = 0;
            this.txtConnectionString.Validated += new System.EventHandler(this.txtConnectionString_Validated);
            // 
            // lblConnectionString
            // 
            this.lblConnectionString.AutoSize = true;
            this.lblConnectionString.Location = new System.Drawing.Point(13, 13);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(124, 17);
            this.lblConnectionString.TabIndex = 1;
            this.lblConnectionString.Text = "Connection String:";
            // 
            // cboDatabase
            // 
            this.cboDatabase.FormattingEnabled = true;
            this.cboDatabase.Location = new System.Drawing.Point(16, 85);
            this.cboDatabase.Name = "cboDatabase";
            this.cboDatabase.Size = new System.Drawing.Size(536, 24);
            this.cboDatabase.TabIndex = 2;
            this.cboDatabase.Text = "DBAChecksDB";
            this.cboDatabase.DropDown += new System.EventHandler(this.cboDatabase_DropDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Database:";
            // 
            // bttnGenerate
            // 
            this.bttnGenerate.Location = new System.Drawing.Point(404, 132);
            this.bttnGenerate.Name = "bttnGenerate";
            this.bttnGenerate.Size = new System.Drawing.Size(148, 23);
            this.bttnGenerate.TabIndex = 4;
            this.bttnGenerate.Text = "Generate Script";
            this.bttnGenerate.UseVisualStyleBackColor = true;
            this.bttnGenerate.Click += new System.EventHandler(this.bttnGenerate_Click);
            // 
            // txtDeployScript
            // 
            this.txtDeployScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDeployScript.Location = new System.Drawing.Point(16, 183);
            this.txtDeployScript.Multiline = true;
            this.txtDeployScript.Name = "txtDeployScript";
            this.txtDeployScript.ReadOnly = true;
            this.txtDeployScript.Size = new System.Drawing.Size(536, 432);
            this.txtDeployScript.TabIndex = 5;
            this.txtDeployScript.TextChanged += new System.EventHandler(this.txtDeployScript_TextChanged);
            this.txtDeployScript.Validated += new System.EventHandler(this.txtDeployScript_Validated);
            // 
            // lblNotice
            // 
            this.lblNotice.AutoSize = true;
            this.lblNotice.ForeColor = System.Drawing.Color.Blue;
            this.lblNotice.Location = new System.Drawing.Point(13, 112);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.Size = new System.Drawing.Size(91, 17);
            this.lblNotice.TabIndex = 6;
            this.lblNotice.Text = "Please wait...";
            this.lblNotice.Visible = false;
            // 
            // bttnCancel
            // 
            this.bttnCancel.Location = new System.Drawing.Point(404, 131);
            this.bttnCancel.Name = "bttnCancel";
            this.bttnCancel.Size = new System.Drawing.Size(148, 23);
            this.bttnCancel.TabIndex = 7;
            this.bttnCancel.Text = "Cancel";
            this.bttnCancel.UseVisualStyleBackColor = true;
            this.bttnCancel.Visible = false;
            this.bttnCancel.Click += new System.EventHandler(this.bttnCancel_Click);
            // 
            // bttnCopy
            // 
            this.bttnCopy.Enabled = false;
            this.bttnCopy.Image = global::DBAChecksServiceConfig.Properties.Resources.ASX_Copy_blue_16x;
            this.bttnCopy.Location = new System.Drawing.Point(16, 149);
            this.bttnCopy.Name = "bttnCopy";
            this.bttnCopy.Size = new System.Drawing.Size(36, 28);
            this.bttnCopy.TabIndex = 8;
            this.bttnCopy.UseVisualStyleBackColor = true;
            this.bttnCopy.Click += new System.EventHandler(this.bttnCopy_Click);
            // 
            // DBDeploy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 649);
            this.Controls.Add(this.bttnCopy);
            this.Controls.Add(this.bttnCancel);
            this.Controls.Add(this.lblNotice);
            this.Controls.Add(this.txtDeployScript);
            this.Controls.Add(this.bttnGenerate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboDatabase);
            this.Controls.Add(this.lblConnectionString);
            this.Controls.Add(this.txtConnectionString);
            this.Name = "DBDeploy";
            this.Text = "DB Deploy";
            this.Load += new System.EventHandler(this.DBDeploy_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Label lblConnectionString;
        private System.Windows.Forms.ComboBox cboDatabase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bttnGenerate;
        private System.Windows.Forms.TextBox txtDeployScript;
        private System.Windows.Forms.Label lblNotice;
        private System.Windows.Forms.Button bttnCancel;
        private System.Windows.Forms.Button bttnCopy;
    }
}