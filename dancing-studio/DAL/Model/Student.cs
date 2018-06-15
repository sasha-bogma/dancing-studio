namespace dancing_studio.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Student
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "ПІБ")]
        [MaxLength(60)]
        public string Name { set; get; }

        [MaxLength(18)]
        [Display(Name = "Номер телефону")]
        [Phone]
        public string PhoneNumber { set; get; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата народження")]
        [Required]
        public DateTime? Birthday { set; get; }

        [MaxLength(500)]
        [Display(Name="Додаткова информація")]
        [DataType(DataType.MultilineText)]
        public string Info { set; get; }
        //
        public virtual ICollection<Parent> Parents { set; get; }

        [Display(Name = "Групи")]
        public virtual ICollection<Group> Groups { set; get; }

        public virtual ICollection<Payment> Payments { set; get; }
        public virtual ICollection<Present> Presences { set; get; }

    }
}