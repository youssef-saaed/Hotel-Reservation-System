using Hotel_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Reservation_System.Pages
{
    public class SignInModel : PageModel
    {
        [BindProperty]
        [DataType(DataType.Text)]
        public string username { get; set; }
        [BindProperty]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool error { get; set; }
        public DB db {  get; set; }
        public SignInModel(DB db)
        {
            this.db = db;
            this.error = false;
        }

        public IActionResult? OnGet()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("username")))
            {
                return RedirectToPage("/Index");
            }
            return null;
        }
        public IActionResult OnPost()
        {
            User? user = db.GetUser(username, password);
            if (user == null)
            {
                return RedirectToPage("/SignIn", new { error = true });
            }
            else
            {
                error = false;
                HttpContext.Session.SetString("username", username);
                HttpContext.Session.SetInt32("user_type", user.UserType);
                if (user.UserType == 2) {
                    return RedirectToPage("/Index");
                }
                return RedirectToPage();
            }
        }
    }
}
