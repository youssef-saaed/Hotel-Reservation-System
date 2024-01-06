namespace Hotel_Reservation_System.Models
{
    public class Reservation
    {
        public int RoomNum { get; set;}
        public int FloorNum { get; set;}
        public int Discount { get; set;}
        public string FromDate {  get; set;}
        public string ToDate { get; set;}
        public decimal Cost {  get; set;}
        public decimal RemainingCost { get; set; }
        public string Prefrences {  get; set;}
        public int GuestId {  get; set;}

    }
}
