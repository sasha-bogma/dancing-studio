namespace dancing_studio.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class StudioContext : DbContext
    {
        public StudioContext()
            : base("name=StudioContext")
        {
        }
        

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Present> Presences { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().
                HasMany(a => a.Students).
                WithMany(p => p.Groups).
                Map(
                    m =>
                    {
                        m.MapLeftKey("GroupId");
                        m.MapRightKey("StudentId");
                        m.ToTable("GrouoStudents");
                    });
            
            modelBuilder.Entity<Teacher>()
                .Property(p => p.Name)
                .IsRequired().HasMaxLength(60);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }

        public System.Data.Entity.DbSet<dancing_studio.DAL.Plan> Plans { get; set; }

    }

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

    public class Group
    {
        public int Id { set; get; }
        [Display(Name = "Викладач")]
        public int TeacherId { set; get; }

        [Required]
        [Display(Name = "Назва групи")]
        [MaxLength(60)]
        public string Name { set; get; }
        //
        public Teacher Teacher { set; get; }
        public virtual ICollection<Student> Students { set; get; }
        public virtual ICollection<Lesson> Lessons { set; get; }
        public virtual ICollection<Plan> Plans { set; get; }
    }

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

    public class Salary
    {
        public int Id { set; get; }

        [Display(Name = "Викладач")]
        public int TeacherId { set; get; }

        [Display(Name="Відсоток")]
        [Required]
        [Range(0, 100)]
        public double lobe { set; get; }

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