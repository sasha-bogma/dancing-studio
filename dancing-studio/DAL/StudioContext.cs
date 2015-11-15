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
        // Контекст настроен для использования строки подключения "StudioContext" из файла конфигурации  
        // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
        // "dancing_studio.DAL.StudioContext" в экземпляре LocalDb. 
        // 
        // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "StudioContext" 
        // в файле конфигурации приложения.
        public StudioContext()
            : base("name=StudioContext")
        {
        }

        // Добавьте DbSet для каждого типа сущности, который требуется включить в модель. Дополнительные сведения 
        // о настройке и использовании модели Code First см. в статье http://go.microsoft.com/fwlink/?LinkId=390109.

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
            // Отношение "многие ко многим" между таблицами группы и студенты
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
            // Ограничение "NOT NULL" и ограничение длины строки для столбца Teacher.Name
            modelBuilder.Entity<Teacher>()
                .Property(p => p.Name)
                .IsRequired().HasMaxLength(60);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }

    }

    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "ФИО")]
        [MaxLength(60)]
        public string Name { get; set; }

        [MaxLength(18)]
        [Display (Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
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
        [Display(Name = "ФИО")]
        [MaxLength(60)]
        public string Name { set; get; }

        [MaxLength(18)]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { set; get; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime? Birthday { set; get; }
        //
        public virtual ICollection<Parent> Parents { set; get; }

        [Display(Name = "Группы")]
        public virtual ICollection<Group> Groups { set; get; }

        public virtual ICollection<Payment> Payments { set; get; }
        public virtual ICollection<Present> Presences { set; get; }

    }

    public class Parent
    {
        public int Id { set; get; }

        public int StudentId { set; get; }

        [MaxLength(60)]
        [Display(Name="Кем является ученику")]
        public string Status { set; get; }

        [Required]
        [Display(Name = "ФИО")]
        [MaxLength(60)]
        public string Name { set; get; }

        [MaxLength(18)]
        [Display(Name = "Номер телефона")]
        public string Phone { set; get; }
        //
        public Student Student { set; get; }
    }

    public class Group
    {
        public int Id { set; get; }
        [Display(Name = "Преподаватель")]
        public int TeacherId { set; get; }

        [Required]
        [Display(Name = "Название группы")]
        [MaxLength(60)]
        public string Name { set; get; }
        //
        public Teacher Teacher { set; get; }
        public virtual ICollection<Student> Students { set; get; }
        public virtual ICollection<Lesson> Lessons { set; get; }
    }

    public class Lesson
    {
        public int Id { set; get; }
        public int GroupId { set; get; }

        [Display(Name = "Преподаватель")]
        public int TeacherId { set; get; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        public DateTime? DateTime { set; get; }

        [Required]
        [Display(Name = "Стоимость")]
        public int Price { set; get; }
        //
        public Group Group { set; get; }
        public Teacher Teacher { set; get; }
        public virtual ICollection<Present> Presents { set; get; }
    }

    public class Salary
    {
        public int Id { set; get; }

        [Display(Name = "Преподаватель")]
        public int TeacherId { set; get; }

        [Required]
        [Range(0, 10000)]
        [Display(Name = "Сумма")]
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
        [Display(Name = "Сумма")]
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
            [Display(Name = "присутствует")]
            Present,
            [Display(Name = "ув. причина")]
            AbsenceValid,
            [Display(Name = "не ув. причина")]
            AbsenceNotValid };
    }
}