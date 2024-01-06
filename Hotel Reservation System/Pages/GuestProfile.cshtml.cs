using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Hotel_Reservation_System.Models;
using System.Dynamic;
using System.Reflection;
using System.Diagnostics.Metrics;
namespace Hotel_Reservation_System.Pages
{
    public class GuestProfileModel : PageModel
    {
        public string username { get; set; }
        private readonly ILogger<GuestProfileModel> _logger;

        public GuestProfileModel(ILogger<GuestProfileModel> logger)
        {
            _logger = logger;
                
        }

        public Models.userGuest user = new Models.userGuest();
        public Models.userOngoingRes res = new Models.userOngoingRes();
        public Models.DoneRes doneres = new Models.DoneRes();
        public List<userOngoingRes> Ongoing { get; set; } = new List<userOngoingRes>();
        public List<DoneRes> Done { get; set; } = new List<DoneRes>();
        public Models.userOngoingRes res2 { get; set; } = new Models.userOngoingRes();
        public int currentfeedres { get; set; } = 0;


        public void HandleGet()
        {
            RedirectToPage("/GuestProfile");
        }
        public IActionResult? OnGet()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("username")))
            {
                return RedirectToPage("/SignIn");
            }
            Ongoing = new List<userOngoingRes>();
            Done = new List<DoneRes>();
            username = HttpContext.Session.GetString("username");
        string ConString = "Data Source = YOUSEFs-LAPTOP\\SQLEXPRESS; Initial Catalog = HotelReservationSystemDB; Integrated Security = True";
            SqlConnection con = new SqlConnection(ConString);
            string queryUserInfo = $"SELECT First_Name, Last_Name, Contact_Email, Phone FROM Users WHERE UserName = '{username}'";
            string queryOngoingRes = $"SeLECT Room_Num ,Fromm, Too, Statee, Remaining_Cost, Cost, r.ID from Reservation r, Guest g, Users u WHERE u.UserName = '{username}' and g.National_ID = u.National_ID and r.Guest_Id = g.ID and Statee in ('Checked In' , 'Pending');";
            string queryDoneRes = $"SeLECT Room_Num ,Fromm, Too, Rating, r.ID from Reservation r  join Guest g on r.Guest_Id = g.ID join Users u on g.National_ID = u.National_ID left join Feedback f on r.ID = f.Reservation_ID Where u.UserName = '{username}' and Statee = 'Checked Out' ";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(queryUserInfo, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user.fname = reader[0].ToString();
                    user.lname = reader[1].ToString();
                    user.email = reader[2].ToString();
                    user.phone = reader[3].ToString();
                }

                reader.Close();

                SqlCommand cmd2 = new SqlCommand(queryOngoingRes, con);
                SqlDataReader reader2 = cmd2.ExecuteReader();
                while (reader2.Read())
                {
                    bool iis_paid;
                    int x = Convert.ToInt32(reader2[4]);
                    if (x == 0)
                    {
                        iis_paid = true;
                    }
                    else
                    {
                        iis_paid = false;
                    }
                    var res1 = new userOngoingRes
                    {
                        roomnum = Convert.ToInt32(reader2[0]),
                        date_from = reader2[1].ToString(),
                        date_to = reader2[2].ToString(),
                        res_state = reader2[3].ToString(),
                        price = Convert.ToInt32(reader2[5]),
                        is_paid = iis_paid,
                        reservationid = Convert.ToInt32(reader2[6]),
                    };
                    Ongoing.Add(res1);

                    Console.WriteLine(res2.reservationid);
                }

                foreach (var Res in Ongoing)
                {
                    Console.WriteLine(Res.roomnum);
                }
                reader2.Close();
                SqlCommand cmd3 = new SqlCommand(queryDoneRes, con);
                SqlDataReader reader3 = cmd3.ExecuteReader();
                while (reader3.Read())
                {
                    doneres.roomnum = Convert.ToInt32(reader3[0]);
                    doneres.date_from = reader3[1].ToString();
                    doneres.date_to = reader3[2].ToString();
                    doneres.rating = reader3.IsDBNull(3) ? 0 : Convert.ToInt32(reader3[3]);
                    doneres.reservationid = Convert.ToInt32(reader3[4]);
                    Done.Add(doneres);
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        public IActionResult OnPostDeleteAccount()
        {
            return OnPostSignOut();
            /*username = HttpContext.Session.GetString("username");

            try
            {
                string ConString = "Data Source = YOUSEFs-LAPTOP\\SQLEXPRESS; Initial Catalog = HotelReservationSystemDB; Integrated Security = True";
                SqlConnection con = new SqlConnection(ConString);
                con.Open();

                string getGuestIdQuery = $"SELECT ID FROM Guest WHERE National_ID = (SELECT National_Id FROM Users WHERE UserName = '{username}')";
                SqlCommand getGuestIdCmd = new SqlCommand(getGuestIdQuery, con);
                int guestId = (int)getGuestIdCmd.ExecuteScalar();

                // Delete reservations associated with the user
                string deleteReservationsQuery = $"DELETE FROM Reservation WHERE Guest_Id = {guestId}";
                SqlCommand deleteReservationsCmd = new SqlCommand(deleteReservationsQuery, con);
                deleteReservationsCmd.ExecuteNonQuery();

                string deleteQuery = $"DELETE FROM Users WHERE UserName = '{username}'";
                SqlCommand cmd = new SqlCommand(deleteQuery, con);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return RedirectToPage("/Index");*/
        }


        public IActionResult OnPostSignOut()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Phone { get; set; }
        public IActionResult OnPostEditAccount()
        {
            username = HttpContext.Session.GetString("username");
            try
            {
                string ConString = "Data Source = YOUSEFs-LAPTOP\\SQLEXPRESS; Initial Catalog = HotelReservationSystemDB; Integrated Security = True";
                SqlConnection con = new SqlConnection(ConString);

                con.Open();

                // Access form values directly from Request.Form
                string email = Request.Form["Email"];
                string phone = Request.Form["Phone"];


                string updateQuery = $"UPDATE Users SET Contact_Email = '{email}', Phone = '{phone}' WHERE UserName = '{username}'";
                Console.WriteLine(updateQuery);
                SqlCommand cmd = new SqlCommand(updateQuery, con);
                cmd.ExecuteNonQuery();

                return RedirectToPage("/GuestProfile", new { handleGet = true });
            }
            catch (SqlException ex)
            {
                Console.Write("test, the query didnt work");
                Console.WriteLine(ex.ToString());
                return RedirectToPage("/GuestProfile", new { handleGet = true });
            }
        }


        public IActionResult OnPostAddRequest()
        {
            username = HttpContext.Session.GetString("username");
            int reservationId = res2.reservationid;


            try
            {

                string description = Request.Form["Description"];
                string notes = Request.Form["Notes"];



                string ConString = "Data Source = YOUSEFs-LAPTOP\\SQLEXPRESS; Initial Catalog = HotelReservationSystemDB; Integrated Security = True";
                SqlConnection con = new SqlConnection(ConString);

                con.Open();
                string queryOngoingRes = $"SeLECT Room_Num ,Fromm, Too, Statee, Remaining_Cost, Cost, r.ID from Reservation r, Guest g, Users u WHERE u.UserName = '{username}' and g.National_ID = u.National_ID and r.Guest_Id = g.ID and Statee in ('Checked In' , 'Pending');";

                SqlCommand cmd = new SqlCommand(queryOngoingRes, con);
                SqlDataReader reader2 = cmd.ExecuteReader();
                while (reader2.Read())
                {
                    bool iis_paid;
                    int x = Convert.ToInt32(reader2[4]);
                    if (x == 0)
                    {
                        iis_paid = true;
                    }
                    else
                    {
                        iis_paid = false;
                    }
                    var res1 = new userOngoingRes
                    {
                        roomnum = Convert.ToInt32(reader2[0]),
                        date_from = reader2[1].ToString(),
                        date_to = reader2[2].ToString(),
                        res_state = reader2[3].ToString(),
                        price = Convert.ToInt32(reader2[5]),
                        is_paid = iis_paid,
                        reservationid = Convert.ToInt32(reader2[6]),
                    };
                    reservationId = res1.reservationid;
                }
                reader2.Close();

                Console.WriteLine("ResID: " + reservationId);
                Console.WriteLine("Desc: " + description);
                Console.WriteLine("Notes: " + notes);
                string updateQuery = $"INSERT INTO Request( Cost,Request_Description, State, Notes, Housekeeping_ID, Reservation_ID) VALUES ( 0,'{description}', 'Sent', '{notes}', NULL, {reservationId})";
                SqlCommand cmd2 = new SqlCommand(updateQuery, con);
                cmd2.ExecuteNonQuery();
                return RedirectToPage("/GuestProfile", new { handleGet = true });
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                return RedirectToPage("/GuestProfile", new { handleGet = true });

            }
        }


        [BindProperty]
        public string feedbackDescription { get; set; }

        public IActionResult OnPostAddFeedback()
        {
            username = HttpContext.Session.GetString("username");
            try
            {
                Int16 reservationId = Convert.ToInt16(Request.Form["newreservationid"]);
                string feedback = Request.Form["newfeedback"];
                int rate = Convert.ToInt32(Request.Form["newrate"]);
                Console.WriteLine("Reservation ID: " + reservationId);
                Console.WriteLine("Edited Feedback Description: " + feedback);
                Console.WriteLine("Edited Rate: " + rate);
                string ConString = "Data Source = YOUSEFs-LAPTOP\\SQLEXPRESS; Initial Catalog = HotelReservationSystemDB; Integrated Security = True";
                SqlConnection con = new SqlConnection(ConString);
                con.Open();


                string queryaddFeedback = $"INSERT INTO Feedback (Review, Rating, Reservation_ID, Feedback_Date) VALUES ('{feedback}', {rate}, {reservationId}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";
                SqlCommand cmd2 = new SqlCommand(queryaddFeedback, con);
                cmd2.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return RedirectToPage("/GuestProfile", new { handleGet = true });

        }

        public IActionResult OnPostEditFeedback()
        {
            username = HttpContext.Session.GetString("username");
            try
            {

                Int16 reservationId = Convert.ToInt16(Request.Form["editedreservationId"]);
                string editedfeedbackDescription = Request.Form["editedfeedbackDescription"];
                int edittedrate = Convert.ToInt32(Request.Form["edittedrate"]);
                Console.WriteLine("Reservation ID: " + reservationId);
                Console.WriteLine("Edited Feedback Description: " + editedfeedbackDescription);
                Console.WriteLine("Edited Rate: " + edittedrate);
                string ConString = "Data Source = YOUSEFs-LAPTOP\\SQLEXPRESS; Initial Catalog = HotelReservationSystemDB; Integrated Security = True";
                SqlConnection con = new SqlConnection(ConString);
                con.Open();
                string queryUpdateFeedback = $"UPDATE Feedback SET Review = '{editedfeedbackDescription}', Rating = {edittedrate} WHERE Reservation_ID = {reservationId}";

                SqlCommand cmd2 = new SqlCommand(queryUpdateFeedback, con);
                cmd2.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return RedirectToPage("/GuestProfile", new { handleGet = true });

        }

        public IActionResult OnPostPayement()
        {
            username = HttpContext.Session.GetString("username");
            try
            {
                Int16 reservationId = Convert.ToInt16(Request.Form["payresid"]);
                string paymenttype = Request.Form["paymentType"];
                Int32 resprice = Convert.ToInt32(Request.Form["payprice"]);

                string ConString = "Data Source = YOUSEFs-LAPTOP\\SQLEXPRESS; Initial Catalog = HotelReservationSystemDB; Integrated Security = True";
                using (SqlConnection con = new SqlConnection(ConString))
                {
                    con.Open();

                    // Step 1: Insert payment record
                    string queryAddPayment = $"INSERT INTO Payment (Amount, Payment_Type, Payment_Date) VALUES ({resprice}, '{paymenttype}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";
                    SqlCommand cmdAddPayment = new SqlCommand(queryAddPayment, con);
                    cmdAddPayment.ExecuteNonQuery();

                    // Step 2: Get the newly inserted payment ID
                    string queryGetPaymentId = "SELECT SCOPE_IDENTITY()";
                    SqlCommand cmdGetPaymentId = new SqlCommand(queryGetPaymentId, con);
                    Int32 paymentId = Convert.ToInt32(cmdGetPaymentId.ExecuteScalar());

                    // Step 3: Update reservation record
                    string queryUpdateReservation = $"UPDATE Reservation SET Remaining_Cost = 0, Payment_Id = {paymentId} WHERE Reservation.ID = {reservationId}";
                    SqlCommand cmdUpdateReservation = new SqlCommand(queryUpdateReservation, con);
                    cmdUpdateReservation.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return RedirectToPage("/GuestProfile", new { handleGet = true });
        }

    }
}
