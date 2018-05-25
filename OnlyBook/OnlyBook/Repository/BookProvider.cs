using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using OnlyBook.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OnlyBook.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly string _configuration;
        //private readonly NpgsqlConnection _npgconn;
        public BookRepository(IConfiguration configuration)
        {
            _configuration = configuration.GetValue<string>("DbConn:ConnectionString");
        }
        
        internal IDbConnection Connection => new NpgsqlConnection(_configuration);

        public Book Add(Book book)
        {
            Book AddedBook;
            using (IDbConnection db = Connection)
            {
                db.Open();
                db.Execute("INSERT INTO books (publisheddate,title) VALUES(@PublishedDate,@Title)", book);
                var books = GetAll().Last();
                AddedBook = GetById(books.BookId);
            }
            return AddedBook;
        }

        public IEnumerable<Book> GetAll()
        {
            var books = new List<Book>();
            using (IDbConnection db = Connection)
            {
                db.Open();
                books = db.Query<Book>("select * from books").ToList();
                var bookauthorList = new List<BookAuthor>();
                foreach (var book in books)
                {
                    var bookauthor =
                        db.Query<BookAuthor>("select * from bookauthor where bookid =" + book.BookId);
                    foreach (var bkauthor in bookauthor)
                    {
                        var authorsList = Connection.Query<Author>("Select * from authors where authorid=" + bkauthor.AuthorId).ToList();
                        foreach (var author in authorsList)
                        {
                            bkauthor.AuthorId = author.AuthorId;
                            bkauthor.Author = author;
                        }
                        bookauthorList.Add(bkauthor);
                    }
                    //book.Authors = new List<Author>(Connection.Query<Author>("select * from authors, bookauthor where bookauthor.bookid = " + book.BookId + " and bookauthor.authorid = authors.authorid"));
                    book.BookAuthors = bookauthorList;
                    bookauthorList = new List<BookAuthor>();

                }
                return books;
            }
        }

        public Book GetById(int id)
        {
            var book = new Book();
            using (IDbConnection db = Connection)
            {
                db.Open();
                var bookauthorList = new List<BookAuthor>();
                book = db.Query<Book>("SELECT * FROM books WHERE bookid = @Id", new {Id = id}).FirstOrDefault();
                if (book != null)
                {
                    var bookauthor =
                        db.Query<BookAuthor>("select * from bookauthor where bookid =" + book.BookId);
                    foreach (var bkauthor in bookauthor)
                    {
                        var authorsList = Connection
                            .Query<Author>("Select * from authors where authorid=" + bkauthor.AuthorId)
                            .ToList();
                        foreach (var author in authorsList)
                        {
                            bkauthor.AuthorId = author.AuthorId;
                            bkauthor.Author = author;
                        }
                        bookauthorList.Add(bkauthor);
                    }
                    book.BookAuthors = bookauthorList;

                }
            }
            return book;
        }

        public void Delete(Book book)
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                db.Execute("delete FROM books WHERE bookid = @Id", new { Id = book.BookId });
            }
        }

        public void Update(Book book)
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                db.Execute("update books set title = @Title, publisheddate=@PublishedDate where bookid=@BookId",book);
            }
        }
    }
}
