using System;
using System.Collections.Generic;

namespace WebAPI.Entity;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public string? ScheduleTime { get; set; }

    public DateOnly? MovieScheduleDate { get; set; }
    public virtual ICollection<MovieSchedule>? MovieSchedules { get; set; } = new List<MovieSchedule>();

    public virtual ICollection<ScheduleSeat>? ScheduleSeats { get; set; } = new List<ScheduleSeat>();
}
