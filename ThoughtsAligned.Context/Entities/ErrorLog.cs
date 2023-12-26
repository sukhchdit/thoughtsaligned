using System;
using System.Collections.Generic;

namespace ThoughtsAligned.Context.Entities;

public partial class ErrorLog
{
    public Guid Id { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? ErrorMessage { get; set; }

    public string? InnerException { get; set; }

    public string? Source { get; set; }

    public string? StackTrace { get; set; }
}
