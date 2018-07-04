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
        public static int CollectorInit(string ConnectionString, string ServerName)
        {
            SqlParameter SQLSourceInstance = new SqlParameter("@ServerName", SqlDbType.VarChar);
            SQLSourceInstance.Value = ServerName;
            SqlParameter CollectID = new SqlParameter("@ID", SqlDbType.Int);
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
    }
}
