using System;
using System.Collections.Generic;

namespace ThoughtsAligned.Context.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdateOn { get; set; }

    public string? UpdatedBy { get; set; }

    public string Email { get; set; } = null!;

    public string? Name { get; set; }

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public byte Status { get; set; }

    public byte UserRole { get; set; }
}
