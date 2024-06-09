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
        //for Add Room Button
        [BindProperty(SupportsGet = true)]
        public int Reservation_Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Floor_Num { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Room_Num { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Statte { get; set; }

        [BindProperty(SupportsGet = true)]
        public float Cost { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Capacity_Details { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Type_Details { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Descriptionn { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Properties { get; set; }

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
        public DataTable dt1 { get; set; }
        public DataTable dt2 { get; set; }
        public DataTable dt3 { get; set; }
        public DataTable dt4 { get; set; }
        public DataTable dt5 { get; set; }
        public DataTable dt6 { get; set; }
        public DataTable dt7 { get; set; }
        public FrontDeskModel(DB db)
        {
            this.db = db;
        }
        public void OnGet()
        {
            dt1 = db.RequestInformation();
            dt3 = db.FilterSent();
            dt4 = db.FilterStarted();
            dt5 = db.FilterClosed();
            dt2 = db.FeedbackInformation();
            dt6 = db.FilterActionTaken();
            dt7 = db.FilterActionNotTaken();
        }
        public IActionResult OnPostOne()
        {
            db.AddRoom(Reservation_Id, Floor_Num, Room_Num, Statte, Cost, Capacity_Details, Type_Details, Descriptionn, Properties);
            Reservation_Id = 0;
            Floor_Num = 0;
            Room_Num = 0;
            Statte = 0;
            Cost = 0;
            Capacity_Details = 0;
            Type_Details = 0;
            Descriptionn = "";
            Properties = "";
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
            db.AddAction(Action_Description, Action_Date, Housekeeping_ID, FeedbackId);
            Action_Description = "";
            Action_Date = "";
            Housekeeping_ID = 0;
            FeedbackId = 0;
            return RedirectToPage("/FrontDesk");
        }
        public void OnPostFour()
        {
            db.Assign();
        }
        public IActionResult OnPostFive()
        {
            db.markAsClosed();
            return RedirectToPage("/FrontDesk");
        }

    }
}
