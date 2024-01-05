using System.Data.SqlClient;

namespace Hotel_Reservation_System.Models
{
    public class DB
    {
        private string connString = "Data Source = YOUSEFs-LAPTOP\\SQLEXPRESS; Initial Catalog = HotelReservationDBS; Integrated Security = True";
        private SqlConnection conn = new SqlConnection();

        public DB()
        {
            conn.ConnectionString = connString;
        }
    }
}
