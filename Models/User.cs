using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CourtBooking.Models
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }
        public string? Email { get; set; }
        [Display(Name = "Số điện thoại")]
        public string? Phone { get; set; }
        [Display(Name = "Đang hoạt động")]
        public bool IsActive { get; set; }
        [Display(Name = "Tạo lúc")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [Display(Name = "Quyền")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
