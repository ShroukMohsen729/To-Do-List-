using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace to_do1
{
    public class task
    {

        [Key]
        public int Id { get; set; } 

        [Required(ErrorMessage = "Task name is required.")]
        [StringLength(50,MinimumLength =10,ErrorMessage= "Name must be between 10 and 50 characters long.")]
        public string name {  get; set; }

        [StringLength(500, ErrorMessage = "Task description cannot exceed 500 characters.")]
        public string description { get; set; }
        public DateTime? deadline { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DefaultValue(null)]
        public DateTime? CompletedAt { get; set; } 

        [DefaultValue("Un Compeleted")]
        public string status { get; set; }               // during task time open or not $

        [ForeignKey("person")]
        public int personId{ get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public task()
        {
            this.status = "Un Compeleted";
            this.CompletedAt = null;
        }

        // public Category Cate { get; set; }
        // public person Per { get; set; }                // i think malhosh lazma 3lshan ana aslan m3aya el id
    }
}
