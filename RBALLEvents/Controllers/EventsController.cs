using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RBALLEvents.Data;
using RBALLEvents.Models;
using RBALLEvents.Models.RacquetballViewModels;

namespace RBALLEvents.Controllers
{
    public class EventsController : Controller
    {
        private readonly RacquetballContext _context;
        private readonly string appErrorPath = "Views/Shared/AppError.cshtml";

        public EventsController(RacquetballContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index(int? eventID)
        {
            //var racquetballContext = _context.Events.Include(c => c.Club);
            //return View(await racquetballContext.ToListAsync());


            var viewModel = new EventIndexView();
            viewModel.Events = await _context.Events
                .Include(i => i.Club)
                        .Include(i => i.Registrations)
                            .ThenInclude(i => i.Member)
                  .AsNoTracking()
                  .OrderBy(i => i.EventName)
                  .ToListAsync();


            //Registrations Display
            if (eventID != null)
            {
                ViewData["EventID"] = eventID.Value;
                viewModel.Registrations = viewModel.Events.Where(
                x => x.RacquetballEventID == eventID).Single().Registrations;

            }
            return View(viewModel);

        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            ViewData["ErrorMessage"] = "An error ocurred trying to get the event details. Please try again.";
            if (id == null)
            {
                return View(appErrorPath);
            }

            var @event = await _context.Events
            .Include(c => c.Club)
            .FirstOrDefaultAsync(m => m.RacquetballEventID == id);
            if (@event == null)
            {
                return View(appErrorPath);
            }

            // Clear error message
            ViewData["ErrorMessage"] = "";

            return View(@event);
        }


        // GET: Events/Create
        public IActionResult Create()
        {
            //ViewData["ClubID"] = new SelectList(_context.Clubs, "ClubID", "ClubID");
            PopulateClubsDropDownList();
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RacquetballEventID,EventName,EventDetails,EventDate,EventType,PostedDate,ClubID")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ClubID"] = new SelectList(_context.Clubs, "ClubID", "ClubID", @event.ClubID);
            PopulateClubsDropDownList(@event.ClubID);
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.AsNoTracking().FirstOrDefaultAsync(m => m.RacquetballEventID == id);

            if (@event == null)
            {
                return NotFound();
            }
            //ViewData["ClubID"] = new SelectList(_context.Clubs, "ClubID", "ClubID", @event.ClubID);
            PopulateClubsDropDownList(@event.ClubID);
            return View(@event);
        }


        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventToUpdate = await _context.Events
                .FirstOrDefaultAsync(c => c.RacquetballEventID == id);

            if (await TryUpdateModelAsync<Event>(eventToUpdate,
                "",
                c => c.EventName, c => c.ClubID, c => c.EventDetails, c => c.EventDate, c => c.EventType))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateClubsDropDownList(eventToUpdate.ClubID);
            return View(eventToUpdate);

        }

        private void PopulateClubsDropDownList(object selectedClub = null)
        {
            var clubsQuery = from d in _context.Clubs
                             orderby d.Name
                             select d;
            ViewBag.ClubID = new SelectList(clubsQuery.AsNoTracking(), "ClubID", "Name", selectedClub);

        }


        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(c => c.Club)
                .FirstOrDefaultAsync(m => m.RacquetballEventID == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
 
        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.RacquetballEventID == id);
        }
    }
}
