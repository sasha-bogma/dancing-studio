namespace dancing_studio.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "ПІБ")]
        [MaxLength(60)]
        public string Name { get; set; }

        [MaxLength(18)]
        [Display (Name = "Номер телефону")]
        [Phone]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата народження")]
        [Required]
        public DateTime? Birthday { set; get; }
        //
        public virtual ICollection<Group> Groups { set; get; }
        public virtual ICollection<Lesson> Lessons { set; get; }
        public virtual ICollection<Salary> Salarys { set; get; }
    }
}