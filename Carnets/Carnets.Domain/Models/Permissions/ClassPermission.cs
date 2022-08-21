﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carnets.Domain.Models
{
    public class ClassPermission
    {
        [Key]
        [ForeignKey("Permission")]
        public string PermissionId { get; set; }
        public Permission Permission { get; set; }

        [Required]
        // TODO: Update Documentatino: varchar(30)
        [MaxLength(30)]
        // TODO: Unique
        public string PermissionName { get; set; }
    }
}
