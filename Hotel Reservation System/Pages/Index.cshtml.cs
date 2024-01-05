using Hotel_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel_Reservation_System.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public (decimal min, decimal max) cost { get; set; }
        private DB db {  get; set; }
        public IndexModel(DB db, ILogger<IndexModel> logger)
        {
            _logger = logger;
            this.db = db;
            cost = db.GetMinMaxRoomPrice();
        }

        public void OnGet()
        {

        }
    }
}
