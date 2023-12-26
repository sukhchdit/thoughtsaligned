using System;
using System.Collections.Generic;

namespace ThoughtsAligned.Context.Entities;

public partial class FileInformation
{
    public Guid Id { get; set; }

    public string FileName { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string FilePath { get; set; } = null!;

    public byte FilePathType { get; set; }

    public bool? Status { get; set; }
}
