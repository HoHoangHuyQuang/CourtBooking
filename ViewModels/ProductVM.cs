using System.ComponentModel.DataAnnotations;

namespace CourtBooking.ViewModels
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public class ProductVM
    {

        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Giá")]
        public decimal Price { get; set; } = 0;
        public string Model { get; set; }
        [Display(Name = "Xuất xứ")]
        public string Origin { get; set; }
        [Display(Name = "Kích cỡ")]
        public string ProductSize { get; set; }
        [Display(Name = "Thumbnail")]
        public string ThumbnailURl { get; set; }
        [Display(Name = "Liên hệ đặt hàng")]
        public string ContactPhone { get; }
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
