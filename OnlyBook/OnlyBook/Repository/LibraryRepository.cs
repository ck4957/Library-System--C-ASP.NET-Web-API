using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;
using OnlyBook.Models;
using Dapper;

namespace OnlyBook.Repository
{
    public class LibraryRepository : ILibraryRepository
    {

        private readonly string _configuration;
        public LibraryRepository(IConfiguration configuration)
        {
            _configuration = configuration.GetValue<string>("DbConn:ConnectionString");
        }

        internal IDbConnection Connection => new NpgsqlConnection(_configuration);


        public Library Add(Library library)
        {
            Library AddedLibrary;
            using (IDbConnection db = Connection)
            {
                db.Open();
                db.Execute("INSERT INTO librarys (libname) VALUES(@LibName)", library);
                var libs = GetAll().Last();
                AddedLibrary = GetById(libs.LibraryId);
            }
            return AddedLibrary;
        }

        public IEnumerable<Library> GetAll()
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                var libraries = db.Query<Library>("select * from librarys").ToList();
                foreach (var lib in libraries)
                {
                    lib.LibBooks = db.Query<Book>("select * from books where libraryid=" + lib.LibraryId);
                }
                return libraries.ToList();
            }
        }

            public Library GetById(int id)
        {
            var library = new Library();
            using (IDbConnection db = Connection)
            {
                db.Open();
                library = db.Query<Library>("SELECT * FROM librarys WHERE libraryid = @Id", new { Id = id }).FirstOrDefault();
                if (library != null)
                {
                    library.LibBooks = db.Query<Book>("select * from books where libraryid=" + library.LibraryId);
                }
            }
            return library;
        }

        public void Delete(Library library)
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                db.Execute("delete FROM librarys WHERE libraryid = @Id", new { Id = library.LibraryId });
            }
        }

        public void Update(Library library)
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                db.Execute("update librarys set libname = @LibName where libraryid=@LibraryId", library);
            }
        }
    }
}
