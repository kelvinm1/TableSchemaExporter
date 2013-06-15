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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace TableSchemaExporter
{
    static class Program
    {
        #region Private Data Members
        private static string m_programPath;
        private static string m_exportPath;
        #endregion Private Data Members

        #region Properties
        /// <summary>
        /// Gets the working directory for this application using reflection.
        /// </summary>
        public static string ProgramPath
        {
            get
            {
                if (string.IsNullOrEmpty(m_programPath))
                {
                    string filePath = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                    m_programPath = Path.GetDirectoryName(filePath);
                }

                return m_programPath;
            }
        }

        /// <summary>
        /// Gets the fixed path used by this program to export table schema Excel files
        /// </summary>
        public static string ExportPath
        {
            get
            {
                if (m_exportPath == null)
                {
                    m_exportPath = Path.Combine(ProgramPath, "Table Schema Views");
                }

                return m_exportPath;
            }
        }
        #endregion Properties

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
