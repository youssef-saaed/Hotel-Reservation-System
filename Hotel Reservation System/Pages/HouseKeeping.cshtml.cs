using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using Hotel_Reservation_System.Models;

namespace Hotel_Reservation_System.Pages
{
    public class HouseKeepingModel : PageModel
    {
        private readonly DB db;
        public DataTable dt6 { get; set; }
        public DataTable dt7 { get; set; }
        public HouseKeepingModel(DB db)
        {
            this.db = db;
        }
        public void OnGet()
        {
            dt7 = db.RequestInfromationForHKs();
        }
        public IActionResult OnPostone()
        {
            db.HK_MarkAsDone();
            return RedirectToPage("/HouseKeeping");
        }
    }
}
