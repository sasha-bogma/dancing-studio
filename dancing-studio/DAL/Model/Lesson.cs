namespace dancing_studio.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Lesson
    {
        public int Id { set; get; }
        public int GroupId { set; get; }

        [Display(Name = "Викладач")]
        public int TeacherId { set; get; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        public DateTime? DateTime { set; get; }

        [Required]
        [Display(Name = "Вартість")]
        public int Price { set; get; }
        //
        public Group Group { set; get; }
        public Teacher Teacher { set; get; }
        public virtual ICollection<Present> Presents { set; get; }
    }
}