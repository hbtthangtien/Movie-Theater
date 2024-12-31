namespace WebAPI.Entity
{
    public class TypeSeat
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public double? price { get; set; }
        public virtual ICollection<Seat>? Seats { get; set; } = new List<Seat>();
        public virtual ICollection<ScheduleSeat>? ScheduleSeats { get; set; } = new List<ScheduleSeat>();
    }
}
