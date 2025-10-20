using DomainEntities.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.IRepositoryFolder;

namespace Repositories.RepositoryFolder
{
    public class StudentRepository :Repository<Student>, IStudentRepository
    {
        public StudentRepository()
        {

        }
    }
}
