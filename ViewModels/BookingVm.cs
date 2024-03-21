using System.ComponentModel.DataAnnotations;
using CourtBooking.Models;

namespace CourtBooking.ViewModels
{
    public class BookingVm
    {
        public int BookingCountId { get; set; }
        [Display(Name = "Ngày")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "Thời gian bắt đầu")]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }
        [Display(Name = "Số giờ thuê")]
        public string Duration { get; set; }
        [Display(Name = "Thời gian kết thúc")]
        public DateTime EndTime => StartDate.Add(StartTime.TimeOfDay).AddHours(double.Parse(Duration));
        public string BookingUserId { get; set; }

    }
}
