﻿using System.ComponentModel.DataAnnotations;

namespace FitnessClubs.Application.FitnessClubs.Dtos
{
    public class CreateFitnessClubDto
    {
        [Required]
        [StringLength(100)]
        public string FitnessClubName { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [MaxLength(500)]
        public string FitnessClubLogoUrl { get; set; }

        [MaxLength(36)]
        public string OwnerId { get; set; }
    }
}
