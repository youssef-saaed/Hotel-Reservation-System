using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Hotel_Reservation_System.Models
{
    public class DB
    {
        public enum SignUpState { SUCCESS, EMPTY_FIELD_OR_INVALID, USERNAME_OR_NID_EXIST, WEAK_PASSWORD, FAILED, NOT_SIGNED_UP};
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

        public User? GetUser(string username, string password)
        {
            string query = $"SELECT * FROM Users WHERE UserName = '{username}' AND Pass_Word = '{password}'";
            User user = new User();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            dt.Load(cmd.ExecuteReader());
            if (dt.Rows.Count == 0)
            {
                conn.Close();
                return null;
            }
            user.Username = (string)dt.Rows[0]["UserName"];
            if (dt.Rows[0]["Contact_Email"] != System.DBNull.Value)
            {
                user.Email = (string)dt.Rows[0]["Contact_Email"];
            }
            user.FName = (string)dt.Rows[0]["First_Name"];
            user.LName = (string)dt.Rows[0]["Last_Name"];
            user.NID = (long)dt.Rows[0]["National_Id"];
            if (dt.Rows[0]["Gender"] == "Male")
            {
                user.Gender = 1;
            }
            else
            {
                user.Gender = 2;
            }
            if (dt.Rows[0]["User_Type"] == "Employee")
            {
                user.UserType = 1;
            }
            else
            {
                user.UserType = 2;
            }
            user.NumberOfRedflags = Convert.ToByte(dt.Rows[0]["Num_Of_RedFlags"]);
            user.Phone = Convert.ToInt64(dt.Rows[0]["Phone"]);
            conn.Close();
            return user;
        }
        public SignUpState SignUp(User user)
        {
            if (user.FName == "" || user.LName == "" || user.Username == "" || user.Email == "" || user.Password == "" || !(user.Gender == 1 || user.Gender == 2) || user.Phone < 1000000000 || user.NID < 10000000000000)
            {
                return SignUpState.EMPTY_FIELD_OR_INVALID;
            }
            Regex reg = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            if (!reg.IsMatch(user.Password))
            {
                return SignUpState.WEAK_PASSWORD;
            }
            string query = $"SELECT COUNT(*) FROM Users WHERE UserName = '{user.Username}' OR National_Id = {user.NID} OR Phone = '{user.Phone}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            if ((int)cmd.ExecuteScalar() > 0)
            {
                conn.Close();
                return SignUpState.USERNAME_OR_NID_EXIST;
            }
            string Gender = (user.Gender == 1) ? "Male" : "Female";
            query = $"INSERT INTO Users(National_Id, First_Name, Last_Name, Contact_Email, Gender, User_Type, Num_Of_RedFlags, Phone, UserName, Pass_Word) Values ({user.NID}, '{user.FName}', '{user.LName}', '{user.Email}', '{Gender}', 'Guest', 0, '{user.Phone}', '{user.Username}', '{user.Password}')";
            cmd = new SqlCommand(query, conn);
            if (cmd.ExecuteNonQuery() > 0)
            {
                query = $"INSERT INTO Guest(National_ID) VALUES ({user.NID})";
                cmd = new SqlCommand(query, conn);
                while (cmd.ExecuteNonQuery() < 1)
                {
                    cmd = new SqlCommand(query, conn);
                }
            }
            else
            {
                conn.Close();
                return SignUpState.FAILED;
            }
            conn.Close();
            return SignUpState.SUCCESS;
        }
        public void MakeReservation(Reservation reservation, string username)
        {
            string query = $"SELECT Guest.ID FROM Guest JOIN Users ON Users.National_Id = Guest.National_ID WHERE UserName = '{username}'";
            conn.Open();
            SqlCommand cmd = new SqlCommand (query, conn);
            reservation.GuestId = Convert.ToInt32(cmd.ExecuteScalar());
            query = $"INSERT INTO Reservation(Floor_Num, Room_Num, Discount, Fromm, Too, date, Cost, Remaining_Cost, Statee, Prefrences, Guest_Id) VALUES ({reservation.FloorNum}, {reservation.RoomNum}, {reservation.Discount}, '{reservation.FromDate}', '{reservation.ToDate}', GETDATE(), {reservation.Cost}, {reservation.RemainingCost}, 'Pending', '{reservation.Prefrences.Replace("'", "\"")}', {reservation.GuestId})";
            cmd = new SqlCommand (query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
