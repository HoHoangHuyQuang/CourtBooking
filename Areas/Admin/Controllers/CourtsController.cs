using CourtBooking.Models;
using CourtBooking.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CourtBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class CourtsController : Controller
    {
        private readonly IUnitOfWork _uow;

        public CourtsController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Admin/Courts
        public async Task<IActionResult> Index()
        {
            if (_uow.CourtRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }
            return View(await _uow.CourtRepo.GetAll());
        }

        // GET: Admin/Courts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (_uow.CourtRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }
            if (id == null)
            {
                return NotFound();
            }

            var court = await _uow.CourtRepo.GetById(id);
            if (court == null)
            {
                return NotFound();
            }

            return View(court);
        }

        // GET: Admin/Courts/Create
        public IActionResult Create()
        {          
            return View();
        }

        // POST: Admin/Courts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourtId,Name,Description,PricePerHour")] Court court)
        {
            if (ModelState.IsValid)
            {
                await _uow.CourtRepo.Add(court);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(court);
        }

        // GET: Admin/Courts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var court = await _uow.CourtRepo.GetById(id);
            if (court == null)
            {
                return NotFound();
            }

            return View(court);
        }

        // POST: Admin/Courts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourtId,Name,Description,PricePerHour,TypeId")] Court court)
        {
            if (id != court.CourtId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _uow.CourtRepo.Update(court);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.CourtRepo.IsExists(court.CourtId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(court);
        }

        // GET: Admin/Courts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_uow.CourtRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }
            if (id == null)
            {
                return NotFound();
            }

            var court = await _uow.CourtRepo.GetById(id);
            if (court == null)
            {
                return NotFound();
            }

            return View(court);
        }

        // POST: Admin/Courts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_uow.CourtRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }
            var court = await _uow.CourtRepo.GetById(id);
            if (court != null)
            {
                _uow.CourtRepo.Delete(court);
            }

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
