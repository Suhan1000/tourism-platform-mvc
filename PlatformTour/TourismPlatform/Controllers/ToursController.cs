using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourismPlatform.Data;
using TourismPlatform.Models;

namespace TourismPlatform.Controllers
{
    [Authorize]
    public class ToursController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ToursController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Tours
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                // Anonymous users see all tours, can't create
                var publicTours = await _context.Tours.Include(t => t.Agency).Where(t => t.IsActive).ToListAsync();
                ViewBag.UserHasAgency = false;
                return View(publicTours);
            }

            // Check if user is a tourist first
            var tourist = await _context.Tourists.FirstOrDefaultAsync(t => t.UserId == user.Id);
            if (tourist != null)
            {
                // Tourist: Show all tours, can book but can't create
                var allTours = await _context.Tours.Include(t => t.Agency).Where(t => t.IsActive).ToListAsync();
                ViewBag.UserHasAgency = false;
                ViewBag.IsTourist = true;
                return View(allTours);
            }

            // Check if user is agency/guide
            var agency = await _context.Agencies.FirstOrDefaultAsync(a => a.UserId == user.Id);
            if (agency != null)
            {
                // Agency/Guide: Show only their tours, can create/edit
                var myTours = await _context.Tours
                    .Where(t => t.AgencyId == agency.Id)
                    .Include(t => t.Agency)
                    .ToListAsync();
                ViewBag.UserHasAgency = true;
                ViewBag.IsTourist = false;
                return View(myTours);
            }

            // No profile found
            ViewBag.UserHasAgency = false;
            return View(new List<Tour>());
        }
        // GET: Tours/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tours/Create
      
[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    [Bind("Name,Description,Destination,Duration,Price,MaxParticipants")] Tour tour,
    IFormFile? ImageFile) // Accept the uploaded image
        {
            System.Diagnostics.Debug.WriteLine("Create method called");

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var agency = await _context.Agencies.FirstOrDefaultAsync(a => a.UserId == user.Id);

                    if (agency != null)
                    {
                        tour.AgencyId = agency.Id;
                        tour.CreatedDate = DateTime.Now;
                        tour.IsActive = true; // Active by default

                        // Handle image upload
                        if (ImageFile != null && ImageFile.Length > 0)
                        {
                            // Generate unique file name
                            var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);

                            // Save path: wwwroot/images/tours/
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/tours", fileName);

                            // Ensure directory exists
                            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await ImageFile.CopyToAsync(stream);
                            }

                            // Store relative path in DB
                            tour.ImagePath = "/images/tours/" + fileName;
                        }

                        System.Diagnostics.Debug.WriteLine($"Saving tour: {tour.Name}, AgencyId: {tour.AgencyId}, ImagePath: {tour.ImagePath}");

                        _context.Add(tour);
                        await _context.SaveChangesAsync();

                        System.Diagnostics.Debug.WriteLine("Tour saved successfully");
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            return View(tour);
        }


        // GET: Tours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var tour = await _context.Tours
                .Include(t => t.Agency)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tour == null) return NotFound();

            return View(tour);
        }

        // GET: Tours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tour = await _context.Tours.FindAsync(id);
            if (tour == null) return NotFound();

            return View(tour);
        }
        public async Task<IActionResult> DebugTours()
        {
            var allTours = await _context.Tours.Include(t => t.Agency).ToListAsync();
            var user = await _userManager.GetUserAsync(User);
            var agency = await _context.Agencies.FirstOrDefaultAsync(a => a.UserId == user.Id);

            var debugInfo = $"Total tours in DB: {allTours.Count}\n";
            debugInfo += $"Current user: {user?.Email}\n";
            debugInfo += $"User has agency: {agency != null}\n";
            debugInfo += $"Agency ID: {agency?.Id}\n";

            foreach (var tour in allTours)
            {
                debugInfo += $"Tour: {tour.Name}, AgencyId: {tour.AgencyId}, Agency: {tour.Agency?.AgencyName}\n";
            }

            return Content(debugInfo);
        }
        // GET: Tours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tour = await _context.Tours
                .Include(t => t.Agency)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tour == null) return NotFound();

            // Check if user owns this tour
            var user = await _userManager.GetUserAsync(User);
            var agency = await _context.Agencies.FirstOrDefaultAsync(a => a.UserId == user.Id);

            if (agency == null || tour.AgencyId != agency.Id)
            {
                return RedirectToAction("Index");
            }

            return View(tour);
        }

        // POST: Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour != null)
            {
                // Verify ownership
                var user = await _userManager.GetUserAsync(User);
                var agency = await _context.Agencies.FirstOrDefaultAsync(a => a.UserId == user.Id);

                if (agency != null && tour.AgencyId == agency.Id)
                {
                    _context.Tours.Remove(tour);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }
        // POST: Tours/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Destination,Duration,Price,MaxParticipants,AgencyId,CreatedDate")] Tour tour)
        {
            if (id != tour.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TourExists(tour.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tour);
        }

        private bool TourExists(int id)
        {
            return _context.Tours.Any(e => e.Id == id);
        }
    }
}