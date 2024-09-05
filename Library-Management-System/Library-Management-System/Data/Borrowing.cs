using System;
using System.Collections.Generic;

namespace Library_Management_System.Data;

public partial class Borrowing
{
    public int BorrowingId { get; set; }

    public string UserId { get; set; } = null!;

    public int BookId { get; set; }

    public DateOnly BorrowDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public bool IsReturned { get; set; }

    public decimal? LateFee { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Aspnetuser User { get; set; } = null!;
}
