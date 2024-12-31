using System;
using System.Collections.Generic;

namespace WebAPI.Entity;

public partial class CinemaRoom
{
    public int CinemaRoomId { get; set; }

    public string? CinemeRoomName { get; set; }

    public int? SeatQuantity { get; set; }

    public virtual ICollection<Movie>? Movies { get; set; } = new List<Movie>();

    public virtual ICollection<Seat>? Seats { get; set; } = new List<Seat>();
}
