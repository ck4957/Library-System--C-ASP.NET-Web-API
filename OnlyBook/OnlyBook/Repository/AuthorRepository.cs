using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using OnlyBook.Models;

namespace OnlyBook.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly string _configuration;
        public AuthorRepository(IConfiguration configuration)
        {
            _configuration = configuration.GetValue<string>("DbConn:ConnectionString");
        }

        internal IDbConnection Connection => new NpgsqlConnection(_configuration);

        public Author Add(Author author)
        {
            Author AddedAuthor;
            using (IDbConnection db = Connection)
            {
                db.Open();
                db.Execute("INSERT INTO authors (fname,lname) VALUES(@FName,@LName)", author);
                var authors = GetAll().Last();
                AddedAuthor = GetById(authors.AuthorId);
            }
            return AddedAuthor;
        }

        public IEnumerable<Author> GetAll()
        {
            var authors = new List<Author>();
            using (IDbConnection db = Connection)
            {
                
                Connection.Open();
                authors = Connection.Query<Author>("select * from authors").ToList();
                var bookauthorList = new List<BookAuthor>();
                foreach (var author in authors)
                {
                    var bookauthor =
                        db.Query<BookAuthor>("select * from bookauthor where authorid =" + author.AuthorId);
                    foreach (var bkauthor in bookauthor)
                    {
                        var booksList = db.Query<Book>("Select * from books where bookid=" + bkauthor.BookId).ToList();
                        foreach (var book in booksList)
                        {
                            bkauthor.BookId = book.BookId;
                            bkauthor.Book = book;
                        }
                        bookauthorList.Add(bkauthor);
                    }
                    //book.Authors = new List<Author>(Connection.Query<Author>("select * from authors, bookauthor where bookauthor.bookid = " + book.BookId + " and bookauthor.authorid = authors.authorid"));
                    author.BookAuthors = bookauthorList;
                    bookauthorList = new List<BookAuthor>();
                }

                return authors;
            }
        }

        public Author GetById(int id)
        {
            var author = new Author();
            using (IDbConnection db = Connection)
            {
                db.Open();
                var bookauthorList = new List<BookAuthor>();
                author = db.Query<Author>("SELECT * FROM authors WHERE authorid = @Id", new {Id = id}).FirstOrDefault();
                if (author != null)
                {
                    var bookauthor =
                        Connection.Query<BookAuthor>("select * from bookauthor where authorid =" + author.AuthorId);
                    foreach (var bkauthor in bookauthor)
                    {
                        var booksList = Connection.Query<Book>("Select * from books where bookid=" + bkauthor.BookId)
                            .ToList();
                        foreach (var book in booksList)
                        {
                            bkauthor.BookId = book.BookId;
                            bkauthor.Book = book;
                        }
                        bookauthorList.Add(bkauthor);
                    }
                    author.BookAuthors = bookauthorList;
                }
            }
            return author;
        }

        public void Delete(Author author)
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                db.Execute("delete FROM authors WHERE authorid = @Id", new { Id = author.AuthorId });
            }
        }

        public void Update(Author author)
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                db.Execute("update authors set fname = @FName, lname=@LName where authorid=@AuthorId", author);
            }
        }

        public Author GetByName(string authorName)
        {
            var author = new Author();
            using (IDbConnection db = Connection)
            {
                db.Open();
                var bookauthorList = new List<BookAuthor>();
                author = db.Query<Author>("SELECT * FROM authors WHERE fname Like '%"+authorName+ "%' or lname like '%" + authorName + "%'").FirstOrDefault();
                if (author != null)
                {
                    var bookauthor =
                        Connection.Query<BookAuthor>("select * from bookauthor where authorid =" + author.AuthorId);
                    foreach (var bkauthor in bookauthor)
                    {
                        var booksList = Connection.Query<Book>("Select * from books where bookid=" + bkauthor.BookId)
                            .ToList();
                        foreach (var book in booksList)
                        {
                            bkauthor.BookId = book.BookId;
                            bkauthor.Book = book;
                        }
                        bookauthorList.Add(bkauthor);
                    }
                    author.BookAuthors = bookauthorList;
                }
            }
            return author;
        }

        public void AddBooktoAuthor(Author author)
        {
            using (IDbConnection db = Connection)
            {

                //var oneAuthor = db.Query<Author>("SELECT * FROM authors WHERE authorid = @Id", new {Id = author.AuthorId})
                //    .FirstOrDefault();

                if (author.BookAuthors.Any())
                {
                    foreach (var val in author.BookAuthors)
                    {
                        var book = db.Query<Book>("select * from books where bookid=" + val.BookId);
                        if (book != null)
                        {
                            db.Execute("Insert into bookauthor(bookid,authorid) values(@BookId,@AuthorId)",
                                new BookAuthor {BookId = val.BookId, AuthorId = author.AuthorId});
                        }
                    }
                }
            }
        }
    }
}
