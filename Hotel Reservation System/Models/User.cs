using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Hotel_Reservation_System.Models
{
    public class User
    {
        public long NID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public byte Gender { get; set; }
        public byte UserType { get; set; }
        public byte NumberOfRedflags { get; set; }
        public long Phone {  get; set; }
        public string Username {  get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
