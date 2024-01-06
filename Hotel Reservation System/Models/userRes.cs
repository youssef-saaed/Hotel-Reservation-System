namespace Hotel_Reservation_System.Models
{
    public class userOngoingRes
    {
        public int roomnum;
        public int price;
        public string date_from = "ew";
        public string date_to = "ds";
        public string res_state = "sd";
        public bool is_paid;
        public int reservationid;

    }

    public class DoneRes
    {
        public int roomnum;
        public string date_from = "ds";
        public string date_to = "ds";
        public int rating;
        public int reservationid;
    }
}
