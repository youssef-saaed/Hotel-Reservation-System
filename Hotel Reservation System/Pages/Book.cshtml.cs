using Hotel_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Resources;

namespace Hotel_Reservation_System.Pages
{
    public class BookModel : PageModel
    {
        private DB db {  get; set; }
        [BindProperty(SupportsGet = true)]
        public int RoomNum {  get; set; }
        [BindProperty(SupportsGet = true)]
        public int FloorNum { get; set; }
        [BindProperty(SupportsGet = true)]
        public decimal Cost {  get; set; }
        [BindProperty(SupportsGet = true)]
        public string DateIn {  get; set; }
        [BindProperty(SupportsGet = true)]
        public string DateOut { get; set; }
        [BindProperty]
        public Reservation reservation { get; set; }
        public BookModel(DB db)
        {
            this.db = db;
        }
        public IActionResult? OnGet()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("username")) || HttpContext.Session.GetInt32("user_type") != 2)
            {
                return RedirectToPage("/SignIn");
            }
            return null;
        }
        public IActionResult OnPost()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("username")))
            {
                return RedirectToPage("/SignIn");
            }
            reservation.RoomNum = RoomNum;
            reservation.FloorNum = FloorNum;
            reservation.FromDate = DateIn;
            reservation.ToDate = DateOut;
            reservation.Cost = Cost;
            reservation.RemainingCost = Cost;
            db.MakeReservation(reservation, HttpContext.Session.GetString("username"));
            return RedirectToPage("/Index");
        }
    }
}
