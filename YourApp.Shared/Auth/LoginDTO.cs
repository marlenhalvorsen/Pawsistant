﻿using System.ComponentModel.DataAnnotations;

namespace Library.Shared.Auth;

public class LoginDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
