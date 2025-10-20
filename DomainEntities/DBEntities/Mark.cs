using System;
using System.Collections.Generic;

namespace DomainEntities.DBEntities;

public partial class Mark
{
    public long StudentId { get; set; }

    public int SubjectId { get; set; }

    public decimal? Mark1 { get; set; }

    public virtual Student Student { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
