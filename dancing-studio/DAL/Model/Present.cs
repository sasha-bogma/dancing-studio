namespace dancing_studio.DAL
{
    using System.ComponentModel.DataAnnotations;

    public class Present
    {
        public int Id { set; get; }
        public int StudentId { set; get; }
        public int LessonId { set; get; }
        public Presence Condition { set; get; }
        //
        public Student Student { set; get; }
        public Lesson Lesson { set; get; }
        //
        public enum Presence {
            [Display(Name = "присутній")]
            Present,
            [Display(Name = "пов. причина")]
            AbsenceValid,
            [Display(Name = "не пов. причина")]
            AbsenceNotValid };
    }
}