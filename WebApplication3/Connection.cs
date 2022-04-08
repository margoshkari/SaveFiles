using System.Data.SqlClient;

namespace WebApplication3
{
    public class Connection
    {
        private static SqlConnection connection;
        private Connection() { }
        public static SqlConnection GetConnection()
        {
            if (connection == null)
            {
                connection = new SqlConnection("Data Source=SQL8001.site4now.net;Initial Catalog=db_a8549a_ipaddres;User Id=db_a8549a_ipaddres_admin;Password=qwerty123");
                connection.Open();
            }
            return connection;
        }
    }
}
