using System;
using System.Collections.Generic;

namespace WebAPI.Entity;

public partial class Employee
{
    public string? EmployeeId { get; set; } = null!;

    public string? AccountId { get; set; } = null!; 
    public required virtual ApplicationUser? Account { get; set; } = null!;
}
