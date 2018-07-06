using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace SQLCollectorService.Classes
{
    static class CollectorFuctions
    {

        private static string _connectionstring = "";
        private static string _servername = "";
        public static bool IsServerConnected(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        public static string ConnectionString
        {
            get { return _connectionstring; }
            set { _connectionstring = value; }


        }

        public static string Servername
        {
            get { return _servername; }
            set { _servername = value; }


        }




        public static int CollectorInit(string ConnectionString, string ServerName)
        {
            SqlParameter SQLSourceInstance = new SqlParameter("@ServerName", SqlDbType.VarChar);
            SQLSourceInstance.Value = ServerName;
            SqlParameter CollectID = new SqlParameter("@CollectID", SqlDbType.Int);
            CollectID.Direction = ParameterDirection.Output;
            Int32 outCollectID = 0;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(ConnectionString, "usp_NewCollection", System.Data.CommandType.StoredProcedure, SQLSourceInstance, CollectID))
            {
                    outCollectID = (int)CollectID.Value;
            }


            return outCollectID;

                           
        }


        public static void CollectServerInfo(string ConnectionString, string DWConnectionString, int CollectID)
        {
         string ServerinfoSQL =  "create table #nodes (nodename varchar(64));" +
                    "IF EXISTS(SELECT SERVERPROPERTY('IsHadrEnabled') where SERVERPROPERTY('IsHadrEnabled')= 1 )" +
                    "insert into #nodes " +
                    "select member_name FROM master.sys.dm_hadr_cluster_members where member_type = 0;" +
                    "insert into #nodes select NodeName FROM sys.dm_os_cluster_nodes;" +
                    "declare @ha_nodes varchar(128) " +
                    "select @ha_nodes = ISNULL(STUFF((SELECT '', ',' + nodename FROM #nodes FOR XML PATH('')), 1,1 , '' ), 'Not a cluster/Always On'); " +
                    "" +
                    "SELECT " +
                    CollectID.ToString() + " AS CollectID" +
                    ",@@SERVERNAME AS [ServerName]" +
                    ", CASE when SERVERPROPERTY('IsClustered') = 1 THEN 'YES' " +
                    "ELSE 'NO' END AS [IsClustered]" +
                    ", SERVERPROPERTY('Edition') AS [Edition]" +
                    ", SERVERPROPERTY('ProductVersion') AS [ProductVersion]" +
                    ", Left(@@Version, Charindex('-', @@version) - 2) As VersionName" +
                    ", SERVERPROPERTY('ProductLevel') AS ProductLevel" +
                    ", SERVERPROPERTY('Collation') AS [Collation]" +
                    ", SERVERPROPERTY('ComputerNamePhysicalNetBIOS') AS [ComputerNamePhysicalNetBIOS]" +
                    ", SERVERPROPERTY('MachineName') AS [MachineName]" +
                    ", SERVERPROPERTY('InstanceName') AS [InstanceName]" +
                    ", SERVERPROPERTY('InstanceDefaultDataPath') AS [InstanceDefaultDataPath]" +
                    ", SERVERPROPERTY('InstanceDefaultLogPath') AS [InstanceDefaultLogPath]" +
                    ", SERVERPROPERTY('IsIntegratedSecurityOnly')  AS [IsIntegratedSecurityOnly]" +
                    ", SERVERPROPERTY('IsHadrEnabled') AS [IsHadrEnabled]" +
                    ", SERVERPROPERTY('HadrManagerStatus') AS [HadrManagerStatus]" +
                    ", @ha_nodes as [Nodes]" +
                    ", SERVERPROPERTY('IsXTPSupported') AS [IsXTPSupported]" +
                    ", create_date AS [SQL Server Install Date]" +
                    "FROM sys.server_principals " +
                    "WHERE name = N'NT AUTHORITY\\SYSTEM'" +
                    "OR name = N'NT AUTHORITY\\NETWORK SERVICE' OPTION(RECOMPILE);";

            using (DataTable da = SqlHelper.FillDataTable(ConnectionString, ServerinfoSQL, System.Data.CommandType.Text))
            {
                DataTable ServerInfoDataTable = new DataTable();

                SqlHelper.LoadDataTable(DWConnectionString, da, "collector.serverinfo");
                
            }


        }


        public static void CollectBackupStatus(string ConnectionString, string DWConnectionString, int CollectID)
        {

            string BackupStatusSQL = "SELECT @CollectID AS CollectID, GETDATE() AS CollectDate, @@SERVERNAME AS Servername, d.name DatabaseName, d.recovery_model_desc, (" +
                " SELECT MAX(backup_start_date) FROM msdb.dbo.backupset b  WHERE b.database_name = d.name AND type = 'D' ) AS 'LastFullBackup', " +
                "(SELECT MAX(backup_start_date) FROM msdb.dbo.backupset b  WHERE b.database_name = d.name AND type = 'I') AS 'LastDiffBackup', " +
                "(SELECT MAX(backup_start_date) FROM msdb.dbo.backupset b  WHERE b.database_name = d.name AND type = 'L') AS 'LastLogBackup' " +
                "FROM sys.databases d WHERE d.source_database_id IS NULL AND database_id NOT IN (SELECT database_id FROM sys.database_mirroring WHERE mirroring_role <> 1);";

            using (DataTable da = SqlHelper.FillDataTable(ConnectionString, BackupStatusSQL, System.Data.CommandType.Text))
            {
                DataTable ServerInfoDataTable = new DataTable();

                SqlHelper.LoadDataTable(DWConnectionString, da, "collector.serverinfo   ");
                
            }


            using (DataTable da = SqlHelper.FillDataTable(ConnectionString, BackupStatusSQL, System.Data.CommandType.Text))
            {
                DataTable ServerInfoDataTable = new DataTable();

                SqlHelper.LoadDataTable(DWConnectionString, da, "collector.backupstatus");

            }

        }
    }
}
