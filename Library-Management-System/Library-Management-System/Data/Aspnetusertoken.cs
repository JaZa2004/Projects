using System;
using System.Collections.Generic;

namespace Library_Management_System.Data;

public partial class Aspnetusertoken
{
    public string UserId { get; set; } = null!;

    public string LoginProvider { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Value { get; set; }

    public virtual Aspnetuser User { get; set; } = null!;
}
