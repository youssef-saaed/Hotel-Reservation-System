using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Hotel_Reservation_System.Models
{
    public class DB
    {
        private string connString = "Data Source = YOUSEFs-LAPTOP\\SQLEXPRESS; Initial Catalog = HotelReservationSystemDB; Integrated Security = True";
        private SqlConnection conn = new SqlConnection();

        public DB()
        {
            conn.ConnectionString = connString;
        }

        public Room[] GetRooms(string DateIn, string DateOut, int Guests, int Min, int Max, int Order, bool Room, bool FSuite, bool VSuite, bool HApr, bool Vila)
        {
            string types = "";
            if (Room)
            {
                types += "1,";
            }
            if (FSuite)
            {
                types += "2,";
            }
            if (VSuite)
            {
                types += "3,";
            }
            if (HApr)
            {
                types += "4,";
            }
            if (Vila)
            {
                types += "5,";
            }
            if (!Room && !FSuite && !VSuite && !HApr && !Vila)
            {
                types = "1,2,3,4,5,";
            }
            string query = $"SELECT r.Floor_Num, r.Room_Num, r.Capacity_Details, r.Type_Details, r.Cost, r.Descriptionn, r.Properties FROM Room r LEFT JOIN Reservation res ON r.Room_Num = res.Room_Num AND r.Floor_Num = res.Floor_Num AND (('{DateIn}' < res.Fromm AND '{DateOut}' <= res.Fromm) OR ('{DateIn}' >= res.Too AND '{DateOut}' > res.Too)) WHERE Type_Details in ({types.Substring(0, types.Length - 1)}) and r.Cost BETWEEN {Min} and {Max} and r.Capacity_Details = {Guests} ORDER BY Cost";
            if (Order == 1)
            {
                query += " DESC";
            }
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                conn.Open();
                dt.Load(cmd.ExecuteReader());
            }
            catch (SqlException err)
            {
                Console.WriteLine(err.Message);
            }
            finally
            {
                conn.Close();
            }

            Room[] rooms = new Room[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                rooms[i] = new Room();
                rooms[i].FloorNum = (int)dt.Rows[i]["Floor_Num"];
                rooms[i].RoomNum = (int)dt.Rows[i]["Room_Num"];
                rooms[i].Capacity = (int)dt.Rows[i]["Capacity_Details"];
                rooms[i].Type = (int)dt.Rows[i]["Type_Details"];
                rooms[i].Cost = (decimal)dt.Rows[i]["Cost"];
                rooms[i].Description = (string)dt.Rows[i]["Descriptionn"];
                rooms[i].Properties = (string)dt.Rows[i]["Properties"];
            }
            return rooms;
        }
        public (decimal min, decimal max) GetMinMaxRoomPrice()
        {
            string query = "SELECT MIN(Cost) Min, MAX(Cost) Max FROM Room";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            (decimal, decimal) cost = ((decimal)dt.Rows[0]["Min"], (decimal)dt.Rows[0]["Max"]);
            conn.Close();
            return cost;
        }
    }
}
