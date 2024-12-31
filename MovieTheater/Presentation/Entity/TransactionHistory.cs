using System;
using System.Collections.Generic;

namespace WebAPI.Entity;

public partial class TransactionHistory
{
    public int Id { get; set; }

    public int PaymentId { get; set; }

    public DateOnly? TransactionDate { get; set; }

    public int? Status { get; set; }

    public string? Notes { get; set; }

    public virtual Payment Payment { get; set; } = null!;
}
