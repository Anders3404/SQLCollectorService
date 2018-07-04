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


namespace SQLCollectorService
{
    class Program
    {
        static void Main(string[] args)
        {
            //RegisteredServerCollection registeredServers = new RegisteredServerCollection; 
            //System.Data.Sql.SqlDataSourceEnumerator instance = System.Data.Sql.SqlDataSourceEnumerator.Instance;

            string DWConnectionString = "Data Source=.; Initial Catalog=IDBMGMT;Integrated Security=true;";
            string SourceConnectionString = "";
            System.Data.DataTable table2 = SmoApplication.EnumAvailableSqlServers(true);

            foreach (System.Data.DataRow row in table2.Rows)
            {
                SourceConnectionString= "Data Source=" + row["Name"].ToString() + "; Initial Catalog=IDBMGMT;Integrated Security=true;";
                int CollectID = SQLCollectorService.Classes.CollectorFuctions.CollectorInit(DWConnectionString, row["Name"].ToString());

                Console.WriteLine("CollectID for " + row["Name"].ToString() + " is: " + CollectID.ToString());

                SQLCollectorService.Classes.CollectorFuctions.CollectServerInfo(SourceConnectionString, DWConnectionString, CollectID);
                
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
