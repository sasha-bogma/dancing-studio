namespace dancing_studio.DAL
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Payment
    {
        public int Id { set; get; }

        public int StudentId { set; get; }

        [Required]
        [Range(0, 1000)]
        [Display(Name = "Сума")]
        public double Amount { set; get; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { set; get; }
        //
        public Student Student { set; get; }
    }
}