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
using System.Text;
using System.Data.SqlClient;

namespace TableSchemaExporter
{
    /// <summary>
    /// This is a wrapper class for database connection.
    /// </summary>
    public class DataAccess : IDisposable
    {
        #region Data Members
        /// <summary>
        /// OdbcConnection : This is the connection
        /// </summary>
        SqlConnection oConnection;

        /// <summary>
        /// OdbcCommand : This is the command
        /// </summary>
        SqlCommand oCommand;
        #endregion

        /// <summary>
        /// Creates an instance of a disposable DataAccess object.
        /// </summary>
        /// <param name="dataSourceName">string: This is the data source name</param>
        public DataAccess(string dataSourceName)
        {
            // Instantiate the SQL connection
            oConnection = new SqlConnection(dataSourceName);

            try
            {
                // Open the connection
                oConnection.Open();

                // DEBUG: Notify the user that the connection is opened
                System.Diagnostics.Debug.WriteLine(
                    string.Format("The connection is established with the database: {0}", oConnection.Database));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// It is used to close the connection to datasource
        /// mode
        /// </summary>
        public void CloseConnection()
        {
            oConnection.Close();
        }

        /// <summary>
        /// This function returns a valid SQL command
        /// </summary>
        /// <param name="Query">string: This is the SQL query</param>
        /// <returns>SQLCommand</returns>
        public SqlCommand GetCommand(string Query)
        {
            oCommand = new SqlCommand();
            oCommand.Connection = oConnection;
            oCommand.CommandText = Query;
            return oCommand;
        }

        #region IDisposable Members
        /// <summary>
        /// This method close the actual connection
        /// </summary>
        void IDisposable.Dispose()
        {
            oConnection.Close();
        }

        #endregion
    }
}
