using CourtBooking.Models;
using CourtBooking.Repositories.Interfaces;
using CourtBooking.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CourtBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ProductsController : Controller
    {

        private readonly IUnitOfWork _uow;
        public ProductsController(IUnitOfWork uow)
        {

            _uow = uow;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index([FromQuery] int? categoryid)
        {
            if (_uow.ProductRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }
            if (categoryid != null)
            {
                return View(await _uow.ProductRepo.GetAllByCategory(categoryid));
            }
            else
            {
                return View(await _uow.ProductRepo.GetAll());
            }
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (_uow.ProductRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var product = await _uow.ProductRepo.GetById(new Guid(id));
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CateId"] = new SelectList(await _uow.CategoryRepo.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(20000000)]
        public async Task<IActionResult> Create(Product product, IFormFile? img)
        {
            if (ModelState.IsValid)
            {
                product.Id = Guid.NewGuid();
                product.ThumbnailURl = await AppUtils.UploadedFileAsync(img, product.Id.ToString());
               
                await _uow.ProductRepo.Add(product);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            ViewData["CateId"] = new SelectList(await _uow.CategoryRepo.GetAll(), "Id", "Name");
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (_uow.ProductRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var product = await _uow.ProductRepo.GetById(new Guid(id));
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CateId"] = new SelectList(await _uow.CategoryRepo.GetAll(), "Id", "Name");
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string? id, [Bind("Id,CateId,Name,Description,Price,Model,Origin,ProductSize,ThumbnailURl,HtmlContent,ContactPhone")] Product product, IFormFile img)
        {

            if (string.IsNullOrEmpty(id) || new Guid(id) != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var productOldData = await _uow.ProductRepo.GetById(new Guid(id));
                    if (productOldData == null)
                    {
                        return NotFound();
                    }
                    if (string.IsNullOrEmpty(productOldData.ThumbnailURl) ||!productOldData.ThumbnailURl.Contains(img.FileName))
                    {
                        product.ThumbnailURl = await AppUtils.UploadedFileAsync(img, product.Id.ToString());
                    }
                    await _uow.ProductRepo.Update(product);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.ProductRepo.IsExists(new Guid(id)))
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
            ViewData["CateId"] = new SelectList(await _uow.CategoryRepo.GetAll(), "Id", "Name");
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (_uow.ProductRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var product = await _uow.ProductRepo.GetById(new Guid(id));
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string? id)
        {
            if (_uow.ProductRepo == null)
            {
                return Problem("Entity set 'DatabaseContext'  is null.");
            }

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var product = await _uow.ProductRepo.GetById(new Guid(id));
            if (product != null)
            {
                _uow.ProductRepo.Delete(product);
            }

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult UploadImage(IFormFile upload, string id)
        {
           
            if (upload == null|| upload.Length <= 0)
            {
                return Json(new { uploaded = false, V = "No file uploaded" });
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/Resources", id, "Images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/Resources", id, "Images", fileName);
            if (System.IO.File.Exists(path))
            {
                return Json(new { uploaded = false, V = "Duplicated file name" });
            }
            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }

            var url = $"{this.Request.Scheme}://{this.Request.Host}{"/Resources/"}{id + "/Images/"}{fileName}";
            return Json(new { uploaded = true, url });
        }

    }
}
