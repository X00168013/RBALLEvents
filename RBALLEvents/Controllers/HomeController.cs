using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using RBALLEvents.Models;
using RBALLEvents.Models.RacquetballViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using RBALLEvents.Data;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace RBALLEvents.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RacquetballContext _context;

        public HomeController(ILogger<HomeController> logger, RacquetballContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Location()
        {
            string markers = "[";
            string conString = "Server=(localdb)\\mssqllocaldb;Database=aspnet-RBALLEvents-AD9631E2-3D14-412E-AABF-A6B0AF911F04;Trusted_Connection=True;MultipleActiveResultSets=true";
            SqlCommand cmd = new SqlCommand("SELECT * FROM EventLocation");
            using (SqlConnection con = new SqlConnection(conString))
            {
                cmd.Connection = con;
                con.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        markers += "{";
                        markers += string.Format("'title': '{0}',", sdr["Address"]);
                        markers += string.Format("'lat': '{0}',", sdr["Latitude"]);
                        markers += string.Format("'lng': '{0}',", sdr["Longitude"]);
                        markers += string.Format("'description': '{0}'", sdr["Description"]);
                        markers += "},";
                    }
                }
                con.Close();
            }

            markers += "];";
            ViewBag.Markers = markers;
            return View();
        }


        /*public IActionResult Location()
        {
            string markers = "[";
            string CS = @"Server=(localdb)\\mssqllocaldb;Database=aspnet-RBALLEvents-AD9631E2-3D14-412E-AABF-A6B0AF911F04;Trusted_Connection=True;MultipleActiveResultSets=true";
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spGetMap", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    markers += "{";
                    markers += string.Format("'title': '{0}',", sdr["Address"]);
                    markers += string.Format("'lat': '{0}',", sdr["Latitude"]);
                    markers += string.Format("'lng': '{0}',", sdr["Longitude"]);
                    markers += string.Format("'description': '{0}'", sdr["Description"]);
                    markers += "},";
                }
            }
            markers += "];";
            ViewBag.Markers = markers;
            return View();
        }

        [HttpPost, ActionName("Location")]
        [ValidateAntiForgeryToken]
        public IActionResult Location(EventLocation location)
        {
            if (ModelState.IsValid)
            {
                string CS = @"Server=(localdb)\\mssqllocaldb;Database=aspnet-RBALLEvents-AD9631E2-3D14-412E-AABF-A6B0AF911F04;Trusted_Connection=True;MultipleActiveResultSets=true";
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("spAddNewLocation", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@CityName", location.Address);
                    cmd.Parameters.AddWithValue("@Latitude", location.Latitude);
                    cmd.Parameters.AddWithValue("@Longitude", location.Longitude);
                    cmd.Parameters.AddWithValue("@Description", location.Description);
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {

            }
            return RedirectToAction("Location");
        }*/
        public async Task<ActionResult> About()
        {
            IQueryable<GenderBreakdown> data =
                from member in _context.Members
                group member by member.Gender into genderGroup
                select new GenderBreakdown()
                {
                    Gender = genderGroup.Key,
                    MemberCount = genderGroup.Count()
                };
            return View(await data.AsNoTracking().ToListAsync());
        }

    }
}
