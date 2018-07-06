using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.RegisteredServers;
using Microsoft.SqlServer.Management.Smo;
using System.Configuration;


namespace SQLCollectorService
{
    class Program
    {
        static void Main(string[] args)
        {
                       
            string DWConnectionStringClassic = ConfigurationManager.ConnectionStrings["Classic"].ToString();
            string DWConnectionStringPaaS = ConfigurationManager.ConnectionStrings["PaaS"].ToString();
            string DWConnectionString = "";
            string SourceConnectionString = "";

            if  (SQLCollectorService.Classes.SqlHelper.IsServerConnected(DWConnectionStringClassic)== true)
            {
                DWConnectionString = DWConnectionStringClassic;
            }
            else if (SQLCollectorService.Classes.SqlHelper.IsServerConnected(DWConnectionStringPaaS) == true)
            {
                DWConnectionString = DWConnectionStringPaaS;
            }

            System.Data.DataTable table2 = SmoApplication.EnumAvailableSqlServers(true);

            foreach (System.Data.DataRow row in table2.Rows)
            {
                SourceConnectionString= "Data Source=" + row["Name"].ToString() + "; Initial Catalog=master;Integrated Security=true;";
                int CollectID = SQLCollectorService.Classes.CollectorFuctions.CollectorInit(DWConnectionString, row["Name"].ToString());

                Console.WriteLine("CollectID for " + row["Name"].ToString() + " is: " + CollectID.ToString());

                SQLCollectorService.Classes.CollectorFuctions.CollectServerInfo(SourceConnectionString, DWConnectionString, CollectID);
                SQLCollectorService.Classes.CollectorFuctions.CollectBackupStatus(SourceConnectionString, DWConnectionString, CollectID);

                if (row["Version"].ToString().StartsWith("10.0")==true)
                {
                    Console.WriteLine("This is SQL Server 2008");
                }


                if (row["Version"].ToString().StartsWith("10.5") == true)
                {
                    Console.WriteLine("This is SQL Server 2008 R2");
                }


                if (row["Version"].ToString().StartsWith("11") == true)
                {
                    Console.WriteLine("This is SQL Server 2012");
                }


                if (row["Version"].ToString().StartsWith("12") == true)
                {
                    Console.WriteLine("This is SQL Server 2014");
                }


                if (row["Version"].ToString().StartsWith("13") == true)
                {
                    Console.WriteLine("This is SQL Server 2016");
                }


                if (row["Version"].ToString().StartsWith("14") == true)
                {
                    Console.WriteLine("This is SQL Server 2017");
                }
            }
            
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
