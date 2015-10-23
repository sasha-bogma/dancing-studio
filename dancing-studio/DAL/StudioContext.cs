namespace dancing_studio.DAL
{
    using System;
    using System.Collections.Generic;
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

        [Display(Name = "ФИО")]
        public string Name { get; set; }

        [Display (Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата рождения")]
        public DateTime? Birthday { set; get; }
        //
        public virtual ICollection<Group> Groups { set; get; }
        public virtual ICollection<Lesson> Lessons { set; get; }
        public virtual ICollection<Salary> Salarys { set; get; }
    }

    public class Student
    {
        public Student(string name) : base() { Name = name; }

        public int Id { get; set; }
        public string Name { set; get; }
        public string PhoneNumber { set; get; }
        public DateTime? Birthday { set; get; }
        //
        public virtual ICollection<Parent> Parents { set; get; }
        public virtual ICollection<Group> Groups { set; get; }
        public virtual ICollection<Payment> Payments { set; get; }
        public virtual ICollection<Present> Presences { set; get; }

    }

    public class Parent
    {
        public Parent(string name) : base() { Name = name; }

        public int Id { set; get; }
        public int StudentId { set; get; }
        public string Status { set; get; }
        public string Name { set; get; }
        public string Phone { set; get; }
        //
        public Student Student { set; get; }
    }

    public class Group
    {
        public int Id { set; get; }
        public int TeacherId { set; get; }
        public string Mame { set; get; }
        //
        public Teacher Teacher { set; get; }
        public virtual ICollection<Student> Students { set; get; }
        public virtual ICollection<Lesson> Lessons { set; get; }
    }

    public class Lesson
    {
        public int Id { set; get; }
        public int GroupId { set; get; }
        public int TeacherId { set; get; }
        public DateTime? DateTime { set; get; }
        public int Price { set; get; }
        //
        public Group Group { set; get; }
        public Teacher Teacher { set; get; }
        public virtual ICollection<Present> Presents { set; get; }
    }

    public class Salary
    {
        public int Id { set; get; }
        public int TeacherId { set; get; }
        public double Amount { set; get; }
        public DateTime? Date { set; get; }
        //
        public Teacher Teacher { set; get; }
    }

    public class Payment
    {
        public int Id { set; get; }
        public int StudentId { set; get; }
        public double Amount { set; get; }
        public DateTime? Date { set; get; }
        //
        public Student Student { set; get; }
    }

    public class Present
    {
        public int Id { set; get; }
        public int StudentId { set; get; }
        public int LessonnId { set; get; }
        public Presence Condition { set; get; }
        //
        public Student Student { set; get; }
        public Lesson Lesson { set; get; }
        //
        public enum Presence { Present, AbsenceValid, AbsenceNotValid };
    }
}