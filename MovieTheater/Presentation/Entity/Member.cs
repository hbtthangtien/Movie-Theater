using System;
using System.Collections.Generic;

namespace WebAPI.Entity;

public partial class Member
{
    public string MemberId { get; set; } = null!;
    public string AccountId { get; set; } = null!;
    public double? Score { get; set; }
    public required virtual ApplicationUser? Account { get; set; } = null!;
}
