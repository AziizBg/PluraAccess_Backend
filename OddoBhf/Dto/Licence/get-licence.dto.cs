﻿using OddoBhf.Models;

namespace OddoBhf.Dto
{
    public class GetLicenceDto
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public Session? CurrentSession { get; set; }
    }
}