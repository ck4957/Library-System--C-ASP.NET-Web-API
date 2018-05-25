using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlyBook.Models;

namespace OnlyBook.Repository
{
    public interface IAuthorRepository
    {
        Author Add(Author author);
        IEnumerable<Author> GetAll();
        Author GetById(int id);
        Author GetByName(string authorName);
        void Delete(Author author);
        void Update(Author author);
        void AddBooktoAuthor(Author author);
    }
}
