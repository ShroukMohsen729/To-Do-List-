using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace to_do1
{
    public class person
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Enter Your Name please !")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Name must be between 10 and 50 characters long.")]
        public string name {  get; set; }

        [Required(ErrorMessage = "Enter Your Email please !")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string email {  get; set; }
        [Required(ErrorMessage = "Enter Your Password please !")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters long.")]
        public string password {  get; set; }
        public ICollection<task> Tasks { get; set; } = new List<task>();


    }
}
