namespace dancing_studio.DAL
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Salary
    {
        public int Id { set; get; }

        [Display(Name = "Викладач")]
        public int TeacherId { set; get; }
        
        [Required]
        [Range(0, 10000)]
        [Display(Name = "Сума")]
        public double Amount { set; get; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        public DateTime? Date { set; get; }
        //
        public Teacher Teacher { set; get; }
    }
}