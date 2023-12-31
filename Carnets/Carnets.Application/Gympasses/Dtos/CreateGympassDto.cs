﻿using Carnets.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Carnets.Application.Gympasses.Dtos
{
    public class CreateGympassDto
    {
        [Required]
        [MaxLength(36)]
        public string GympassTypeId { get; set; }

        [MaxLength(100)]
        public string SuccessUrl { get; set; }

        [MaxLength(100)]
        public string CancelUrl { get; set; }

        public PaymentType PaymentType { get; set; }
    }
}
