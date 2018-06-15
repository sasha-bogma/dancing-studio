namespace dancing_studio.DAL
{
    using System.ComponentModel.DataAnnotations;

    public class Plan
    {
        public int Id { set; get; }
        public DayOfWeek LessDay { set; get; }
        public TimeInterval LessTimeInterval { set; get; }
        public int HallNum { set; get; }
        public int GroupId { set; get; }
        //
        public Group Group { set; get; }
        //
        public enum DayOfWeek { 
            [Display(Name = "Понеділок")]
            Monday,
            [Display(Name = "Вівторок")]
            Tuesday,
            [Display(Name = "Середа")]
            Wednesday,
            [Display(Name = "Четвер")]
            Thursday,
            [Display(Name = "П'ятниця")]
            Friday,
            [Display(Name = "Субота")]
            Saturday,
            [Display(Name = "Неділя")]
            Sunday
        }
        public enum TimeInterval
        {
            [Display(Name = "08:00-09:30")]
            t08_00,
            [Display(Name = "09:30-11:00")]
            t09_30,
            [Display(Name = "11:00-12:30")]
            t11_00,
            [Display(Name = "12:30-14:00")]
            t12_30,
            [Display(Name = "14:00-15:30")]
            t14_00,
            [Display(Name = "15:30-17:00")]
            t15_30,
            [Display(Name = "17:00-18:30")]
            t17_00,
            [Display(Name = "18:30-20:00")]
            t18_30,
            [Display(Name = "20:00-21:30")]
            t20_00
        }
    }
}