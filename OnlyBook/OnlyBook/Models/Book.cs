using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlyBook.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }
        //public List<int> BookIds { get; set; }
        public IEnumerable<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public string Title { get; set; }
        //public List<Author> Authors { get; set; }
        public DateTime PublishedDate { get; set; }
        public IEnumerable<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        public int PatronId { get; set; }
        public int LibraryId { get; set; }
    }

    public class BookAuthor
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }

    //Structure made similar to Book - Author  Link
    public class Library
    {
        public int LibraryId { get; set; }
        public string LibName { get; set; }
        public IEnumerable<Book> LibBooks { get; set; }
    }

    public class LibraryBook
    {
        public int LibraryId { get; set; }
        public Library Library { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }

    public class Patron
    {
        public int PatronId { get; set; }
        public string PatName { get; set; }
        public IEnumerable<Book> PatronsBooks { get; set; }
    }

    public class PatronBookLib
    {
        public int PatronId { get; set; }
        public int LibraryId { get; set; }
        public int BookId { get; set; }
        public Library Library { get; set; }
        public Book Book { get; set; }
        public Patron Patron { get; set; }


    }
}
