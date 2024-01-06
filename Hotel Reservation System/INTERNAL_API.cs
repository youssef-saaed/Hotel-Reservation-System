using Hotel_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hotel_Reservation_System
{
    public class RoomFetcherController : Controller
    {
        private DB db { get; set; }
        public RoomFetcherController(DB db)
        {
            this.db = db;
        }

        [Route("/FetchRooms/{DateIn}/{DateOut}/{Guests}/{Min}/{Max}/{Order}/{Room}/{FSuite}/{VSuite}/{HApr}/{Vila}")]
        public IActionResult FetchRooms(string DateIn, string DateOut, int Guests, int Min, int Max, int Order, bool Room, bool FSuite, bool VSuite, bool HApr, bool Vila)
        {
            Room[] FilteredRooms = db.GetRooms(DateIn, DateOut, Guests, Min, Max, Order, Room, FSuite, VSuite, HApr, Vila);
            JsonArray fetched = new JsonArray();
            for (int i = 0; i < FilteredRooms.Length; i++)
            {
                fetched.Add(FilteredRooms[i]);
            }
            return new JsonResult(fetched);
        }
        [Route("/Book/{RoomNumber}/{FloorNumber}/{DateIn}/{DateOut}/{Cost}")]
        public IActionResult BookingPage(int RoomNumber, int FloorNumber, string DateIn, string DateOut, decimal Cost)
        {
            return RedirectToPage("/Book", new { RoomNum = RoomNumber, FloorNum = FloorNumber, DateIn = DateIn, DateOut = DateOut, Cost = Cost});
        }
    }
}
