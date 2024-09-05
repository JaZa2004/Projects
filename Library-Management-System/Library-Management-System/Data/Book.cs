using System;
using System.Collections.Generic;

namespace Library_Management_System.Data;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public int AuthorId { get; set; }

    public int GenreId { get; set; }

    public DateOnly? PublishedDate { get; set; }

    public string? Isbn { get; set; }

    public int AvailableCopies { get; set; }

    public virtual ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
}
