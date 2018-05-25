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
    public class PatronRepository : IPatronRepository
    {
        private readonly string _configuration;
        public PatronRepository(IConfiguration configuration)
        {
            _configuration = configuration.GetValue<string>("DbConn:ConnectionString");
        }

        internal IDbConnection Connection => new NpgsqlConnection(_configuration);

        public Patron Add(Patron patron)
        {
            Patron AddedPatron;
            using (IDbConnection db = Connection)
            {
                db.Open();
                db.Execute("INSERT INTO patrons (patname) VALUES(@PatName)", patron);
                var patrons= GetAll().Last();
                AddedPatron = GetById(patrons.PatronId);
            }
            return AddedPatron;

        }

        public void Delete(Patron patron)
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                db.Execute("delete FROM patrons WHERE patronid = @Id", new { Id = patron.PatronId });
            }
        }

        public IEnumerable<Patron> GetAll()
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                var patrons = db.Query<Patron>("select * from patrons").ToList();
                //var patronbookList = new List<Patron>();
                foreach (var pat in patrons)
                {
                   pat.PatronsBooks = db.Query<Book>("select * from books where patronid=" + pat.PatronId);
                }

                return patrons.ToList();
            }
        }
        public Patron GetById(int id)
        {
            var patron = new Patron();
            using (IDbConnection db = Connection)
            {
                db.Open();
                patron = db.Query<Patron>("SELECT * FROM patrons WHERE patronid = @Id", new { Id = id }).FirstOrDefault();
                if (patron != null)
                {
                    patron.PatronsBooks = db.Query<Book>("select * from books where patronid="+patron.PatronId);
                }
            }
            return patron;
        }

        public void Update(Patron patron)
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                db.Execute("update patrons set patname = @PatName where patronid=@PatronId", patron);
            }
        }

        public void Checkout(int patronid, int bookid)
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                var book = db.Query<Book>("select patronid from books where bookid=@BookId",new{BookId = bookid}).FirstOrDefault();
                if (book!=null)
                    db.Execute("update books set patronid = @PatronId where bookid=@BookId", new{PatronId = patronid,BookId = bookid});
            }
        }

        public void BookReturn(int patronid, int bookid)
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                var book = db.Query<Book>("select * from books where bookid=@BookId", new { BookId = bookid }).FirstOrDefault();
                if (book != null)
                    db.Execute("update books set patronid = null where bookid=@BookId", new { BookId = bookid });
            }
        }
    }
}
