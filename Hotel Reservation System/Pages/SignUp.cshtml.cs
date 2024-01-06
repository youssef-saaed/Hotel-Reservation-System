using Hotel_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel_Reservation_System.Pages
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public User user { get; set; }
        private DB db {  get; set; }
        [BindProperty(SupportsGet = true)]
        public string msg {  get; set; }
        public SignUpModel(DB db)
        { 
            this.db = db;
            this.msg = "";
        }

        public IActionResult? OnGet()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("username")))
            {
                return RedirectToPage("/Index");
            }
            return null;
        }

        public IActionResult OnPost()
        {
            DB.SignUpState s;
            s = db.SignUp(user);
            if (s == DB.SignUpState.USERNAME_OR_NID_EXIST)
            {
                return RedirectToPage("/SignUp", new { msg = "Username, phone number or national id already exists" });
            }
            else if (s == DB.SignUpState.WEAK_PASSWORD)
            {
                return RedirectToPage("/SignUp", new { msg = "Try to choose a stronger password" });
            }
            else if (s == DB.SignUpState.FAILED)
            {
                return RedirectToPage("/SignUp", new { msg = "Something went wrong please try again" });
            }
            else if (s == DB.SignUpState.EMPTY_FIELD_OR_INVALID)
            {
                return RedirectToPage("/SignUp", new { msg = "Don't leave empty field and check that you entered valid data" });
            }
            else
            {
                msg = "";
                HttpContext.Session.SetString("username", user.Username);
                return RedirectToPage("/Index");
            }
        }
    }
}
