namespace dancing_studio.DAL
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Group
    {
        public int Id { set; get; }

        [Display(Name = "Викладач")]
        public int TeacherId { set; get; }

        [Required]
        [Display(Name = "Назва групи")]
        [MaxLength(60)]
        public string Name { set; get; }
        
        public Teacher Teacher { set; get; }

        public virtual ICollection<Student> Students { set; get; }

        public virtual ICollection<Lesson> Lessons { set; get; }

        public virtual ICollection<Plan> Plans { set; get; }
    }
}