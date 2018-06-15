namespace dancing_studio.DAL
{
    using System.ComponentModel.DataAnnotations;

    public class Parent
    {
        public int Id { set; get; }

        public int StudentId { set; get; }

        [MaxLength(60)]
        [Display(Name="Ким доводиться")]
        public string Status { set; get; }

        [Required]
        [Display(Name = "ПІБ")]
        [MaxLength(60)]
        public string Name { set; get; }

        [MaxLength(18)]
        [Display(Name = "Номер телефону")]
        public string Phone { set; get; }
        //
        public Student Student { set; get; }
    }
}