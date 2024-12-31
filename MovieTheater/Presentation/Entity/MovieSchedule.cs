using System;
using System.Collections.Generic;

namespace WebAPI.Entity;

public partial class MovieSchedule
{
    public string MovieId { get; set; } = null!;

    public int ScheduleId { get; set; }   
    public virtual Movie Movie { get; set; } = null!;

    public virtual Schedule Schedule { get; set; } = null!;
}
