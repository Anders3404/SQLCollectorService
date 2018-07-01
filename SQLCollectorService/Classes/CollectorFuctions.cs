using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace SQLCollectorService.Classes
{
    class CollectorFuctions
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
    }
}
