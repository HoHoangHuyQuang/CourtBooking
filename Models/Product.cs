using System.ComponentModel.DataAnnotations;

namespace CourtBooking.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public int? CateId { get; set; }
        [Display(Name = "Danh mục")]
        public Category? Category { get; set; }
        [Display(Name = "Tên Sản Phẩm")]
        public string? Name { get; set; }
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }
        [Display(Name = "Giá")]
        public decimal Price { get; set; } = 0;
        public string? Model { get; set; }
        [Display(Name = "Xuất xứ")]
        public string? Origin { get; set; }
        [Display(Name = "Kích cỡ")]
        public string? ProductSize { get; set; }
        [Display(Name = "Thumbnail")]
        public string? ThumbnailURl { get; set; }
        [Display(Name = "Chi tiết sản phẩm")]
        public string? HtmlContent { get; set; }
        [Display(Name = "Liên hệ đặt hàng")]
        public string? ContactPhone { get; }
    }
}
