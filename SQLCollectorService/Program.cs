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

            string ConnectionString = "Data Source=.; Initial Catalog=IDBMGMT;Integrated Security=true;";

            System.Data.DataTable table2 = SmoApplication.EnumAvailableSqlServers(true);

            foreach (System.Data.DataRow row in table2.Rows)
            {
                int CollectID = SQLCollectorService.Classes.CollectorFuctions.CollectorInit(ConnectionString,row["Name"].ToString());

                Console.WriteLine("CollectID for " + row["Name"].ToString() + " is: " + CollectID.ToString());
            }
            //EnumAvailableSqlServers
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
