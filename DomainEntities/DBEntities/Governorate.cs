using System;
using System.Collections.Generic;

namespace DomainEntities.DBEntities;

public partial class Governorate
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
