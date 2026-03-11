using System.Configuration;
using System.Data.SqlClient;

namespace SDSExam_Colinares.DAL
{
    public static class DbHelper
    {
        public static SqlConnection GetConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["RecyclableDB"].ConnectionString;
            return new SqlConnection(connStr);
        }
    }
}