using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CourtBooking.Models
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public class Booking
    {
        public Guid BookingId { get; set; }
        [Display(Name = "Tạo lúc")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Display(Name = "Thới gian bắt đầu")]
        public DateTime StartTime { get; set; }
        [Display(Name = "Số giờ thuê")]
        public float DurationInHour { get; set; }
        [Display(Name = "Thời gian kết thúc")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Số tiền thanh toán")]
        public decimal TotalAmount { get; set; }
        [Display(Name = "Thông tin thêm")]
        public string? ExtraData { get; set; }     
        
        [Display(Name = "Trạng thái")]
        public int Status { get; set; } 
       
        public Guid? UserId { get; set; }
        public User? Users { get; set; }
        public int CourtId { get; set; }
        [Display(Name = "Sân")]
        public Court Court { get; set; }
        public string? Checksum { get; set; }
        
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
