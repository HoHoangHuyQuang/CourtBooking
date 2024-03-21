using CourtBooking.Models;
using CourtBooking.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourtBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _uow;

        public CategoriesController(IUnitOfWork uow)
        {
            _uow = uow;
        }



        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {
            return _uow.CategoryRepo != null ?
                        View(await _uow.CategoryRepo.GetAll()) :
                        Problem("Entity set 'DatabaseContext.Categories'  is null.");
        }

        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (_uow.CategoryRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }
            if (id == null)
            {
                return NotFound();
            }

            var category = await _uow.CategoryRepo.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                await _uow.CategoryRepo.Add(category);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (_uow.CategoryRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }
            if (id == null)
            {
                return NotFound();
            }

            var category = await _uow.CategoryRepo.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _uow.CategoryRepo.Update(category);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.CategoryRepo.IsExists(category.Id))
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
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_uow.CategoryRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }
            if (id == null)
            {
                return NotFound();
            }


            var category = await _uow.CategoryRepo.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_uow.CategoryRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }


            var category = await _uow.CategoryRepo.GetById(id);
            if (category != null)
            {
                _uow.CategoryRepo.Delete(category);
            }

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
