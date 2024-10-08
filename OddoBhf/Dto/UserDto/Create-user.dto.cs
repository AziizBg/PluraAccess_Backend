﻿using System.ComponentModel;

namespace OddoBhf.Dto.User
{
    public class CreateUserDto
    {
        [DefaultValue("Root")]
        public string? UserName { get; set; }
        [DefaultValue("example@oddo-bhf.com")]
        public string? Email { get; set; }
        [DefaultValue("Admin")]
        public string? Password { get; set; }
        [DefaultValue("User")]
        public string? Role { get; set; }

    }
}
