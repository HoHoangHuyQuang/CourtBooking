using CourtBooking.Data;
using CourtBooking.Models;
using CourtBooking.Repositories.Interfaces;
using CourtBooking.Utils;
using CourtBooking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Diagnostics;


namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IOptionsSnapshot<AppSettings> appSettings)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }

        public IActionResult Index()
        {
            ViewBag.HighLight = _appSettings.HighlightVidUrl;
            return View();
        }
        public async Task<IActionResult> Booking()
        {
            var availCourt = await _unitOfWork.BookingRepo.GetAllCourtAvailable(DateTime.Now, DateTime.Now.AddDays(_appSettings.MaxBookingRange));
            ViewBag.AvailCourts = new SelectList(availCourt, "CourtId", "Name");

            return View(null);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Booking([Bind("StartDate,StartTime,Duration,BookingCountId")] BookingVm bookingVm)
        {
            DateTime combine_time = bookingVm.StartDate.Add(bookingVm.StartTime.TimeOfDay);
            DateTime combine_Opentime = bookingVm.StartDate.Add(TimeSpan.Parse(_appSettings.Open));
            DateTime combine_Closetime = bookingVm.StartDate.Add(TimeSpan.Parse(_appSettings.Close));
            var availCourt = await _unitOfWork.BookingRepo.GetAllCourtAvailable(DateTime.Now, DateTime.Now.AddDays(_appSettings.MaxBookingRange));

            if (bookingVm.EndTime > combine_Closetime || combine_time < combine_Opentime || combine_time.Date > DateTime.Now.AddDays(_appSettings.MaxBookingRange))
            {
                ViewBag.AvailCourts = new SelectList(availCourt, "CourtId", "Name");
                return View(bookingVm);
            }
            Booking booking = new Booking();
            var court = await _unitOfWork.CourtRepo.GetById(bookingVm.BookingCountId);
            if (court == null || !availCourt.Any(x => x.Equals(court)))
            {
                ViewBag.AvailCourts = new SelectList(availCourt, "CourtId", "Name");
                return View(bookingVm);
            }

            booking.BookingId = Guid.NewGuid();
            booking.Status = 0;
            booking.CourtId = court.CourtId;
            booking.Court = court;
            booking.TotalAmount =decimal.Parse(bookingVm.Duration) * court.PricePerHour;
            booking.EndTime = bookingVm.EndTime;
           
            booking.Checksum = AppUtils.MD5Enscrypt(booking.BookingId.ToString() + booking.CourtId.ToString() + booking.CreatedAt.ToString() + booking.StartTime.ToString() + booking.DurationInHour.ToString() + booking.TotalAmount.ToString() + booking.Status.ToString());

            await _unitOfWork.BookingRepo.Add(booking);
            await _unitOfWork.SaveChangesAsync();
            ViewBag.AvailCourts = new SelectList(availCourt, "CourtId", "Name");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ProductView()
        {
            return View(await _unitOfWork.ProductRepo.GetAll());
        }
        public async Task<IActionResult> ProductDetail(string id)
        {
            return View(await _unitOfWork.ProductRepo.GetById(new Guid(id)));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
