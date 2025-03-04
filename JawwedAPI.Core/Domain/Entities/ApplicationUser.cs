using System;
using System.ComponentModel.DataAnnotations;

namespace JawwedAPI.Core.Domain.Entities;

public class ApplicationUser
{
    [Key]
    public Guid UserId { get; set; } = Guid.NewGuid();

    [StringLength(70, ErrorMessage = "Name cannot be longer than 70 characters")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Email is required!")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public ApplicationRoles UserRole { get; set; } = ApplicationRoles.Basic;
}

public enum ApplicationRoles
{
    Basic,
    Premium,
}
