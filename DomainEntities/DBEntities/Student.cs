using System;
using System.Collections.Generic;

namespace DomainEntities.DBEntities;

public partial class Student
{
    public long Id { get; set; }

    public string FirstName { get; set; } = null!;

    public int NationalId { get; set; }

    public DateTime BirthDate { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int Governorate { get; set; }

    public bool Gender { get; set; }

    public string LastName { get; set; } = null!;

    public virtual Governorate GovernorateObj { get; set; } = null!;

    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();

    public ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
