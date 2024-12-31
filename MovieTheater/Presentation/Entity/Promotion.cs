﻿using System;
using System.Collections.Generic;

namespace WebAPI.Entity;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string? Detail { get; set; }

    public int? DiscountLevel { get; set; }

    public DateTime? EndTime { get; set; }

    public string? Image { get; set; }

    public DateTime? StartTime { get; set; }

    public string? Title { get; set; }
}
