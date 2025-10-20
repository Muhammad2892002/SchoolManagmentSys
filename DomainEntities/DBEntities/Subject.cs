using System;
using System.Collections.Generic;

namespace DomainEntities.DBEntities;

public partial class Subject
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();

    public ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
