using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlyBook.Models;

namespace OnlyBook.Repository
{
    public interface IPatronRepository
    {
        Patron Add(Patron patron);
        IEnumerable<Patron> GetAll();
        Patron GetById(int id);
        void Delete(Patron patron);
        void Update(Patron patron);
        void Checkout(int patronid, int bookid);
        void BookReturn(int patronid, int bookid);
    }
}
