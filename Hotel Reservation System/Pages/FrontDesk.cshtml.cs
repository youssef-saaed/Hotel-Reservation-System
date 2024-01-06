using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using Hotel_Reservation_System.Models;

namespace Hotel_Reservation_System.Pages
{
    public class FrontDeskModel : PageModel
    {

        private readonly DB db;
        //for Add Room
        [BindProperty(SupportsGet = true)]
        public int resID { get; set; }

        [BindProperty(SupportsGet = true)]
        public int floorNum { get; set; }

        [BindProperty(SupportsGet = true)]
        public int roomNum { get; set; }

        [BindProperty(SupportsGet = true)]
        public int state { get; set; }

        [BindProperty(SupportsGet = true)]
        public float cost { get; set; }

        [BindProperty(SupportsGet = true)]
        public int capacity { get; set; }

        [BindProperty(SupportsGet = true)]
        public int type { get; set; }

        [BindProperty(SupportsGet = true)]
        public string description { get; set; }

        [BindProperty(SupportsGet = true)]
        public string properties { get; set; }

        [BindProperty(SupportsGet = true)]
        public int ffloorNum { get; set; }

        [BindProperty(SupportsGet = true)]
        public int rroomNum { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Action_Description { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Action_Date { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Housekeeping_ID { get; set; }

        [BindProperty(SupportsGet = true)]
        public int FeedbackId { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool sent { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool started { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool closed { get; set; }
        public DataTable dt1 { get; set; }
        public DataTable dt2 { get; set; }
        public DataTable dt3 { get; set; }
        public DataTable dt4 { get; set; }
        public DataTable dt5 { get; set; }

        public FrontDeskModel(DB db)
        {
            this.db = db;
        }
        public void OnGet()
        {
            dt1 = db.RequestInformation();
            dt2 = db.FilterSent();
            dt3 = db.FilterStarted();
            dt4 = db.FilterClosed();
            /*  dt2 = db.Filter(sent, started, closed);*/
            dt5 = db.FeedbackInformation();
        }

        public IActionResult OnPostOne()
        {
            db.AddRoom(resID, floorNum, roomNum, state, cost, capacity, type, description, properties);
            resID = 0;
            floorNum = 0;
            roomNum = 0;
            state = 0;
            cost = 0;
            capacity = 0;
            type = 0;
            description = "";
            properties = "";
            return RedirectToPage("/FrontDesk");
        }
        public IActionResult OnPostTwo()
        {
            db.DeleteRoom(ffloorNum, rroomNum);
            ffloorNum = 0;
            rroomNum = 0;
            return RedirectToPage("/FrontDesk");
        }
        public IActionResult OnPostThree()
        {
            db.Assign();
            return RedirectToPage("/FrontDesk");
        }
        public IActionResult OnPostFour()
        {
            db.markAsClosed();
            return RedirectToPage("/FrontDesk");
        }
        public IActionResult OnPostFive()
        {
            db.AddAction(Action_Description, Action_Date, Housekeeping_ID, FeedbackId);
            Action_Description = "";
            Action_Date = "";
            Housekeeping_ID = 0;
            FeedbackId = 0;
            return RedirectToPage("/FrontDesk");
        }
        public IActionResult OnPostSix()
        {
            db.markAsClosed();
            return RedirectToPage("/FrontDesk");
        }
    }
}
