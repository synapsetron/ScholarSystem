﻿using System.ComponentModel.DataAnnotations;

namespace EntryTask.Infrastructure.Options
{
    public class ConnectionStringsOptions
    {
        public const string SectionName = "Database";

        [Required]
        public string DefaultConnection { get; set; } = string.Empty;

        [Required]
        public string ServerVersion { get; set; } = string.Empty;
    }
}
