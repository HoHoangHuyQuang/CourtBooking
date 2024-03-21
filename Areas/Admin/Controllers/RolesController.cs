using CourtBooking.Models;
using CourtBooking.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourtBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        private readonly IUnitOfWork _uow;

        public RolesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Admin/Roles
        public async Task<IActionResult> Index()
        {
            return _uow.RoleRepo != null ?
                        View(await _uow.RoleRepo.GetAll()) :
                        Problem("Entity set 'DatabaseContext'  is null.");
        }

        // GET: Admin/Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (_uow.RoleRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }
            if (id == null)
            {
                return NotFound();
            }

            var role = await _uow.RoleRepo.GetById(id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: Admin/Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Role role)
        {
            if (ModelState.IsValid)
            {
                await _uow.RoleRepo.Add(role);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Admin/Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (_uow.RoleRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }
            if (id == null)
            {
                return NotFound();
            }

            var role = await _uow.RoleRepo.GetById(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Admin/Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Role role)
        {
            if (_uow.RoleRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }
            if (id != role.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _uow.RoleRepo.Update(role);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.RoleRepo.IsExists(role.Id))
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
            return View(role);
        }

        // GET: Admin/Roles/Delete/5
        public IActionResult Delete(int? id)
        {
            if (_uow.RoleRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }
            if (id == null)
            {
                return NotFound();
            }

            var role = _uow.RoleRepo.GetById(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Admin/Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_uow.RoleRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }
            var role = await _uow.RoleRepo.GetById(id);
            if (role != null)
            {
                _uow.RoleRepo.Delete(role);
            }

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
