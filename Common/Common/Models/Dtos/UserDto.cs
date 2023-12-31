﻿using Auth.Domain.Enums;
using Common.Enums;

namespace Auth.Application.Common.Dtos
{
    public abstract class UserDto
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime RegisteredDate { get; set; }

        public GenderType Gender { get; set; }

        public abstract RoleType Role { get; }

        public string AvatarUrl { get; set; }
    }
}
