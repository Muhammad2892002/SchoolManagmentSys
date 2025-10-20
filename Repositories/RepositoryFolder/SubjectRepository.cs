using DomainEntities.DBEntities;
using Repositories.IRepositoryFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RepositoryFolder
{
    public class SubjectRepository :Repository<Subject>,ISubjectRepository
    {
        public SubjectRepository()
        {
        }
    }
}
