using System.Data.SqlClient;

namespace OgrenciKursTakipSistemi.Database
{
    public static class DbConnection
    {
        private static string connectionString =
            "Server=localhost;Database=OgrenciKursDB;Trusted_Connection=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
