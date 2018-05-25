using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Internal.Http;
using OnlyBook.Models;
using OnlyBook.Repository;

namespace OnlyBook.Controllers
{
    [Route("[controller]")]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            return _bookRepository.GetAll();
        }
        [HttpGet("{id}", Name = "GetBook")]
        public IActionResult Get(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }
        [HttpPost]
        public IActionResult Post([FromBody]Book value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var book =_bookRepository.Add(value);
            return CreatedAtRoute("GetBook", new { id = book.BookId }, book);

        }

        // PUT book/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Book value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var note = _bookRepository.GetById(id);
            if (note == null)
            {
                return NotFound();
            }
            value.BookId = id;
            _bookRepository.Update(value);
            var response = new { MessageBody = "Successfully Updated" };
            return Ok(response);
        }
        // DELETE book/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            _bookRepository.Delete(book);
            var response = new {MessageBody = "Successfully Deleted"};
            return Ok(response);
        }
    }
}
