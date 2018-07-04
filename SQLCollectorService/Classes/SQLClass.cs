using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace SQLCollectorService.Classes
{
    static class SqlHelper
    {
        /// <summary>
        /// Set the connection, command, and then execute the command with non query.
        /// </summary>
        public static Int32 ExecuteNonQuery(String connectionString, String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    // There're three command types: StoredProcedure, Text, TableDirect. The TableDirect 
                    // type is only for OLE DB.  
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Set the connection, command, and then execute the command and only return one value.
        /// </summary>
        public static Object ExecuteScalar(String connectionString, String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Set the connection, command, and then execute the command with query and return the reader.
        /// </summary>
        public static SqlDataReader ExecuteReader(String connectionString, String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);

                conn.Open();
                // When using CommandBehavior.CloseConnection, the connection will be closed when the 
                // IDataReader is closed.
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
        }

        /// <summary>
        /// Set the connection, command, and then execute the command with query and return the DataTable.
        /// </summary>
        public static DataTable FillDataTable(String connectionString, String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            DataTable ReturnTable = new DataTable();
            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);

                conn.Open();
                // When using CommandBehavior.CloseConnection, the connection will be closed when the 
                // IDataReader is closed.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ReturnTable);

                return ReturnTable;
            }
        }


        /// <summary>
        /// Set the connection, the DataTable, SQLTable to insert the Datatable into.
        /// </summary>
        public static void LoadDataTable(String connectionString, DataTable InputDataTable, String SQLTableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    foreach (DataColumn c in InputDataTable.Columns)
                        bulkCopy.ColumnMappings.Add(c.ColumnName, c.ColumnName);

                    bulkCopy.DestinationTableName = SQLTableName;
                    try
                    {
                        bulkCopy.WriteToServer(InputDataTable);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}

