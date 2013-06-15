#region License
/**
 * TableSchemaExporter
 * Author: Kelvin Miles (kelvinm1@aol.com)
 *
 * Copyright (C) 2013 Kelvin Miles
 * 
 * This program is free software: you can redistribute it and/or modify it under 
 * the terms of the GNU General Public License as published by the Free Software 
 * Foundation, either version 3 of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
 * details.
 *
 * You should have received a copy of the GNU General Public License along with
 * this program. If not, see <http://www.gnu.org/licenses/>.
 *
 */
#endregion License
namespace TableSchemaExporter
{
    partial class MainForm
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
            this.dataExtractWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckbxTableNameList = new System.Windows.Forms.CheckedListBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.chbxSelectAll = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataExtractWorker
            // 
            this.dataExtractWorker.WorkerReportsProgress = true;
            this.dataExtractWorker.WorkerSupportsCancellation = true;
            this.dataExtractWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.dataExtractWorker_DoWork);
            this.dataExtractWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.dataExtractWorker_ProgressChanged);
            this.dataExtractWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.dataExtractWorker_RunWorkerCompleted);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckbxTableNameList);
            this.groupBox1.Controls.Add(this.btnExport);
            this.groupBox1.Controls.Add(this.btnRefresh);
            this.groupBox1.Controls.Add(this.chbxSelectAll);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(500, 328);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Table(s):";
            // 
            // ckbxTableNameList
            // 
            this.ckbxTableNameList.FormattingEnabled = true;
            this.ckbxTableNameList.HorizontalScrollbar = true;
            this.ckbxTableNameList.Location = new System.Drawing.Point(9, 24);
            this.ckbxTableNameList.Name = "ckbxTableNameList";
            this.ckbxTableNameList.Size = new System.Drawing.Size(360, 274);
            this.ckbxTableNameList.TabIndex = 0;
            this.ckbxTableNameList.Click += new System.EventHandler(this.ckbxTableNameList_Click);
            this.ckbxTableNameList.SelectedIndexChanged += new System.EventHandler(this.ckbxTableNameList_SelectedIndexChanged);
            this.ckbxTableNameList.DoubleClick += new System.EventHandler(this.ckbxTableNameList_DoubleClick);
            this.ckbxTableNameList.Leave += new System.EventHandler(this.ckbxTableNameList_Leave);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(375, 189);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(115, 42);
            this.btnExport.TabIndex = 9;
            this.btnExport.Text = "Export Table Schema(s)";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(375, 132);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(115, 42);
            this.btnRefresh.TabIndex = 10;
            this.btnRefresh.Text = "Refresh Table Names";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // chbxSelectAll
            // 
            this.chbxSelectAll.AutoSize = true;
            this.chbxSelectAll.Location = new System.Drawing.Point(401, 98);
            this.chbxSelectAll.Name = "chbxSelectAll";
            this.chbxSelectAll.Size = new System.Drawing.Size(70, 17);
            this.chbxSelectAll.TabIndex = 7;
            this.chbxSelectAll.Text = "Select All";
            this.chbxSelectAll.UseVisualStyleBackColor = true;
            this.chbxSelectAll.CheckedChanged += new System.EventHandler(this.chbxSelectAll_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 301);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Tables selected: {0}";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 350);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 23);
            this.progressBar1.TabIndex = 8;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(333, 360);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(179, 13);
            this.linkLabel1.TabIndex = 11;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Open folder containing exported files";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 387);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Export Table Schema";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker dataExtractWorker;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox ckbxTableNameList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chbxSelectAll;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}

