using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlyBook.Models;

namespace OnlyBook.Repository
{
    public interface ILibraryRepository
    {
        Library Add(Library library);
        IEnumerable<Library> GetAll();
        Library GetById(int id);
        void Delete(Library library);
        void Update(Library library);
    }
}
