using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CourtBooking.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Display(Name = "Tên quyền")]
        public string? Name { get; set; }
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }
        [JsonIgnore]
        public IList<User> Users { get; set; } = new List<User>();
    }
}
