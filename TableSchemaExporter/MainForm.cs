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
#region Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using TableSchemaExporter.Properties;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
#endregion Using Directives

namespace TableSchemaExporter
{
    /// <summary>
    /// Main UI Window
    /// </summary>
    public partial class MainForm : Form
    {
        #region Constructors
        /// <summary>
        /// Default MainForm UI Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            UpdateNumberOfCheckedItems();
        }
        #endregion Constructors

        #region Private Methods
        /// <summary>
        /// Load table schema names into CheckBoxList control in the format: [schema].[name]
        /// </summary>
        private void LoadTables()
        {
            // Use datasource specified in app.config to query SQL Server database for list of table schemas
            using (DataAccess data = new DataAccess(ConfigurationManager.ConnectionStrings["datasource"].ConnectionString))
            {
                using (SqlCommand cmd = data.GetCommand(Resources.select_tables_query))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Refresh CheckBoxList control with latest table schemas
                        ckbxTableNameList.Items.Clear();
                        while (reader.Read())
                        {
                            ckbxTableNameList.Items.Add(reader["SchemaTable"]);
                        }
                    }
                }
            }

            // Update table selected
            UpdateNumberOfCheckedItems();
        }

        /// <summary>
        /// Disable/Enable UI elements in the MainForm. 
        /// This is mainly called just before the background process of exporting is started and after it has completed.
        /// </summary>
        /// <param name="flag"></param>
        private void DisableUI(bool flag)
        {
            flag = !flag;
            this.groupBox1.Enabled = flag;
            this.btnExport.Enabled = flag;
            this.btnRefresh.Enabled = flag;
            this.chbxSelectAll.Enabled = flag;
            this.label1.Enabled = flag;
        }

        /// <summary>
        /// This method actual performs the work of exporting each table schema into its own standard Excel (xls) formatted file.
        /// </summary>
        /// <param name="schemaDataTable">DataTable object containing schema information for table</param>
        /// <param name="filename">filename used to save this excel</param>
        /// <param name="excel">instance of MS Excel object</param>
        private void ExportTableDataToExcel(DataTable schemaDataTable, string filename, Microsoft.Office.Interop.Excel.Application excel)
        {
            // Create instance of MS Excel if done NOT already
            if (excel == null)
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
            }

            Microsoft.Office.Interop.Excel.Workbook workbook = excel.Application.Workbooks.Add(true);

            // Add column headings...
            int iCol = 0;
            foreach (DataColumn c in schemaDataTable.Columns)
            {
                iCol++;
                excel.Cells[1, iCol] = c.ColumnName;
            }
            // for each row of data...
            int iRow = 0;
            foreach (DataRow r in schemaDataTable.Rows)
            {
                iRow++;

                // add each row's cell data...
                iCol = 0;
                foreach (DataColumn c in schemaDataTable.Columns)
                {
                    iCol++;
                    excel.Cells[iRow + 1, iCol] = r[c.ColumnName];
                }
            }

            // Global missing reference for objects we are not defining...
            object missing = System.Reflection.Missing.Value;

            // Save the workbook...
            workbook.SaveAs(filename,
                Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, missing, missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                missing, missing, missing, missing, missing);
            
            // If wanting to make Excel visible and activate the worksheet, uncomment the following code...
            // excel.Visible = true;
            // Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveSheet;
            // ((Microsoft.Office.Interop.Excel._Worksheet)worksheet).Activate();
            
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Close Workbook...
            if (workbook != null)
                workbook.Close(true, missing, missing);
            NAR(workbook);
        }

        /// <summary>
        /// This method performs the release of the Excel COM object. 
        /// We do this to fix a known issue where Excel does not quit after automation from a .NET client.
        /// <see cref="http://support.microsoft.com/kb/317109"/>
        /// </summary>
        /// <param name="o"></param>
        private void NAR(object o)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(o);
            }
            catch { }
            finally
            {
                o = null;
            }
        }

        /// <summary>
        /// Cleans up the "Table Schema Views" folder used to export each table as it
        /// </summary>
        /// <returns></returns>
        private bool CleanExportPath()
        {
            bool result = true;

            try
            {
                DirectoryInfo dir = new DirectoryInfo(Program.ExportPath);
                foreach (System.IO.FileInfo file in dir.GetFiles()) file.Delete();
            }
            catch
            {
                // return false
            }

            return result;
        }

        /// <summary>
        /// Thread-safe call to export table schemas and is used by the background worker to start the exporting process.
        /// </summary>
        private void DoExport()
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            using (DataAccess data = new DataAccess(ConfigurationManager.ConnectionStrings["datasource"].ConnectionString))
            {
                int count = 0;
                int items = ckbxTableNameList.CheckedItems.Count;

                // for each item in the checklist....
                foreach (object table in ckbxTableNameList.CheckedItems)
                {
                    // Update progress for progressbar...
                    int progress = (int)(((double)(count + 1) / (double)items) * 100);

                    string tableName = table.ToString();

                    using (SqlCommand cmd = data.GetCommand(Resources.select_table_schema_query))
                    {
                        // Add table name parameter
                        cmd.Parameters.AddWithValue("@TableName", tableName);

                        using (SqlDataAdapter reader = new SqlDataAdapter(cmd))
                        {
                            // Remove [] characters around the schema & table names
                            string fileName = Path.Combine(Program.ExportPath, 
                                Regex.Replace(tableName, @"[\[\]]", string.Empty) + ".xls");
                            DataSet ds = new DataSet();
                            reader.Fill(ds);
                            ExportTableDataToExcel(ds.Tables[0], fileName, excel);
                        }
                    }

                    count++;
                    dataExtractWorker.ReportProgress(progress);
                    Thread.Sleep(100);
                }
            }

            // Quit and release instance of MS Excel
            excel.Quit();
            NAR(excel);
        }

        /// <summary>
        /// Updates the number of tables selected to export in the CheckBoxList control.
        /// </summary>
        private void UpdateNumberOfCheckedItems()
        {
            label1.Text = string.Format("Tables selected: {0}", ckbxTableNameList.CheckedItems.Count);
        }
        #endregion Private Methods

        #region UI Event Handlers
        /// <summary>
        /// MainForm load event handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Check to see if our folder for exported files already exists. If not, create it.
                if (!Directory.Exists(Path.Combine(Program.ExportPath)))
                {
                    Directory.CreateDirectory(Program.ExportPath);
                }

                // Now load list of table schemas
                LoadTables();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// CheckBoxList checkedChanged event handle to updating the count of selected tables when a box is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < ckbxTableNameList.Items.Count; i++)
            {
                ckbxTableNameList.SetItemChecked(i, chbxSelectAll.Checked);
            }
            UpdateNumberOfCheckedItems();
        }

        #region Command Buttion Events
        /// <summary>
        /// Refresh button click event handle to update the list of tables in the CheckBoxList control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadTables();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Export button click event handle to start the export process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            DisableUI(true);

            this.Cursor = Cursors.WaitCursor;

            try
            {
                // Clean export path...
                CleanExportPath();

                // Start worker thread....
                dataExtractWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error");
            }
        }
        #endregion Command Button Events

        #region Background Worker Events
        /// <summary>
        /// Background worker event to handle work of exporting the table schemas to Excel file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataExtractWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            dataExtractWorker.ReportProgress(0);
            DoExport();
        }

        /// <summary>
        /// Background worker event to handle progress update when BackgroundWorker.ReportProgress() is called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataExtractWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Background worker event for when the export process is done.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataExtractWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Changes the cursor back to the default, 
            // sets the progress to zero, and 
            // enables the UI form when done exporting
            this.Cursor = Cursors.Default;
            progressBar1.Value = 0;
            DisableUI(false);

            MessageBox.Show(this, "Process Completed!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion Background Worker Events

        #region CheckListBox Events
        /// <summary>
        /// CheckBoxList leave event handle to update count of selected tables.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbxTableNameList_Leave(object sender, EventArgs e)
        {
            UpdateNumberOfCheckedItems();
        }

        /// <summary>
        /// CheckBoxList selectedIndexChanged event handle to update count of selected tables.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbxTableNameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateNumberOfCheckedItems();
        }

        /// <summary>
        /// CheckBoxList click event handle to update count of selected tables.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbxTableNameList_Click(object sender, EventArgs e)
        {
            UpdateNumberOfCheckedItems();
        }

        /// <summary>
        /// CheckBoxList doubleClick event handle to update count of selected tables.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbxTableNameList_DoubleClick(object sender, EventArgs e)
        {
            UpdateNumberOfCheckedItems();
        }

        /// <summary>
        /// CheckBoxList itemCheck event handle to update count of selected tables.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbxTableNameList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            UpdateNumberOfCheckedItems();
        }
        #endregion CheckListBox Events

        /// <summary>
        /// Link click event handle to open export path in Windows Exporer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string windir = Environment.GetEnvironmentVariable("WINDIR");
            System.Diagnostics.Process prc = new System.Diagnostics.Process();
            prc.StartInfo.FileName = windir + @"\explorer.exe";
            prc.StartInfo.Arguments = Program.ExportPath;
            prc.Start();
        }

        #endregion UI Event Handlers
    }
}
