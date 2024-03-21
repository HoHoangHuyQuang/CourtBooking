namespace CourtBooking.Models
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }       
        public string Content { get; set; }
        public bool IsActive { get; set; }
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
