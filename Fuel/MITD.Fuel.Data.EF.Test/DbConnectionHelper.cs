using System.Configuration;

namespace MITD.Fuel.Data.EF.Test
{
    public static class DbConnectionHelper
    {
        public static string GetConnectionString()
        {
            var conn =ConfigurationManager.ConnectionStrings["DataContainer"];
            return conn!=null?conn.ConnectionString:string.Empty;
        }
    }
}