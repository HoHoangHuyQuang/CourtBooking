using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourtBooking.Models;
using CourtBooking.Utils;
using CourtBooking.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CourtBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {

        private readonly IUnitOfWork _uow;
        public UsersController(IUnitOfWork uow)
        {

            _uow = uow;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {

            return View(await _uow.UserRepo.GetAll());
        }

        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _uow.UserRepo == null)
            {
                return NotFound();
            }

            var user = await _uow.UserRepo.GetById(new Guid(id));
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Admin/Users/Create
        public async Task<IActionResult> Create()
        {
            ViewData["RoleId"] = new SelectList(await _uow.RoleRepo.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,FullName,Email,Phone,IsActive,RoleId")] User user, string ConfirmPassword)
        {
            if (string.IsNullOrEmpty(user.Password)|| string.IsNullOrEmpty(ConfirmPassword)|| !ConfirmPassword.Equals(user.Password))
            {
                ModelState.AddModelError("", "Mật khẩu không khớp!");
                ViewData["RoleId"] = new SelectList(await _uow.RoleRepo.GetAll(), "Id", "Name", user.RoleId);
                return View(user);
            }
            if (await _uow.UserRepo.GetByUsername(user.UserName) != null)
            {
                ModelState.AddModelError("", "Username đã được sử dụng!");
                ViewData["RoleId"] = new SelectList(await _uow.RoleRepo.GetAll(), "Id", "Name", user.RoleId);
                return View(user);
            }
            if (ModelState.ContainsKey("Role"))
            {
                ModelState.Remove("Role");
            }
            if (ModelState.IsValid)
            {
                user.Id = Guid.NewGuid();
                user.Password = AppUtils.HmacSha256Encrypt(user.Password);
                await _uow.UserRepo.Add(user);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(await _uow.RoleRepo.GetAll(), "Id", "Name", user.RoleId);
            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _uow.UserRepo == null)
            {
                return NotFound();
            }

            var user = await _uow.UserRepo.GetById(new Guid(id));
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(await _uow.RoleRepo.GetAll(), "Id", "Name", user.RoleId);
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Password,FullName,Email,Phone,IsActive,RoleId")] User user, string ConfirmPassword)
        {
            if (new Guid(id) != user.Id)
            {
                return NotFound();
            }
            if (string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(ConfirmPassword) || !ConfirmPassword.Equals(user.Password))
            {
                ModelState.AddModelError("", "Mật khẩu không khớp!");
                ViewData["RoleId"] = new SelectList(await _uow.RoleRepo.GetAll(), "Id", "Name", user.RoleId);
                return View(user);
            }
            if (ModelState.ContainsKey("Role"))
            {
                ModelState.Remove("Role");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var oldData = await _uow.UserRepo.GetById(new Guid(id));
                    if (oldData != null && !oldData.Password.Equals(user.Password))
                    {
                        user.Password = AppUtils.HmacSha256Encrypt(user.Password);
                    }
                    await _uow.UserRepo.Update(user);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.UserRepo.IsExists(user.Id))
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
            ViewData["RoleId"] = new SelectList(await _uow.RoleRepo.GetAll(), "Id", "Name", user.RoleId);
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _uow.UserRepo == null)
            {
                return NotFound();
            }

            var user = await _uow.UserRepo.GetById(new Guid(id));
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_uow.UserRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }
            var user = await _uow.UserRepo.GetById(new Guid(id));
            if (user != null)
            {
                _uow.UserRepo.Delete(user);
            }

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
