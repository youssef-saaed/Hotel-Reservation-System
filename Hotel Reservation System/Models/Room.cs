namespace Hotel_Reservation_System.Models
{
    public class Room
    {
        public int FloorNum { get; set; }
        public int RoomNum { get; set; }
        public int State { get; set; }
        public decimal Cost { get; set; }
        public int Capacity {  get; set; }
        public int Type {  get; set; }
        public string? Description { get; set; }
        public string? Properties { get; set; }
    }
}
