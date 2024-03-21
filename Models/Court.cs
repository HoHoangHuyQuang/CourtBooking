
using System.ComponentModel.DataAnnotations;

namespace CourtBooking.Models
{
    public class Court
    {
        public int CourtId { get; set; }
        [Display(Name = "Tên Sân")]
        public string? Name { get; set; }
        [Display(Name = "Địa chỉ Sân")]
        public string? Address { get; set; }
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }
        [Display(Name = "Giá mỗi giờ")]
        public decimal PricePerHour { get; set; } = 0;
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
