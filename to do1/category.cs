using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace to_do1
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<task> Tasks { get; set; } = new List<task>();
    }
}