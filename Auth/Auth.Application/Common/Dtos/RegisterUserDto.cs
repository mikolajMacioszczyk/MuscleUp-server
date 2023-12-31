﻿using Auth.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Auth.Application.Common.Dtos
{
    public abstract class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public GenderType Gender { get; set; }

        [MaxLength(500)]
        public string AvatarUrl { get; set; }
    }
}
