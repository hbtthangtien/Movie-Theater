using System;
using System.Collections.Generic;

namespace WebAPI.Entity;

public partial class Payment
{
    public int PaymentId { get; set; }
    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentStatus { get; set; }
    public double? TotalAmount { get; set; }

    public string? AccountId { get; set; }
    public virtual ApplicationUser? Account { get; set; }

    public virtual ICollection<TransactionHistory> TransactionHistories { get; set; } = new List<TransactionHistory>();
}
